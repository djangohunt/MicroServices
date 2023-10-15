using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformInterface
{
	public enum MessageSource
	{

	}

	internal class AircraftMessage
	{
		public MessageSource Source { get; set; }
		public string Name { get; set; }
		public string Body { get; set; }
	}
}
