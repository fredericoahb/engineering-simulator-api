using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace EngineeringSimulator.API.Tests;

public class EndpointIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EndpointIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // ── Health ────────────────────────────────────────────────

    [Fact]
    public async Task Health_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // ── Info ──────────────────────────────────────────────────

    [Fact]
    public async Task Info_ReturnsOkWithVersion()
    {
        var response = await _client.GetAsync("/api/v1/info");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("Engineering Simulator API", body);
    }

    // ── Wobbe ────────────────────────────────────────────────

    [Fact]
    public async Task Wobbe_ValidInput_ReturnsOk()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/wobbe", new
        {
            pcs = 39.0,
            relativeDensity = 0.60
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(body.GetProperty("result").GetDouble() > 0);
    }

    [Fact]
    public async Task Wobbe_ZeroPcs_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/wobbe", new
        {
            pcs = 0,
            relativeDensity = 0.60
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Wobbe_NegativeDensity_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/wobbe", new
        {
            pcs = 39.0,
            relativeDensity = -0.5
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ── Reynolds ──────────────────────────────────────────────

    [Fact]
    public async Task Reynolds_ValidInput_ReturnsOkWithClassification()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/reynolds", new
        {
            rho = 998.0,
            velocity = 1.5,
            diameter = 0.05,
            mu = 0.001
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Turbulent", body.GetProperty("classification").GetString());
    }

    [Fact]
    public async Task Reynolds_LaminarInputs_ReturnsLaminar()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/reynolds", new
        {
            rho = 1.0,
            velocity = 0.01,
            diameter = 0.01,
            mu = 0.001
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Laminar", body.GetProperty("classification").GetString());
    }

    [Fact]
    public async Task Reynolds_ZeroVelocity_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/reynolds", new
        {
            rho = 998.0,
            velocity = 0.0,
            diameter = 0.05,
            mu = 0.001
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ── Rayleigh ─────────────────────────────────────────────

    [Fact]
    public async Task Rayleigh_ValidInput_ReturnsOk()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/rayleigh", new
        {
            g = 9.81,
            beta = 3.41e-3,
            deltaT = 20.0,
            l = 0.1,
            nu = 1.56e-5,
            alpha = 2.21e-5
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(body.GetProperty("result").GetDouble() > 0);
    }

    [Fact]
    public async Task Rayleigh_ZeroAlpha_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/rayleigh", new
        {
            g = 9.81,
            beta = 3.41e-3,
            deltaT = 20.0,
            l = 0.1,
            nu = 1.56e-5,
            alpha = 0.0
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ── Carnot ───────────────────────────────────────────────

    [Fact]
    public async Task Carnot_ValidInput_ReturnsOk()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/carnot", new
        {
            th = 500.0,
            tc = 300.0
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(0.4, body.GetProperty("result").GetDouble(), precision: 4);
    }

    [Fact]
    public async Task Carnot_ThLessThanTc_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/carnot", new
        {
            th = 200.0,
            tc = 300.0
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Carnot_EqualTemperatures_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/carnot", new
        {
            th = 300.0,
            tc = 300.0
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Carnot_ZeroTh_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/v1/carnot", new
        {
            th = 0.0,
            tc = 300.0
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ── Correlation ID ───────────────────────────────────────

    [Fact]
    public async Task AllEndpoints_ReturnCorrelationIdHeader()
    {
        var response = await _client.GetAsync("/health");
        Assert.True(response.Headers.Contains("X-Correlation-Id"));
    }
}
