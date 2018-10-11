using GalaSoft.MvvmLight;
using log4net;

namespace ScreenshotSender.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(MainViewModel));

        public MainViewModel()
        {
            _logger.Debug("MainViewModel ctor");
        }

        public string Title { get; set; } = "Screenshot Sender";
    }
}