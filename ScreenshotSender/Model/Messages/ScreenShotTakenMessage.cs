using GalaSoft.MvvmLight.Messaging;
using System.Drawing;

namespace ScreenshotSender.Model.Messages
{
    public class ScreenShotTakenMessage : MessageBase
    {
        private ScreenShotTakenMessage()
        {
        }

        public ScreenShotTakenMessage(string deviceName, Bitmap bitmap)
        {
            DeviceName = deviceName;
            Bitmap = bitmap;
        }

        public Bitmap Bitmap { get; private set; }
        public string DeviceName { get; private set; }
    }
}