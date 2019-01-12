using System.Collections.Generic;

namespace ScreenshotSender.Model.Interface
{
    public interface IDisplayHandler
    {
        IEnumerable<Display> Get();

        void Save(IEnumerable<Display> displaySettings);
    }
}