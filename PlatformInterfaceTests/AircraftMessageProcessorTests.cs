using PlatformInterface;

namespace PlatformInterfaceTests
{
	public class AircraftMessageProcessorTests
	{
		private const string MessageBody = "Test";

		[Theory]
		[InlineData(MessageType.Mav1, $"Mavlink 1: {MessageBody}")]
		[InlineData(MessageType.Mav2, $"Mavlink 2: {MessageBody}")]
		public void MessageIsProcessedByType(MessageType messageType, string newMessage)
		{
			NonStrategyMessageParser nonStrategyMessageParser = new(messageType);
			Message parsedMessage = nonStrategyMessageParser.Parse(newMessage);

			Assert.True(parsedMessage.Body == MessageBody);
		}

		[Theory]
		[InlineData(MessageType.Mav1, "Test")]
		public void AircraftMessageIsProcessed(MessageType messageType, string newMessage)
		{
			AircraftMessageProcessor aircraftMessageProcessor = new(messageType);
			aircraftMessageProcessor.ProcessMessage(newMessage);

			Message? processedMessage = aircraftMessageProcessor.ProcessedMessages.FirstOrDefault(message => message.Body == "Test");

			Assert.NotNull(processedMessage);
			Assert.Contains(processedMessage, aircraftMessageProcessor.ProcessedMessages);
		}
	}
}