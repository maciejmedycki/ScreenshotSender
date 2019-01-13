namespace ScreenshotSender.Model
{
    public partial class ActionHandler
    {
        public struct ActionSetting
        {
            public ActionSetting(string serializedSetting)
            {
                Name = string.Empty;
                Type = string.Empty;
                ShouldExecute = false;
                var truncated = serializedSetting.Replace("{", string.Empty).Replace("}", string.Empty);
                var splitted = truncated.Split(',');
                foreach (var fragment in splitted)
                {
                    var keyValues = fragment.Split(':');
                    if (keyValues.Length == 2)
                    {
                        switch (keyValues[0])
                        {
                            case "Name":
                                Name = keyValues[1];
                                break;
                            case "Type":
                                Type = keyValues[1];
                                break;
                            case "ShouldExecute":
                                ShouldExecute = bool.Parse(keyValues[1]);
                                break;
                        }
                    }
                }
            }

            public string Name { get; private set; }
            public bool ShouldExecute { get; private set; }
            public string Type { get; private set; }
        }
    }
}