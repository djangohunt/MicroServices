using MassTransit;

namespace WeatherMonitor;

public class PingConsumer : IConsumer<Ping>
{
	private readonly ILogger<PingConsumer> _logger;

	public PingConsumer(ILogger<PingConsumer> logger)
	{
		_logger = logger;
	}

	public Task Consume(ConsumeContext<Ping> context)
	{
		ConsoleKeyInfo button = Console.ReadKey(true);
		_logger.LogInformation("Pressed {Button}", button.Key.ToString());
		return Task.CompletedTask;
	}
}