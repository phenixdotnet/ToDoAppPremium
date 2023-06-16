using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ToDoBackend;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<AspNetCoreInstrumentationOptions>(options => options.RecordException = true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapControllers();

app.Run();

