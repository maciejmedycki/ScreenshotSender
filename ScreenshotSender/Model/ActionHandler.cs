using ScreenshotSender.Model.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ScreenshotSender.Model
{
    public class ActionHandler : IActionHandler
    {
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
                    action.ShouldExecute = deserializedSavedSettings.Any(a => a.Name == action.Name && a.ShouldExecute);
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
            var serializedData = Serialize(_actions.Select(a=>(BaseAction)a));
            _settingsHandler.SetSelectedActions(serializedData);
        }

        private IEnumerable<IAction> Deserialize(string serializedActions)
        {
            if (!string.IsNullOrEmpty(serializedActions))
            {
                var serializer = new XmlSerializer(typeof(List<BaseAction>));
                using (var reader = new StringReader(serializedActions))
                {
                    var deserializedObject = (List<BaseAction>)serializer.Deserialize(reader);
                    return deserializedObject;
                }
            }
            return null;
        }

        private string Serialize(IEnumerable<BaseAction> actionsToSerialize)
        {
            var serializer = new XmlSerializer(typeof(List<BaseAction>));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, actionsToSerialize.ToList());
                return writer.ToString();
            }
        }
    }
}