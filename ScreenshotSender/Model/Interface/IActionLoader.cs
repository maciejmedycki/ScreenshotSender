using System.Collections.Generic;

namespace ScreenshotSender.Model.Interface
{
    public interface IActionHandler
    {
        IEnumerable<IAction> Load();
        void Save();
    }
}