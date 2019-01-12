using GalaSoft.MvvmLight;
using ScreenshotSender.Model.Interface;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ScreenshotSender.ViewModel
{
    public class ActionsViewModel : ViewModelBase
    {
        private readonly IActionHandler _actionLoader;
        private ObservableCollection<ActionViewModel> _actions;

        public ActionsViewModel(IActionHandler actionLoader)
        {
            if (actionLoader == null)
            {
                throw new ArgumentException("Parameter cannot be null", "actionLoader");
            }
            _actionLoader = actionLoader;
            var loadedActions = actionLoader.Load();
            var actionsViewModels = loadedActions.Select(a => new ActionViewModel(a, SaveChanges));
            _actions = new ObservableCollection<ActionViewModel>(actionsViewModels);
        }

        public ObservableCollection<ActionViewModel> Actions
        {
            get
            {
                return _actions;
            }
            set
            {
                _actions = value;
                RaisePropertyChanged(() => Actions);
            }
        }

        private void SaveChanges()
        {
            _actionLoader.Save();
        }
    }
}