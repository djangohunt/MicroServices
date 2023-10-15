using MessageProcessing;
using MessageProcessing.Strategies.AircraftMessageProcessing;

namespace PlatformInterface
{
	public enum MessageType
	{
		Mav1,
		Mav2,
	}

	internal class AircraftMessageProcessor
	{
		public NonStrategyMessageParser NonStrategyMessageParser { get; set; }
		public IMessageParserStrategy MessageParser  { get; }

		public AircraftMessageProcessor(MessageType messageType)
		{
			MessageParser = IMessageParserStrategy.GetMessageParserFor(messageType);
		}

		public List<Message> ProcessedMessages { get; } = new();

		public void ProcessMessage(string message)
		{
			MessageParser.Parse(message);
		}
	}

	public class Message
	{
		public string Body { get; }
		public Message(string body) {  Body = body; }
	}
}
