using System.Net.Mail;

namespace ScreenshotSender.Model.Interface
{
    public interface ILastFileCollector
    {
        LinkedResource GetLastFile();
    }
}