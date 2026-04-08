using INTEX2026.Authorization;
using INTEX2026.Data;
using INTEX2026.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var useSqliteIntegrationTest =
    builder.Environment.IsEnvironment("Testing")
    && string.Equals(builder.Configuration["Testing:DatabaseProvider"], "Sqlite", StringComparison.OrdinalIgnoreCase);

if (useSqliteIntegrationTest)
{
    var sqlitePath = builder.Configuration["Testing:SqlitePath"];
    if (string.IsNullOrWhiteSpace(sqlitePath))
        throw new InvalidOperationException("Testing:SqlitePath is required for integration tests (Sqlite).");
    builder.Services.AddDbContext<HavynDbContext>(options => options.UseSqlite(sqlitePath));
}
else
{
    // Build connection string from environment variables, falling back to appsettings
    var pgHost = Environment.GetEnvironmentVariable("PGHOST") ?? builder.Configuration["Database:PGHOST"] ?? "localhost";
    var pgPort = Environment.GetEnvironmentVariable("PGPORT") ?? builder.Configuration["Database:PGPORT"] ?? "5432";
    var pgDatabase = Environment.GetEnvironmentVariable("PGDATABASE") ?? builder.Configuration["Database:PGDATABASE"] ?? "havyn";
    var pgUser = Environment.GetEnvironmentVariable("PGUSER") ?? builder.Configuration["Database:PGUSER"] ?? "postgres";
    var pgPassword = Environment.GetEnvironmentVariable("PGPASSWORD") ?? builder.Configuration["Database:PGPASSWORD"] ?? "";

    // For development, disable SSL mode; for production, use 'require'
    var sslMode = Environment.GetEnvironmentVariable("SSL_MODE") ?? builder.Configuration["Database:SSL_MODE"] ?? "require";
    var connectionString = $"Host={pgHost};Port={pgPort};Database={pgDatabase};Username={pgUser};Password={pgPassword};SSL Mode={sslMode};";
    builder.Services.AddDbContext<HavynDbContext>(options => options.UseNpgsql(connectionString));
}

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // New/changed passwords only — does not invalidate existing hashes (see project auth guidelines).
        options.Password.RequiredLength = 14;
        options.Password.RequiredUniqueChars = 1;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<HavynDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "havyn.auth";
    options.Cookie.HttpOnly = true;
    if (builder.Environment.IsEnvironment("Testing"))
    {
        // Integration tests use HTTP + WebApplicationFactory; Secure+None cookies are not sent by HttpClient.
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
    }
    else
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        // Cross-origin SPA (e.g. Azure Static Web Apps → API on another host) requires None + Secure.
        options.Cookie.SameSite = SameSiteMode.None;
    }

    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthPolicies.RequireStaff, p =>
        p.RequireRole(AuthRoles.ExecutiveAdmin, AuthRoles.RegionalManager, AuthRoles.SocialWorker));
    options.AddPolicy(AuthPolicies.ExecutiveOrRegional, p =>
        p.RequireRole(AuthRoles.ExecutiveAdmin, AuthRoles.RegionalManager));
    options.AddPolicy(AuthPolicies.ExecutiveAdminOnly, p => p.RequireRole(AuthRoles.ExecutiveAdmin));
    options.AddPolicy(AuthPolicies.RegionalManagerOnly, p => p.RequireRole(AuthRoles.RegionalManager));
    options.AddPolicy(AuthPolicies.SocialWorkerOnly, p => p.RequireRole(AuthRoles.SocialWorker));
    options.AddPolicy(AuthPolicies.DonorOnly, p => p.RequireRole(AuthRoles.Donor));
    options.AddPolicy(AuthPolicies.StaffOrDonor, p =>
        p.RequireRole(
            AuthRoles.ExecutiveAdmin,
            AuthRoles.RegionalManager,
            AuthRoles.SocialWorker,
            AuthRoles.Donor));
});

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] { "http://localhost:3000", "http://localhost:5173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(corsOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHsts();
}

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}
app.UseCors("Frontend");

var cspExtras = builder.Configuration.GetSection("Security:CspConnectSrcExtra").Get<string[]>() ?? Array.Empty<string>();
app.Use(async (context, next) =>
{
    var connectParts = new List<string> { "'self'" };
    connectParts.AddRange(corsOrigins);
    connectParts.AddRange(cspExtras);
    var connectSrc = string.Join(" ", connectParts.Distinct(StringComparer.Ordinal));

    var csp = app.Environment.IsDevelopment()
        ? "default-src 'self'; " +
          "script-src 'self' 'unsafe-inline'; " +
          "style-src 'self' 'unsafe-inline'; " +
          "img-src 'self' data:; " +
          "font-src 'self'; " +
          $"connect-src {connectSrc}; " +
          "frame-ancestors 'none';"
        : "default-src 'self'; " +
          "script-src 'self'; " +
          "style-src 'self'; " +
          "img-src 'self' data:; " +
          "font-src 'self'; " +
          $"connect-src {connectSrc}; " +
          "frame-ancestors 'none';";

    context.Response.Headers.ContentSecurityPolicy = csp;
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<HavynDbContext>();

    await db.Database.EnsureCreatedAsync();

    await RoleSeedService.SeedAsync(services);
    await CsvSeedService.SeedAsync(db, builder.Configuration);
}

app.Run();

/// <summary>Exposes Program to integration tests (<see cref="Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactory{TEntryPoint}"/>).</summary>
public partial class Program;
