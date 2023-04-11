using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using Presentation.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginW : Window
    {
        private LoginVM loginvm;
        public LoginW()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            loginvm = new LoginVM();
            this.DataContext = loginvm;
            //loginvm.bController.DeleteData();
            loginvm.LoadData();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            UserModel userM = loginvm.Login();
            if (userM != null)
            {
                UserMenuW UMenu = new UserMenuW(userM);
                UMenu.Show();
                this.Close();
            }
        }

        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {
            loginvm.Register();
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            loginvm.Password = passwordBox.Password;
        }
    }
}
