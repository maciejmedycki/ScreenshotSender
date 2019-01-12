using GalaSoft.MvvmLight;
using ScreenshotSender.Model;
using System;

namespace ScreenshotSender.ViewModel
{
    public class DisplayViewModel : ViewModelBase
    {
        private readonly Display _display;
        private readonly Action _saveAction;

        public DisplayViewModel(Display display, Action saveAction)
        {
            if (display == null)
            {
                throw new ArgumentException("Parameter cannot be null", "display");
            }
            if (saveAction == null)
            {
                throw new ArgumentException("Parameter cannot be null", "saveAction");
            }
            _saveAction = saveAction;
            _display = display;
        }

        public Display Display
        {
            get
            {
                return _display;
            }
        }

        public string Name
        {
            get
            {
                return _display.FriendlyName;
            }
        }       

        public bool Selected
        {
            get
            {
                return _display.Selected;
            }
            set
            {
                _display.Selected = value;
                _saveAction?.Invoke();
                RaisePropertyChanged(() => Selected);
            }
        }
    }
}