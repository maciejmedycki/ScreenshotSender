using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using log4net;
using ScreenshotSender.Model;
using ScreenshotSender.Model.Interface;
using System;
using System.Linq;
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
        private string _lastLogEntry;
        private ICommand _startStop;
        private string _startStopButtonContent = LocalizedStrings.MainWindowStartCaption;
        private ICommand _takeScreenshot;

        public MainViewModel(SettingsViewModel settingsViewModel, EmailSettingsViewModel emailSettingsViewModel, ActionsViewModel actionsViewModel,
            ISettingsHandler settingsHandler, IScreenshotTaker screenshotTaker, IWindowMinimizer windowMinimizer)
        {
            _settingsViewModel = settingsViewModel ?? throw new ArgumentException("Parameter cannot be null", nameof(settingsViewModel));
            _emailSettingsViewModel = emailSettingsViewModel ?? throw new ArgumentException("Parameter cannot be null", nameof(emailSettingsViewModel));
            _actionsViewModel = actionsViewModel ?? throw new ArgumentException("Parameter cannot be null", nameof(actionsViewModel));
            _settingsHandler = settingsHandler ?? throw new ArgumentException("Parameter cannot be null", nameof(settingsHandler));
            _screenshotTaker = screenshotTaker ?? throw new ArgumentException("Parameter cannot be null", nameof(screenshotTaker));
            _windowMinimizer = windowMinimizer ?? throw new ArgumentException("Parameter cannot be null", nameof(windowMinimizer));
            _sendTimer = new System.Threading.Timer(SendTimerCallback);
            AttachToLogApender();
            if (_settingsViewModel.AutoStart)
            {
                Start();
            }
            _logger.Debug("Started");
        }

        public ActionsViewModel ActionsViewModel
        {
            get { return _actionsViewModel; }
        }

        public EmailSettingsViewModel EmailSettingsViewModel
        {
            get { return _emailSettingsViewModel; }
        }

        public string LastLogEntry
        {
            get
            {
                return _lastLogEntry;
            }
            private set
            {
                _lastLogEntry = value;
                RaisePropertyChanged(() => LastLogEntry);
            }
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

        private void AttachToLogApender()
        {
            var appenders = LogManager.GetRepository().GetAppenders().Where(a => a.GetType() == typeof(LogAppender));
            if (appenders != null)
            {
                foreach (var appender in appenders)
                {
                    var logsAppender = appender as LogAppender;
                    if (logsAppender != null)
                    {
                        logsAppender.LogAdded += LogAdded;
                    }
                }
            }
        }

        private void LogAdded(string message)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                var now = DateTime.Now;
                LastLogEntry = $"{now.ToShortDateString()} {now.ToLongTimeString()} {message}";
            });
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