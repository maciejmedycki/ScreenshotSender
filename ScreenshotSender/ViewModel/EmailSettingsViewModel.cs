using GalaSoft.MvvmLight;
using log4net;
using ScreenshotSender.Model.Interface;
using System;

namespace ScreenshotSender.ViewModel
{
    public class EmailSettingsViewModel : ViewModelBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(EmailSettingsViewModel));
        private readonly ISettingsHandler _settingsHandler;

        private string _emailFrom;
        private string _emailFromAlias;
        private string _emailFromPassword;
        private bool _emailSmtpEnableSsl;
        private string _emailSmtpHost;
        private int _emailSmtpPort;
        private string _emailSmtpUserName;
        private string _emailTo;
        private bool _isExpanded;

        public EmailSettingsViewModel(ISettingsHandler settingsHandler)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentException("Parameter cannot be null", nameof(settingsHandler));
            }
            _settingsHandler = settingsHandler;
            _emailFrom = settingsHandler.GetEmailFrom();
            _emailFromAlias = settingsHandler.GetEmailFromAlias();
            _emailTo = settingsHandler.GetEmailTo();
            _emailFromPassword = settingsHandler.GetEmailFromPassword();
            _emailSmtpEnableSsl = settingsHandler.GetEmailSmtpEnableSsl();
            _emailSmtpHost = settingsHandler.GetEmailSmtpHost();
            _emailSmtpPort = settingsHandler.GetEmailSmtpPort();
            _emailSmtpUserName = settingsHandler.GetEmailSmtpUserName();
            _isExpanded = settingsHandler.GetEmailIsExpanded();
        }

        public string EmailFrom
        {
            get
            {
                return _emailFrom;
            }
            set
            {
                _emailFrom = value;
                _settingsHandler.SetEmailFrom(value);
                RaisePropertyChanged(() => EmailFrom);
            }
        }

        public string EmailFromAlias
        {
            get
            {
                return _emailFromAlias;
            }
            set
            {
                _emailFromAlias = value;
                _settingsHandler.SetEmailFromAlias(value);
                RaisePropertyChanged(() => EmailFromAlias);
            }
        }

        public string EmailFromPassword
        {
            get
            {
                return _emailFromPassword;
            }
            set
            {
                _emailFromPassword = value;
                _settingsHandler.SetEmailFromPassword(value);
                RaisePropertyChanged(() => EmailFromPassword);
            }
        }

        public bool EmailSmtpEnableSsl
        {
            get
            {
                return _emailSmtpEnableSsl;
            }
            set
            {
                _emailSmtpEnableSsl = value;
                _settingsHandler.SetEmailSmtpEnableSsl(value);
                RaisePropertyChanged(() => EmailSmtpEnableSsl);
            }
        }

        public string EmailSmtpHost
        {
            get
            {
                return _emailSmtpHost;
            }
            set
            {
                _emailSmtpHost = value;
                _settingsHandler.SetEmailSmtpHost(value);
                RaisePropertyChanged(() => EmailSmtpHost);
            }
        }

        public int EmailSmtpPort
        {
            get
            {
                return _emailSmtpPort;
            }
            set
            {
                _emailSmtpPort = value;
                _settingsHandler.SetEmailSmtpPort(value);
                RaisePropertyChanged(() => EmailSmtpPort);
            }
        }

        public string EmailSmtpUserName
        {
            get
            {
                return _emailSmtpUserName;
            }
            set
            {
                _emailSmtpUserName = value;
                _settingsHandler.SetEmailSmtpUserName(value);
                RaisePropertyChanged(() => EmailSmtpUserName);
            }
        }

        public string EmailTo
        {
            get
            {
                return _emailTo;
            }
            set
            {
                _emailTo = value;
                _settingsHandler.SetEmailTo(value);
                RaisePropertyChanged(() => EmailTo);
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
                _settingsHandler.SetEmailIsExpanded(value);
                RaisePropertyChanged(() => IsExpanded);
            }
        }
    }
}