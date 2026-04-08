using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace INTEX2026.Tests;

/// <summary>
/// Covers the manual checklist: /me contract, login/logout, password length, 401/403, wrong login,
/// CORS header for allowed origin, CSP present, donor forbidden on admin API.
/// </summary>
public class AuthIntegrationTests : IClassFixture<HavynApiFactory>
{
    private readonly HavynApiFactory _factory;

    public AuthIntegrationTests(HavynApiFactory factory) => _factory = factory;

    private HttpClient CreateClient() =>
        _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    [Fact]
    public async Task Get_me_while_logged_out_returns_200_with_isAuthenticated_false()
    {
        using var client = CreateClient();
        var response = await client.GetAsync("/api/auth/me");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        Assert.False(root.GetProperty("isAuthenticated").GetBoolean());
        Assert.Equal(JsonValueKind.Null, root.GetProperty("userName").ValueKind);
        Assert.Equal(JsonValueKind.Null, root.GetProperty("email").ValueKind);
        Assert.Equal(0, root.GetProperty("roles").GetArrayLength());
    }

    [Fact]
    public async Task Login_then_me_then_logout_then_me_anonymous()
    {
        using var client = CreateClient();
        var login = await client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "admin@havyn.org", password = "TempPass!12345" });
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);

        var meAuth = await client.GetFromJsonAsync<JsonElement>("/api/auth/me");
        Assert.True(meAuth.GetProperty("isAuthenticated").GetBoolean());
        Assert.Equal("admin@havyn.org", meAuth.GetProperty("email").GetString());

        var logout = await client.PostAsync("/api/auth/logout", null);
        Assert.Equal(HttpStatusCode.OK, logout.StatusCode);

        var meOut = await client.GetFromJsonAsync<JsonElement>("/api/auth/me");
        Assert.False(meOut.GetProperty("isAuthenticated").GetBoolean());
    }

    [Fact]
    public async Task Post_login_wrong_password_returns_401()
    {
        using var client = CreateClient();
        var response = await client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "admin@havyn.org", password = "WrongPassword!!!!" });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Protected_dashboard_admin_without_cookie_returns_401()
    {
        using var client = CreateClient();
        var response = await client.GetAsync("/api/dashboard/admin");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Donor_cannot_access_admin_dashboard_returns_403()
    {
        using var client = CreateClient();
        var login = await client.PostAsJsonAsync(
            "/api/auth/login",
            new { email = "donor@havyn.org", password = "TempPass!12345" });
        login.EnsureSuccessStatusCode();

        var response = await client.GetAsync("/api/dashboard/admin");
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Cors_allowed_origin_gets_access_control_allow_origin()
    {
        using var client = CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/me");
        request.Headers.TryAddWithoutValidation("Origin", "http://localhost:3000");
        var response = await client.SendAsync(request);
        Assert.True(response.Headers.TryGetValues("Access-Control-Allow-Origin", out var values));
        Assert.Contains("http://localhost:3000", values);
    }

    [Fact]
    public async Task Responses_include_content_security_policy_header()
    {
        using var client = CreateClient();
        var response = await client.GetAsync("/api/auth/me");
        response.EnsureSuccessStatusCode();
        Assert.True(response.Headers.Contains("Content-Security-Policy"));
    }

    [Fact]
    public async Task Register_donor_13_char_password_is_rejected()
    {
        using var client = CreateClient();
        var email = $"u_{Guid.NewGuid():N}@test.local";
        var response = await client.PostAsJsonAsync(
            "/api/auth/register-donor",
            new
            {
                displayName = "Test User",
                email,
                password = "1234567890123",
                acceptPrivacyPolicy = true,
            });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_donor_14_char_all_lowercase_succeeds()
    {
        using var client = CreateClient();
        var email = $"u_{Guid.NewGuid():N}@test.local";
        var response = await client.PostAsJsonAsync(
            "/api/auth/register-donor",
            new
            {
                displayName = "Test User",
                email,
                password = "abcdefghijklmn",
                acceptPrivacyPolicy = true,
            });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
