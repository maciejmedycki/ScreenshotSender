namespace ScreenshotSender.Model.Interface
{
    public interface IMachineKeyHandler
    {
        bool CheckIfMachineKeyHasChanhed();
        void UpdateMachineKey();
    }
}