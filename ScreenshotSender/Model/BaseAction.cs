using ScreenshotSender.Model.Interface;

namespace ScreenshotSender.Model
{
    public abstract class BaseAction : IAction
    {
        public abstract string Name { get; }

        public abstract bool ShouldExecute { get; set; }

        public string Serialize()
        {
            return $"{{Name:{Name},ShouldExecute:{ShouldExecute},Type:{GetType()}}}";
        }
    }
}