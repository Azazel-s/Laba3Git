using System.Windows;

namespace Demo4
{
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void Button_Account(object sender, RoutedEventArgs e)
        {
            if(Authorization.GetRole(textBoxLogin.Text, textBoxPassword.Text) == null)
            {
                MessageBox.Show("Данные введены не корректно!", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void Button_Click_Out(object sender, RoutedEventArgs e)
        {
            this.Close();
        }          
    }
}
