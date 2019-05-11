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
            try
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

                    LinkedResource linkedResourceLastFile = null;
                    MemoryStream memoryStreamLastFile = null;
                    var latestFileInfo = _lastFileCollector.GetLastFile();
                    if (latestFileInfo != null)
                    {
                        memoryStreamLastFile = new MemoryStream(File.ReadAllBytes(latestFileInfo.FullName));
                        memoryStreamLastFile.Position = 0;
                        var mimeType = MimeTypes.GetMimeType(latestFileInfo.Name);
                        linkedResourceLastFile = new LinkedResource(memoryStreamLastFile, mimeType);
                    }

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

                    if (linkedResourceLastFile != null && linkedResourceLastFile.ContentType.MediaType.Contains("image"))
                    {
                        htmlString = htmlString.Replace("#$LATESTFILELINKEDRESOURCEHOLDER$#", $"<img src = 'cid: { linkedResourceLastFile.ContentId}' />");
                    }
                    else
                    {
                        htmlString = htmlString.Replace("#$LATESTFILELINKEDRESOURCEHOLDER$#", string.Empty);
                    }
                    var alternateView = AlternateView.CreateAlternateViewFromString(htmlString, null, MediaTypeNames.Text.Html);
                    alternateView.LinkedResources.Add(linkredResourceScreenshot);
                    if (linkedResourceLastFile != null)
                    {
                        if (linkedResourceLastFile.ContentType.MediaType.Contains("image"))
                        {
                            alternateView.LinkedResources.Add(linkedResourceLastFile);
                        }
                        else
                        {
                            mail.Attachments.Add(new Attachment(latestFileInfo.FullName));
                        }
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
                    if (linkedResourceLastFile != null)
                    {
                        linkedResourceLastFile.Dispose();
                        memoryStreamLastFile.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(LocalizedStrings.EmailSenderErrorMessage, ex);
            }
        }

        ~EmailSender()
        {
            Messenger.Default?.Unregister<ScreenShotTakenMessage>(this);
        }
    }
}