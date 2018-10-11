using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using log4net;

namespace ScreenshotSender.ViewModel
{
    public class ViewModelLocator
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ViewModelLocator));

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
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