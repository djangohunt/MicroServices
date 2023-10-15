using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformInterface
{
	public enum AircraftType
	{
		AR3,
		AR4,
		AR5
	}

	public class AircraftMessageProcessor
	{
		public Dictionary<AircraftType, Message> ProcessedMessagesByAircraftType { get; } = new();

		public void ProcessMessage(AircraftType aircraftType)
		{
			throw new NotImplementedException();
		}
	}

	public class Message
	{
		public string Body { get; }
		public Message(string body) {  Body = body; }
	}
}
