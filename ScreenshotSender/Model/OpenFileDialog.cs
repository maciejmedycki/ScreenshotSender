using ScreenshotSender.Model.Interface;

namespace ScreenshotSender.Model
{
    public class OpenFileDialog : IOpenFileDialog
    {
        public string ChooseFolderPath()
        {
            var result = string.Empty;
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                var dialogResult = dialog.ShowDialog();
                if (dialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    result = dialog.SelectedPath;
                }
            }
            return result;
        }
    }
}