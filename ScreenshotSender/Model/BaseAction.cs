using ScreenshotSender.Model.Interface;
using System.IO;
using System.Xml.Serialization;

namespace ScreenshotSender.Model
{
    public abstract class BaseAction : IAction
    {
        public abstract string Name { get; }

        public abstract bool ShouldExecute { get; set; }

        public string Serialize()
        {
            var serializer = new XmlSerializer(typeof(BaseAction));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }
    }
}