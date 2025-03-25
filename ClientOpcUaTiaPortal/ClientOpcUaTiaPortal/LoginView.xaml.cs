using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientOpcUaTiaPortal
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        ContentControl contentControl;
        CreateConfig config;
        public LoginView(ContentControl contentSend)
        {
            contentControl = contentSend;
            config = new CreateConfig(contentControl);
            InitializeComponent();
            
        }
        private void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            var passwordEnterd = PasswordBox.Password;
            var loginEntered = loginToAccount.Text;
            string login = "login";
            string envPw = "haslo";


                if (passwordEnterd !=envPw )
                {
                    MessageBox.Show("Wrong passowrd");
                }
              else if(login!=loginEntered)
              {
                MessageBox.Show("Wrong login");
              }
                else
                {
                contentControl.Content= config;
                }
            

        }
        private void OnPasswordChanged(object sender, EventArgs e)
        {
            LoginButton.IsEnabled = !string.IsNullOrEmpty(PasswordBox.Password);
        }
    }
}
