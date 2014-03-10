using System.Net.Mail;

namespace RF.Common
{
	public class EMailSender
	{
		public void SendEmail(string from, string to, string subject, string body)
		{
			Args.IsNotNullOrEmpty(from, "from");
			Args.IsNotNullOrEmpty(to, "to");
			Args.IsNotNullOrEmpty(subject, "subject");
			Args.IsNotNullOrEmpty(body, "body");

			MailMessage message = Utils.CreateMailMessage(to, subject, body, true);
			message.ReplyToList.Add (new MailAddress(from));
			
			Utils.SendEmailAsync(message);

			//SmtpClient smtp = new SmtpClient();
			//smtp.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
			//smtp.EnableSsl = true;
			//smtp.Send(message);
		}
	}
}
