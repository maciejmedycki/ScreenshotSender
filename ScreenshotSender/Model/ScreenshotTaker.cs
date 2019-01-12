using GalaSoft.MvvmLight.Messaging;
using log4net;
using ScreenshotSender.Model.Interface;
using ScreenshotSender.Model.Messages;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ScreenshotSender.Model
{
    public class ScreenshotTaker : IScreenshotTaker
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ScreenshotTaker));
        private readonly IDisplayHandler _displayHandler;

        public ScreenshotTaker(IDisplayHandler displayHandler)
        {
            _displayHandler = displayHandler;
        }

        public void TakeScreenshot()
        {
            if (Screen.AllScreens != null)
            {
                foreach (var screen in _displayHandler.Get().Where(d => d.Selected))
                {
                    var left = screen.X;
                    var top = screen.Y;
                    var width = screen.Width;
                    var height = screen.Height;
                    using (var bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        using (var bmpGraphics = Graphics.FromImage(bitmap))
                        {
                            bmpGraphics.CopyFromScreen(left, top, 0, 0, new Size(width, height));
                            Messenger.Default?.Send(new ScreenShotTakenMessage(screen.FriendlyName, bitmap));
                            //TODO:https://stackoverflow.com/questions/1546091/wpf-createbitmapsourcefromhbitmap-memory-leak
                        }
                    }
                }
            }
            else
            {
                _logger.Warn("There are no screens to take screenshot from.");
            }
        }
    }
}