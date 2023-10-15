using System.Net.Mail;
using System.Net;

namespace EmailTesting;

public class EmailSender
{
	public void SendEmail(string recipient, string subject, string body)
	{
		SmtpClient smtpClient = new("smtp.gmail.com", 587)
		{
			Credentials = null,
			DeliveryFormat = SmtpDeliveryFormat.SevenBit,
			DeliveryMethod = SmtpDeliveryMethod.Network,
			EnableSsl = false,
			PickupDirectoryLocation = null,
			TargetName = null,
			Timeout = 0,
			UseDefaultCredentials = false
		};
		smtpClient.EnableSsl = true;
		smtpClient.UseDefaultCredentials = false;
		smtpClient.Credentials = new NetworkCredential("xnc30x@googlemail.com", "cableAstro333");

		// Ignore certificate validation errors (for testing purposes only)
		ServicePointManager.ServerCertificateValidationCallback =
			(sender, certificate, chain, sslPolicyErrors) => true;

		MailMessage mailMessage = new();
		mailMessage.From = new MailAddress("xnc30x@googlemail.com");
		mailMessage.To.Add(recipient);
		mailMessage.Subject = subject;
		mailMessage.Body = body;

		try
		{
			smtpClient.Send(mailMessage);
			Console.WriteLine("Email sent successfully.");
		}
		catch (Exception ex)
		{
			Console.WriteLine("Failed to send email. Error: " + ex.Message);
		}
	}
}