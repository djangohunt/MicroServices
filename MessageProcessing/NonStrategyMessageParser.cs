// -----------------------------------------------------------------------------
// Project: MessageProcessing
// File:NonStrategyMessageParser.cs
// -----------------------------------------------------------------------------
// Copyright:
// This is an unpublished work the copyright in which vests in TEKEVER Ltd.
// All rights reserved.
// 
// The information contained herein is the property of TEKEVER Ltd and is
// supplied without liability for errors or omissions. No part may be reproduced,
// disclosed or used except as authorised by contract or other written permission.
// 
// The copyright and the foregoing restriction on reproduction, disclosure and
// use extend to all media in which the information may be embodied.
// -----------------------------------------------------------------------------

namespace PlatformInterface;

public class NonStrategyMessageParser
{
	private MessageType _messageType;
	private const string Mav1Id = "Mavlink 1: ";
	private const string Mav2Id = "Mavlink 2: ";

	public NonStrategyMessageParser(MessageType messageType)
	{
		_messageType = messageType;
	}

	public Message Parse(string newMessage)
	{
		if (_messageType == MessageType.Mav1)
		{
			return new Message(newMessage.Replace(Mav1Id, ""));
		}
		else
		{
			return new Message(newMessage.Replace(Mav2Id, ""));
		}
	}
}