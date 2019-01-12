using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using ScreenshotSender.Model.Interface;
using ScreenshotSender.Model.Messages;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace ScreenshotSender.Model.Actions
{
    [Serializable]
    public class EmailSender : BaseAction
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(EmailSender));
        private readonly ILastFileCollector _lastFileCollector;
        private readonly ISettingsHandler _settingsHandler;

        public EmailSender()
            : this(SimpleIoc.Default.GetInstance<ISettingsHandler>(), SimpleIoc.Default.GetInstance<ILastFileCollector>())
        {
        }

        [PreferredConstructorAttribute]
        public EmailSender(ISettingsHandler settingsHandler, ILastFileCollector lastFileCollector)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentException("Parameter cannot be null", "settingsHandler");
            }
            _settingsHandler = settingsHandler;
            _lastFileCollector = lastFileCollector;
            Messenger.Default?.Register<ScreenShotTakenMessage>(this, SendEmailWithScreenshots);
        }

        public override string Name => "Send an email";

        public override bool ShouldExecute { get; set; }

        private void SendEmailWithScreenshots(ScreenShotTakenMessage screenShotTakenMessage)
        {
            if (ShouldExecute)
            {
                var addressFrom = new MailAddress(_settingsHandler.GetEmailFrom(), _settingsHandler.GetEmailFrom());
                var addressTo = new MailAddress(_settingsHandler.GetEmailTo());
                var mail = new MailMessage(addressFrom, addressTo);

                var memoryStreamScreenshot = new MemoryStream();
                screenShotTakenMessage.Bitmap.Save(memoryStreamScreenshot, ImageFormat.Jpeg);
                memoryStreamScreenshot.Position = 0;
                var linkredResourceScreenshot = new LinkedResource(memoryStreamScreenshot, MediaTypeNames.Image.Jpeg);

                var linkredResourceLastFile = _lastFileCollector.GetLastFile();

                //TODO: missing in ISettingsHandler
                mail.Subject = "Screenshot sender";
                //TODO: missing in ISettingsHandler
                var htmlString = $@"<html>
                      <body>
                      <p>
                        New screenshot was taken by ScreenShot sender.
                        <br/>
                        <img src='cid: { linkredResourceScreenshot.ContentId}'  />
                        #$LATESTFILELINKEDRESOURCEHOLDER$#
                      </p>
                      </body>
                      </html>
                     ";

                if (linkredResourceLastFile != null)
                {
                    htmlString.Replace("#$LATESTFILELINKEDRESOURCEHOLDER$#", $"< img src = 'cid: { linkredResourceLastFile.ContentId}' />");
                }
                else
                {
                    htmlString.Replace("#$LATESTFILELINKEDRESOURCEHOLDER$#", string.Empty);
                }
                var alternateView = AlternateView.CreateAlternateViewFromString(htmlString, null, MediaTypeNames.Text.Html);
                alternateView.LinkedResources.Add(linkredResourceScreenshot);
                if (linkredResourceLastFile != null)
                {
                    alternateView.LinkedResources.Add(linkredResourceLastFile);
                }
                mail.AlternateViews.Add(alternateView);

                var smtp = new SmtpClient();
                smtp.Host = _settingsHandler.GetEmailSmtpHost();
                smtp.Port = _settingsHandler.GetEmailSmtpPort();
                smtp.EnableSsl = _settingsHandler.GetEmailSmtpEnableSsl();
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_settingsHandler.GetEmailSmtpUserName(), _settingsHandler.GetEmailFromPassword());
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Timeout = 20000;

                smtp.Send(mail);

                //TODO: disposing!
                smtp.Dispose();
                mail.Dispose();
                //smtp.SendAsync(mail, null);
                memoryStreamScreenshot.Dispose();
                linkredResourceScreenshot.Dispose();
                if (linkredResourceLastFile != null)
                {
                    linkredResourceLastFile.Dispose();
                }
            }
        }

        ~EmailSender()
        {
            Messenger.Default?.Unregister<ScreenShotTakenMessage>(this);
        }
    }
}