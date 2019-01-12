using GalaSoft.MvvmLight.Messaging;
using log4net;
using ScreenshotSender.Model.Messages;
using System;

namespace ScreenshotSender.Model.Actions
{
    public class BitmapSaver : BaseAction
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BitmapSaver));

        public BitmapSaver()
        {
            Messenger.Default?.Register<ScreenShotTakenMessage>(this, SaveScreenBitmapToFile);
        }

        public override string Name => "Save to file";
        public override bool ShouldExecute { get; set; }

        private void SaveScreenBitmapToFile(ScreenShotTakenMessage screenShotTakenMessage)
        {
            if (ShouldExecute)
            {
                var now = DateTime.Now;
                var deviceName = screenShotTakenMessage.DeviceName;
                var datePart = now.ToString("yyyy.MM.dd HH.mm.ss.fff");
                var filePath = datePart + "_" + deviceName + ".bmp";
                screenShotTakenMessage.Bitmap.Save(filePath);
            }
        }

        ~BitmapSaver()
        {
            Messenger.Default?.Unregister<ScreenShotTakenMessage>(this, SaveScreenBitmapToFile);
        }
    }
}