using ScreenshotSender.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenshotSender.Model
{
    public class MachineKeyHandler : IMachineKeyHandler
    {
        private readonly ISettingsHandler _settingsHandler;

        public MachineKeyHandler(ISettingsHandler settingsHandler)
        {
            if (settingsHandler == null)
            {
                throw new ArgumentException("Parameter cannot be null", "settingsHandler");
            }
            _settingsHandler = settingsHandler;
        }
        public bool CheckIfMachineKeyHasChanhed()
        {
            var result = GetCurrentMachineKey() != _settingsHandler.GetMachineKey();
            return result;
        }

        public void UpdateMachineKey()
        {
            var machineKey = GetCurrentMachineKey();
            _settingsHandler.SetMachineKey(machineKey);
        }

        private string GetCurrentMachineKey()
        {
            var machineKey = Environment.MachineName;
            return machineKey;
        }
    }
}
