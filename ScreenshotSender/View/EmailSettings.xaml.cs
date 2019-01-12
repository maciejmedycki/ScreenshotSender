using ScreenshotSender.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace ScreenshotSender.View
{
    /// <summary>
    ///    Interaction logic for EmailSettings.xaml
    /// </summary>
    public partial class EmailSettings : UserControl
    {
        //TODO: meh~! not so MVVM with that PasswordBox

        public EmailSettings()
        {
            DataContextChanged += EmailSettings_DataContextChanged;
            InitializeComponent();
        }

        private void EmailSettings_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = GetViewModel();
            if (viewModel != null)
            {
                PasswordBox.Password = viewModel.EmailFromPassword;
            }
        }

        private EmailSettingsViewModel GetViewModel()
        {
            return this.DataContext as EmailSettingsViewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = GetViewModel();
            if (viewModel != null)
            {
                viewModel.EmailFromPassword = PasswordBox.Password;
            }
        }
    }
}