using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ToDoBackend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.Configure<AspNetCoreInstrumentationOptions>(options => options.RecordException = true);

        // Add services to the container.
        builder.Logging.AddLoki();

        builder.WebHost.UseSentry(o =>
        {
            o.Dsn = "https://5258989b0ff24bc29e0fb6d03b24e5e5@o4505384980250624.ingest.sentry.io/4505387448336384";
            o.IncludeActivityData = false;
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services
                .AddOpenTelemetry()
                .WithTracing(o =>
                {
                    o.AddSource(TelemetryConstants.ServiceName)
                     .ConfigureResource(r => r.AddService(TelemetryConstants.ServiceName))
                     .AddAspNetCoreInstrumentation(option =>
                     {
                         option.RecordException = false;
                         option.Filter = ExcludeMetricsFilter;
                     });
                    o.AddConsoleExporter(o => o.Targets = ConsoleExporterOutputTargets.Console);
                    o.AddJaegerExporter();
                })
                .WithMetrics(o =>
                {
                    o.AddAspNetCoreInstrumentation();
                    o.AddPrometheusExporter();
                });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");
        app.MapControllers();

        app.Run();
    }

    private static bool ExcludeMetricsFilter(HttpContext httpContext)
    {
        return httpContext.Request.Path != "/metrics";
    }
}
