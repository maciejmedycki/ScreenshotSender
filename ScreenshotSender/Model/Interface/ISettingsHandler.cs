namespace ScreenshotSender.Model.Interface
{
    public interface ISettingsHandler
    {
        bool GetAttachLastFileFromLocation();

        bool GetAutoStart();

        int GetCheckInterval();

        string GetDisplays();

        string GetEmailFrom();

        string GetEmailFromAlias();

        string GetEmailFromPassword();

        bool GetEmailSmtpEnableSsl();

        string GetEmailSmtpHost();

        int GetEmailSmtpPort();

        string GetEmailSmtpUserName();

        string GetEmailTo();

        string GetLastFileToAttachLocation();

        string GetMachineKey();

        string GetSelectedActions();

        void SetAttachLastFileFromLocation(bool lastFileToAttachLocation);

        void SetAutoStart(bool autoStart);

        void SetCheckInterval(int checkInterval);

        void SetDisplays(string serializedDisplays);

        void SetEmailFrom(string emailFrom);

        void SetEmailFromAlias(string emailFromAlias);

        void SetEmailFromPassword(string emailFromPassword);

        void SetEmailSmtpEnableSsl(bool emailSmtpEnableSsl);

        void SetEmailSmtpHost(string emailSmtpHost);

        void SetEmailSmtpPort(int emailSmtpPort);

        void SetEmailSmtpUserName(string emailSmtpUserName);

        void SetEmailTo(string emailTo);

        void SetLastFileToAttachLocation(string lastFileToAttachLocation);

        void SetMachineKey(string machineKey);

        void SetSelectedActions(string selectedActions);
    }
}