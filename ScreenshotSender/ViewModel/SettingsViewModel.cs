using GalaSoft.MvvmLight;
using log4net;
using ScreenshotSender.Model.Interface;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ScreenshotSender.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SettingsViewModel));
        private readonly IDisplayHandler _displayHandler;
        private readonly ISettingsHandler _settingsHandler;
        private bool _attachLastFileFromLocation;
        private bool _autoStart;
        private int _checkInterval;
        private ObservableCollection<DisplayViewModel> _displays;
        private string _lastFileToAttachLocation;

        public SettingsViewModel(ISettingsHandler settingsHandler, IDisplayHandler displayHandler)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentException("Parameter cannot be null", "settingsHandler");
            }
            if (displayHandler == null)
            {
                throw new ArgumentException("Parameter cannot be null", "displayHandler");
            }
            _settingsHandler = settingsHandler;
            _displayHandler = displayHandler;
            _checkInterval = settingsHandler.GetCheckInterval();
            _autoStart = settingsHandler.GetAutoStart();
            _attachLastFileFromLocation = settingsHandler.GetAttachLastFileFromLocation();
            _lastFileToAttachLocation = settingsHandler.GetLastFileToAttachLocation();
            var displays = _displayHandler.Get().Select(d => new DisplayViewModel(d, SaveDisplaySettings));
            _displays = new ObservableCollection<DisplayViewModel>(displays);
        }

        public bool AttachLastFileFromLocation
        {
            get
            {
                return _attachLastFileFromLocation;
            }
            set
            {
                _attachLastFileFromLocation = value;
                _settingsHandler.SetAttachLastFileFromLocation(value);
                RaisePropertyChanged(() => AttachLastFileFromLocation);
            }
        }

        public bool AutoStart
        {
            get
            {
                return _autoStart;
            }
            set
            {
                _autoStart = value;
                _settingsHandler.SetAutoStart(value);
                RaisePropertyChanged(() => AutoStart);
            }
        }

        public int CheckInterval
        {
            get
            {
                return _checkInterval;
            }
            set
            {
                _checkInterval = value;
                _settingsHandler.SetCheckInterval(value);
                RaisePropertyChanged(() => CheckInterval);
            }
        }

        public ObservableCollection<DisplayViewModel> Displays
        {
            get
            {
                return _displays;
            }
            set
            {
                _displays = value;
                RaisePropertyChanged(() => Displays);
            }
        }

        public string LastFileToAttachLocation
        {
            get
            {
                return _lastFileToAttachLocation;
            }
            set
            {
                _lastFileToAttachLocation = value;
                _settingsHandler.SetLastFileToAttachLocation(value);
                RaisePropertyChanged(() => LastFileToAttachLocation);
            }
        }

        public void SaveDisplaySettings()
        {
            _displayHandler.Save(_displays.Select(dd => dd.Display));
        }
    }
}