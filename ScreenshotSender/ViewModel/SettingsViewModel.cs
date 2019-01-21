using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using log4net;
using ScreenshotSender.Model.Interface;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

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
        private bool _isExpanded;
        private string _lastFileToAttachLocation;
        private IOpenFileDialog _openFileDialog;
        private ICommand _showFolderDialogCommand;

        public SettingsViewModel(ISettingsHandler settingsHandler, IDisplayHandler displayHandler, IOpenFileDialog openFileDialog)
        {
            _settingsHandler = settingsHandler ?? throw new ArgumentException("Parameter cannot be null", nameof(settingsHandler));
            _displayHandler = displayHandler ?? throw new ArgumentException("Parameter cannot be null", nameof(displayHandler));
            _openFileDialog = openFileDialog ?? throw new ArgumentException("Parameter cannot be null", nameof(openFileDialog));
            _checkInterval = settingsHandler.GetCheckInterval();
            _autoStart = settingsHandler.GetAutoStart();
            _attachLastFileFromLocation = settingsHandler.GetAttachLastFileFromLocation();
            _lastFileToAttachLocation = settingsHandler.GetLastFileToAttachLocation();
            var displays = _displayHandler.Get().Select(d => new DisplayViewModel(d, SaveDisplaySettings));
            _displays = new ObservableCollection<DisplayViewModel>(displays);
            _isExpanded = settingsHandler.GetSettingsIsExpanded();
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

        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                _settingsHandler.SetSettingsIsExpanded(value);
                RaisePropertyChanged(() => IsExpanded);
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

        public ICommand ShowFolderDialogCommand
        {
            get
            {
                if (_showFolderDialogCommand == null)
                {
                    _showFolderDialogCommand = new RelayCommand(DoShowFolderDialogCommand);
                }
                return _showFolderDialogCommand;
            }
        }

        public void DoShowFolderDialogCommand()
        {
            var directory = _openFileDialog.ChooseFolderPath();
            if (!string.IsNullOrWhiteSpace(directory))
            {
                LastFileToAttachLocation = directory;
            }
        }

        public void SaveDisplaySettings()
        {
            _displayHandler.Save(_displays.Select(dd => dd.Display));
        }
    }
}