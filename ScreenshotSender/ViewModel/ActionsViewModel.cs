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
        private readonly ISettingsHandler _settingsHandler;
        private ObservableCollection<ActionViewModel> _actions;
        private bool _isExpanded;

        public ActionsViewModel(IActionHandler actionLoader, ISettingsHandler settingsHandler)
        {
            if (actionLoader == null)
            {
                throw new ArgumentException("Parameter cannot be null", nameof(actionLoader));
            }
            if (settingsHandler == null)
            {
                throw new ArgumentException("Parameter cannot be null", nameof(settingsHandler));
            }
            _actionLoader = actionLoader;
            _settingsHandler = settingsHandler;
            var loadedActions = actionLoader.Load();
            var actionsViewModels = loadedActions.Select(a => new ActionViewModel(a, SaveChanges));
            _actions = new ObservableCollection<ActionViewModel>(actionsViewModels);
            _isExpanded = settingsHandler.GetActionsIsExpanded();
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

        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                _settingsHandler.SetActionsIsExpanded(value);
                RaisePropertyChanged(() => IsExpanded);
            }
        }

        private void SaveChanges()
        {
            _actionLoader.Save();
        }
    }
}