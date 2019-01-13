using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using log4net;
using ScreenshotSender.Model;
using ScreenshotSender.Model.Actions;
using ScreenshotSender.Model.Interface;

namespace ScreenshotSender.ViewModel
{
    public class ViewModelLocator
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ViewModelLocator));

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //TODO: create ModelLocator?
            SimpleIoc.Default.Register<ISettingsHandler, SettingsHandler>();
            SimpleIoc.Default.Register<IMachineKeyHandler, MachineKeyHandler>();
            SimpleIoc.Default.Register<IDisplayHandler, DisplayHandler>();
            SimpleIoc.Default.Register<IActionHandler, ActionHandler>();
            SimpleIoc.Default.Register<IWindowMinimizer, WindowMinimizer>();
            SimpleIoc.Default.Register<IScreenshotTaker, ScreenshotTaker>();
            SimpleIoc.Default.Register<ILastFileCollector, LastFileCollector>();
            SimpleIoc.Default.Register<IOpenFileDialog, OpenFileDialog>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<ActionsViewModel>();
            SimpleIoc.Default.Register<EmailSettingsViewModel>();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}