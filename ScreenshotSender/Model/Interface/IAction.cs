namespace ScreenshotSender.Model.Interface
{
    public interface IAction
    {
        string Name { get; }
        bool ShouldExecute { get; set; }
        string Serialize();
    }
}