using EngineeringSimulator.API.Middleware;
using EngineeringSimulator.Application.DTOs;
using EngineeringSimulator.Application.Services;
using EngineeringSimulator.Application.Validators;
using EngineeringSimulator.Infrastructure.Logging;
using FluentValidation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ── Serilog ──────────────────────────────────────────────────
builder.Host.UseSerilog((context, config) => config
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "EngineeringSimulatorAPI")
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {CorrelationId:l} {Message:lj}{NewLine}{Exception}"));

// ── Services ─────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Engineering Simulator API",
        Version = "v1",
        Description = "REST API for engineering calculations: dimensionless numbers and thermodynamic cycles.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Engineering Simulator",
            Url = new Uri("https://github.com/engineering-simulator-api")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

// ── DI: Application layer ────────────────────────────────────
builder.Services.AddSingleton<EngineeringCalculationService>();

// ── DI: FluentValidation ─────────────────────────────────────
builder.Services.AddScoped<IValidator<WobbeRequest>, WobbeRequestValidator>();
builder.Services.AddScoped<IValidator<ReynoldsRequest>, ReynoldsRequestValidator>();
builder.Services.AddScoped<IValidator<RayleighRequest>, RayleighRequestValidator>();
builder.Services.AddScoped<IValidator<CarnotRequest>, CarnotRequestValidator>();

var app = builder.Build();

// ── Middleware pipeline ──────────────────────────────────────
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Engineering Simulator API v1");
    c.RoutePrefix = string.Empty;
});

app.UseSerilogRequestLogging();
app.MapControllers();

app.Run();

// Required for WebApplicationFactory in integration tests
public partial class Program { }
