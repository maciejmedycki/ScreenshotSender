using ScreenshotSender.Model.Interface;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;

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

        public LinkedResource GetLastFile()
        {
            if (_settingsHandler.GetAttachLastFileFromLocation())
            {
                var pathToSearch = _settingsHandler.GetLastFileToAttachLocation();
                var directoryInfo = new DirectoryInfo(pathToSearch);
                if (directoryInfo != null)
                {
                    var latestFile = directoryInfo.GetFiles().OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
                    if (latestFile != null)
                    {
                        using (var memoryStream = new MemoryStream(File.ReadAllBytes(latestFile.FullName)))
                        {
                            memoryStream.Position = 0;
                            //TODO: why it is not working?                        
                            // return new LinkedResource(memoryStream, MimeTypes.GetMimeType(latestFile.Name));
                            var mimeType = MimeTypes.GetMimeType(latestFile.Name);
                            var linkedResource = new LinkedResource(memoryStream, mimeType);
                            return linkedResource;
                        }
                    }
                }
            }
            return null;
        }
    }
}