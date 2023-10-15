using MassTransit;
using WeatherMonitor;

WebApplicationBuilder builder = WebApplication.CreateBuilder();

builder.Services.AddMassTransit(busConfigurator =>
{
	busConfigurator.AddConsumers(typeof(Program).Assembly);
	busConfigurator.UsingInMemory((context, cfg) =>
	{
		cfg.ConfigureEndpoints(context);
	});
});

builder.Services.AddHostedService<PingPublisher>();

WebApplication app  = builder.Build();

app.Run();