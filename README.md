# EmailService
Helper library to send e-mail messages.

Usage:
First you have to define your SMTP server.
var smtpServer = new SmtpServer("mail.digikabel.hu", 25, false, "user@digikabel.hu", "pass", null, true);

Then create a message sender.
var sendMail = new SendMail(smtpServer);

Optionally you can check what happened with your e-mail.
sendMail.SentChanged += SendMail_SentChanged;

And finally, you just send your message.
sendMail.Send("sender@email.address.hu", "receiver@email.address.hu", "Title of e-mail", "Content of e-mail");
