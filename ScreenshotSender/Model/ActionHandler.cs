using ScreenshotSender.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScreenshotSender.Model
{
    public partial class ActionHandler : IActionHandler
    {
        private const char _actionSeparator = ';';
        private static readonly IList<IAction> _actions = new List<IAction>();
        private readonly ISettingsHandler _settingsHandler;

        public ActionHandler(ISettingsHandler settingsHandler)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentException("Parameter cannot be null", "settingsHandler");
            }
            _settingsHandler = settingsHandler;
        }

        public IEnumerable<IAction> Load()
        {
            var assembly = GetType().Assembly;
            var types = from type in assembly.GetTypes()
                        where typeof(IAction).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract
                        select type;
            var savedSettings = _settingsHandler.GetSelectedActions();
            var deserializedSavedSettings = Deserialize(savedSettings);
            foreach (var type in types)
            {
                var action = Activator.CreateInstance(type) as IAction;
                if (deserializedSavedSettings != null)
                {
                    action.ShouldExecute = deserializedSavedSettings.Any(a => a.Name == action.Name && a.Type == type.ToString() && a.ShouldExecute);
                }
                if (!_actions.Contains(action))
                {
                    _actions.Add(action);
                }
                yield return action;
            }
        }

        public void Save()
        {
            var serializedData = Serialize(_actions.Select(a => (BaseAction)a));
            _settingsHandler.SetSelectedActions(serializedData);
        }

        private IEnumerable<ActionSetting> Deserialize(string serializedActions)
        {
            if (!string.IsNullOrEmpty(serializedActions))
            {
                var splitted = serializedActions.Split(_actionSeparator);
                foreach (var actionString in splitted)
                {
                    yield return new ActionSetting(actionString);
                }
            }
        }

        private string Serialize(IEnumerable<BaseAction> actionsToSerialize)
        {
            var stringBuilder = new StringBuilder();
            foreach (var action in actionsToSerialize)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(_actionSeparator);
                }
                var serialized = action.Serialize();
                stringBuilder.Append(serialized);
            }
            return stringBuilder.ToString();
        }
    }
}