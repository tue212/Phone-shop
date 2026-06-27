using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PhoneShop
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginWindow : Window
    {
        private Window? _window;
        public static Window? ActiveWindow { get; set; }
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            if (username == "admin" && password == "1234")
            {
                if (rememberMeCheckBox.IsChecked == true)
                {
                    var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    var (encryptedPassword, entropy) = Password.Encrypt(password);
                    localSettings.Values["Username"] = username;
                    localSettings.Values["Password"] = encryptedPassword;
                    localSettings.Values["Entropy"] = entropy;
                }
                _window = new MainWindow();
                ActiveWindow = _window;
                _window.Activate();
                this.Close();
            } else
            {
                var dialog = new ContentDialog()
                {
                    XamlRoot = this.Content.XamlRoot,
                    Title = "Error",
                    Content = "Invalid username or password",
                    CloseButtonText = "OK"
                };
                var _ =  await dialog.ShowAsync();
            }
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("Username")
                && localSettings.Values.ContainsKey("Password")
                && localSettings.Values.ContainsKey("Entropy"))
            {
                string username = (string) localSettings.Values["Username"];
                string encryptedPassword = (string)localSettings.Values["Password"];
                string entropy = (string)localSettings.Values["Entropy"];

                string rawPassword = Password.Decrypt(encryptedPassword, entropy);
                usernameTextBox.Text = username;
                passwordBox.Password = rawPassword;
            }    
        }
    }
}
