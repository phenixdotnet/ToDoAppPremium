using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ToDoAppPremium;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ToDoService>();
builder.Services.Configure<AspNetCoreInstrumentationOptions>(options => options.RecordException = true);

builder.Logging.AddLoki();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.WebHost.UseSentry(o => {
    o.Dsn = "https://80c7571e241c4c939dc206bb18859b04@o4505384980250624.ingest.sentry.io/4505384982872064";
    o.IncludeActivityData = false;
});

builder.Services
        .AddOpenTelemetry()
        .WithTracing(o =>
        {
            o.SetSampler(new TraceIdRatioBasedSampler(1.0))
             .AddSource(TelemetryConstants.ServiceName)
             .ConfigureResource(r => r.AddService(TelemetryConstants.ServiceName))
             .AddAspNetCoreInstrumentation(option => option.RecordException = true);
            o.AddConsoleExporter(o => o.Targets = ConsoleExporterOutputTargets.Console);
            o.AddJaegerExporter();
        })
        .WithMetrics((obj) => {
            obj.AddAspNetCoreInstrumentation();
            obj.AddPrometheusExporter();
        }) ;


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSentryTracing();


app.UseAuthorization();

app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

