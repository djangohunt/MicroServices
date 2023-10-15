using PlatformInterface;

namespace MessageProcessing.Strategies.AircraftMessageProcessing
{
	public interface IMessageParserStrategy
	{
		Message Parse(string messageToParse);

		public static IMessageParserStrategy GetMessageParserFor(MessageType messageType)
		{
			return messageType switch
			{
				MessageType.Mav1 => new Mavlink1MessageParserStrategy(),
				MessageType.Mav2 => new Mavlink2MessageParserStrategy(),
				_ => new Mavlink2MessageParserStrategy()
			};
		}
	}
}
