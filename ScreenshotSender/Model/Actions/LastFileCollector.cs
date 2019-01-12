using ScreenshotSender.Model.Interface;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace ScreenshotSender.Model.Actions
{
    public class LastFileCollector : ILastFileCollector
    {
        private ISettingsHandler _settingsHandler;

        public LastFileCollector(ISettingsHandler settingsHandler)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentException("Parameter cannot be null", "settingsHandler");
            }
            _settingsHandler = settingsHandler;
        }

        public FileInfo GetLastFile()
        {
            if (_settingsHandler.GetAttachLastFileFromLocation())
            {
                var pathToSearch = _settingsHandler.GetLastFileToAttachLocation();
                var directoryInfo = new DirectoryInfo(pathToSearch);
                if (directoryInfo != null)
                {
                    var latestFile = directoryInfo.GetFiles().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
                    return latestFile;
                }
            }
            return null;
        }
    }
}