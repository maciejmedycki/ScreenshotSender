using ScreenshotSender.Model.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ScreenshotSender.Model
{
    public class DisplayHandler : IDisplayHandler
    {
        private readonly IMachineKeyHandler _machineKeyHandler;
        private readonly ISettingsHandler _settingsHandler;

        public DisplayHandler(ISettingsHandler settingsHandler, IMachineKeyHandler machineKeyHandler)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentException("Parameter cannot be null", "settingsHandler");
            }
            _settingsHandler = settingsHandler;
            if (machineKeyHandler == null)
            {
                throw new ArgumentException("Parameter cannot be null", "machineKeyHandler");
            }
            _machineKeyHandler = machineKeyHandler;
        }

        public IEnumerable<Display> Get()
        {
            var machineKeyHasChanged = _machineKeyHandler.CheckIfMachineKeyHasChanhed();
            _machineKeyHandler.UpdateMachineKey();
            var serializedDispalys = _settingsHandler.GetDisplays();
            var savedDisplays = Deserialize(serializedDispalys);
            //TODO: for testing purposes move Screen.AllScreens to separate class
            var presentDisplays = Screen.AllScreens.Select(screen => new Display(screen.DeviceName, screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height, true));
            if (!machineKeyHasChanged && savedDisplays!=null && savedDisplays.Count() == presentDisplays.Count())
            {
                return savedDisplays;
            }
            return presentDisplays;
        }

        public void Save(IEnumerable<Display> displaySettings)
        {
            var serialized = Serialize(displaySettings);
            _settingsHandler.SetDisplays(serialized);
        }

        private IEnumerable<Display> Deserialize(string serializedDisplay)
        {
            if (!string.IsNullOrWhiteSpace(serializedDisplay))
            {
                var serializer = new XmlSerializer(typeof(List<Display>));
                using (var stringReader = new StringReader(serializedDisplay))
                {
                    var deserializedObject = (List<Display>)serializer.Deserialize(stringReader);
                    return deserializedObject;
                }
            }
            return null;
        }

        private string Serialize(IEnumerable<Display> displaySettings)
        {
            var serializer = new XmlSerializer(typeof(List<Display>));
            using (var stringWriter = new StringWriter())
            {
                serializer.Serialize(stringWriter, displaySettings.ToList());
                return stringWriter.ToString();
            }
        }
    }
}