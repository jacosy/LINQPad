<Query Kind="Program">
  <NuGetReference>OpenPop.NET</NuGetReference>
  <NuGetReference>WTG_SMTP</NuGetReference>
  <Namespace>OpenPop.Mime</Namespace>
  <Namespace>OpenPop.Mime.Decode</Namespace>
  <Namespace>OpenPop.Mime.Header</Namespace>
  <Namespace>OpenPop.Mime.Traverse</Namespace>
  <Namespace>WTG.SMTP</Namespace>
  <Namespace>WTG.SMTP.Client</Namespace>
  <Namespace>WTG.SMTP.DNS</Namespace>
  <Namespace>WTG.SMTP.DSN</Namespace>
  <Namespace>WTG.SMTP.Helpers</Namespace>
  <Namespace>WTG.SMTP.Log</Namespace>
  <Namespace>WTG.SMTP.Mail</Namespace>
  <Namespace>WTG.SMTP.Session</Namespace>
  <Namespace>WTG.SMTP.Settings</Namespace>
</Query>

void Main()
{
	SmtpServer server = new SmtpServer("localhost", System.Net.IPAddress.Any);
	server.DeliverySucceeded += (sender, args) =>
	{
		Debugger.Break();
	};
	server.StartListener(25);
	server.MailReceived += (sender, args) =>
	{
		var bytes = Encoding.UTF8.GetBytes(args.Mail.RawMailData.Dump());
		var message = new Message(bytes);
		message.Dump("Message").ToMailMessage().Dump("MailMessage");
	};
	Console.ReadLine();
	server.Stop();
	Math.Abs(Guid.NewGuid())
}

// Define other methods and classes here