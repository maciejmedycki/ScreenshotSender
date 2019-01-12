using System.IO;

namespace ScreenshotSender.Model.Interface
{
    public interface ILastFileCollector
    {
        FileInfo GetLastFile();
    }
}