using log4net;
using ScreenshotSender.Model.Interface;
using ScreenshotSender.Properties;

namespace ScreenshotSender.Model
{
    public class SettingsHandler : ISettingsHandler
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(SettingsHandler));

        public bool GetAttachLastFileFromLocation()
        {
            return Get<bool>(SettingsKeys.AttachLastFileFromLocationKey);
        }

        public bool GetAutoStart()
        {
            return Get<bool>(SettingsKeys.AutoStart);
        }

        public int GetCheckInterval()
        {
            return Get<int>(SettingsKeys.CheckInterval);
        }

        public string GetDisplays()
        {
            return Get<string>(SettingsKeys.DisplaysSettings);
        }

        public string GetEmailFrom()
        {
            return Get<string>(SettingsKeys.EmailFrom);
        }

        public string GetEmailFromAlias()
        {
            return Get<string>(SettingsKeys.EmailFromAlias);
        }

        public string GetEmailFromPassword()
        {
            return Get<string>(SettingsKeys.EmailFromPassword);
        }

        public bool GetEmailSmtpEnableSsl()
        {
            return Get<bool>(SettingsKeys.EmailSmtpEnableSsl);
        }

        public string GetEmailSmtpHost()
        {
            return Get<string>(SettingsKeys.EmailSmtpHost);
        }

        public int GetEmailSmtpPort()
        {
            return Get<int>(SettingsKeys.EmailSmtpPort);
        }

        public string GetEmailSmtpUserName()
        {
            return Get<string>(SettingsKeys.EmailSmtpUserName);
        }

        public string GetEmailTo()
        {
            return Get<string>(SettingsKeys.EmailTo);
        }

        public string GetLastFileToAttachLocation()
        {
            return Get<string>(SettingsKeys.LastFileToAttachLocationKey);
        }

        public string GetMachineKey()
        {
            return Get<string>(SettingsKeys.MachineKey);
        }

        public string GetSelectedActions()
        {
            return Get<string>(SettingsKeys.SelectedActions);
        }

        public void SetAttachLastFileFromLocation(bool attachLastFileFromLocation)
        {
            Set(SettingsKeys.AttachLastFileFromLocationKey, attachLastFileFromLocation);
        }

        public void SetAutoStart(bool autoStart)
        {
            Set(SettingsKeys.AutoStart, autoStart);
        }

        public void SetCheckInterval(int checkInterval)
        {
            Set(SettingsKeys.CheckInterval, checkInterval);
        }

        public void SetDisplays(string serializedDisplays)
        {
            Set(SettingsKeys.DisplaysSettings, serializedDisplays);
        }

        public void SetEmailFrom(string emailFrom)
        {
            Set(SettingsKeys.EmailFrom, emailFrom);
        }

        public void SetEmailFromAlias(string emailFromAlias)
        {
            Set(SettingsKeys.EmailFromAlias, emailFromAlias);
        }

        public void SetEmailFromPassword(string emailFromPassword)
        {
            Set(SettingsKeys.EmailFromPassword, emailFromPassword);
        }

        public void SetEmailSmtpEnableSsl(bool emailSmtpEnableSsl)
        {
            Set(SettingsKeys.EmailSmtpEnableSsl, emailSmtpEnableSsl);
        }

        public void SetEmailSmtpHost(string emailSmtpHost)
        {
            Set(SettingsKeys.EmailSmtpHost, emailSmtpHost);
        }

        public void SetEmailSmtpPort(int emailSmtpPort)
        {
            Set(SettingsKeys.EmailSmtpPort, emailSmtpPort);
        }

        public void SetEmailSmtpUserName(string emailSmtpUserName)
        {
            Set(SettingsKeys.EmailSmtpUserName, emailSmtpUserName);
        }

        public void SetEmailTo(string emailTo)
        {
            Set(SettingsKeys.EmailTo, emailTo);
        }

        public void SetLastFileToAttachLocation(string lastFileToAttachLocation)
        {
            Set(SettingsKeys.LastFileToAttachLocationKey, lastFileToAttachLocation);
        }

        public void SetMachineKey(string machineKey)
        {
            Set(SettingsKeys.MachineKey, machineKey);
        }

        public void SetSelectedActions(string selectedActions)
        {
            Set(SettingsKeys.SelectedActions, selectedActions);
        }

        private T Get<T>(string settingKey)
        {
            return (T)Settings.Default[settingKey];
        }

        private void Set<T>(string settingKey, T value)
        {
            Settings.Default[settingKey] = value;
            Settings.Default.Save();
        }
    }
}