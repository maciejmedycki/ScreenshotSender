using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using ScreenshotSender.Model;
using ScreenshotSender.Model.Interface;
using System.Windows.Input;

namespace ScreenshotSender.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(MainViewModel));
        private readonly ActionsViewModel _actionsViewModel;
        private readonly EmailSettingsViewModel _emailSettingsViewModel;
        private readonly IScreenshotTaker _screenshotTaker;
        private readonly System.Threading.Timer _sendTimer;
        private readonly ISettingsHandler _settingsHandler;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly IWindowMinimizer _windowMinimizer;
        private bool _isWorking = false;
        private ICommand _startStop;
        private string _startStopButtonContent = LocalizedStrings.MainWindowStartCaption;
        private ICommand _takeScreenshot;

        public MainViewModel(SettingsViewModel settingsViewModel, EmailSettingsViewModel emailSettingsViewModel, ActionsViewModel actionsViewModel,
            ISettingsHandler settingsHandler, IScreenshotTaker screenshotTaker, IWindowMinimizer windowMinimizer)
        {
            //TODO: argExceptoin when null
            _settingsHandler = settingsHandler;
            _screenshotTaker = screenshotTaker;
            _actionsViewModel = actionsViewModel;
            _settingsViewModel = settingsViewModel;
            _emailSettingsViewModel = emailSettingsViewModel;
            _windowMinimizer = windowMinimizer;
            _sendTimer = new System.Threading.Timer(SendTimerCallback);
            if (_settingsViewModel.AutoStart)
            {
                Start();
            }
        }

        public ActionsViewModel ActionsViewModel
        {
            get { return _actionsViewModel; }
        }

        public EmailSettingsViewModel EmailSettingsViewModel
        {
            get { return _emailSettingsViewModel; }
        }

        public SettingsViewModel SettingsViewModel
        {
            get { return _settingsViewModel; }
        }

        public ICommand StartStop
        {
            get
            {
                if (_startStop == null)
                {
                    _startStop = new RelayCommand(DoStartStop);
                }
                return _startStop;
            }
        }

        public string StartStopButtonContent
        {
            get
            {
                return _startStopButtonContent;
            }
            set
            {
                _startStopButtonContent = value;
                RaisePropertyChanged(() => StartStopButtonContent);
            }
        }

        public ICommand TakeScreenshot
        {
            get
            {
                if (_takeScreenshot == null)
                {
                    _takeScreenshot = new RelayCommand(DoTakeScreenshot);
                }
                return _takeScreenshot;
            }
        }

        public string Title { get; set; } = LocalizedStrings.ScreenshotSenderTitle;

        public void DoStartStop()
        {
            if (_isWorking)
            {
                Stop();
            }
            else
            {
                Start();
            }
        }

        public void DoTakeScreenshot()
        {
            _screenshotTaker?.TakeScreenshot();
        }

        private void SendTimerCallback(object state)
        {
            _logger.Debug("SendTimerCallback tick");
            DoTakeScreenshot();
        }

        private void Start()
        {
            _isWorking = true;
            _windowMinimizer.MinimizeWindow();
            StartStopButtonContent = LocalizedStrings.MainWindowStopCaption;            
            _sendTimer.Change(2000, SettingsViewModel.CheckInterval * 1000);
        }

        private void Stop()
        {
            _isWorking = false;
            StartStopButtonContent = LocalizedStrings.MainWindowStartCaption;
            _sendTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        }
    }
}