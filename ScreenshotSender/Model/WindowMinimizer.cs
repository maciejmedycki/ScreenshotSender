using ScreenshotSender.Model.Interface;
using System.Windows;

namespace ScreenshotSender.Model
{
    public class WindowMinimizer : IWindowMinimizer
    {
        public void MinimizeWindow()
        {
            if (App.Current != null && App.Current.MainWindow != null)
            {
                App.Current.MainWindow.WindowState = WindowState.Minimized;
            }
        }
    }
}