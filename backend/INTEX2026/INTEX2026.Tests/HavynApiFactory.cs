using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace INTEX2026.Tests;

/// <summary>Selects Sqlite via Program.cs test branch so only one EF database provider is registered.</summary>
public sealed class HavynApiFactory : WebApplicationFactory<Program>
{
    private readonly string _dbPath = Path.Combine(Path.GetTempPath(), $"havyn_itest_{Guid.NewGuid():N}.sqlite");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.UseSetting("CsvData:RootPath", "");
        builder.UseSetting("Cors:AllowedOrigins:0", "http://localhost:3000");
        builder.UseSetting("Testing:DatabaseProvider", "Sqlite");
        builder.UseSetting("Testing:SqlitePath", $"Data Source={_dbPath}");
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            try
            {
                File.Delete(_dbPath);
            }
            catch
            {
                // ignore locked file on some runners
            }
        }
    }
}
