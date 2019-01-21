using GalaSoft.MvvmLight;
using ScreenshotSender.Model.Interface;
using System;

namespace ScreenshotSender.ViewModel
{
    public class ActionViewModel : ViewModelBase
    {
        private readonly IAction _action;
        private readonly Action _saveAction;

        public ActionViewModel(IAction action, Action saveAction)
        {
            _action = action ?? throw new ArgumentException("Parameter cannot be null", nameof(action)); ;
            _saveAction = saveAction ?? throw new ArgumentException("Parameter cannot be null", nameof(saveAction));
        }

        public string Name
        {
            get
            {
                return _action.Name;
            }
        }

        public bool ShouldExecute
        {
            get
            {
                return _action.ShouldExecute;
            }
            set
            {
                _action.ShouldExecute = value;
                _saveAction();
                RaisePropertyChanged(() => ShouldExecute);
            }
        }
    }
}