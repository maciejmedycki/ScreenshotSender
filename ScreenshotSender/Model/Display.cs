using System;

namespace ScreenshotSender.Model
{
    [Serializable]
    public class Display
    {
        public Display(string name, int x, int y, int width, int height, bool selected)
        {
            Name = name;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Selected = selected;
        }

        private Display()
        {
        }

        public string FriendlyName { get { return Name.Replace(@"\", "").Replace(".", ""); } }
        public int Height { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}