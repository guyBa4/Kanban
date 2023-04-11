using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using Presentation.ViewModel;
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
using System.Windows.Shapes;

namespace Presentation.View
{
    /// <summary>
    /// Interaction logic for UserMenu.xaml
    /// </summary>
    public partial class UserMenuW : Window
    {
        internal UserMenuVM userMenuVM;
        internal UserMenuW(UserModel user)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            userMenuVM = new UserMenuVM(user);
            DataContext = userMenuVM;
        }


        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            userMenuVM.Logout();
            LoginW loginWindow = new LoginW();
            loginWindow.Show();
            this.Close();
        }

        private void Join_Board_Button_Click(object sender, RoutedEventArgs e)
        {
            JoinBoardW joinBW = new JoinBoardW(userMenuVM.User, userMenuVM);
            joinBW.Show();
        }

        private void Enter_Board_Button(object sender, RoutedEventArgs e)
        {
            BoardW boardWindow = new BoardW(userMenuVM.User, userMenuVM.SelectedBoard, this);
            boardWindow.Show();
            this.Hide();
        }

        private void Remove_Board_Button(object sender, RoutedEventArgs e)
        {
            userMenuVM.RemoveBoard();
        }

        private void Create_Board_Button_Click(object sender, RoutedEventArgs e)
        {
            CreateBoardW joinBW = new CreateBoardW(userMenuVM.User, userMenuVM);
            joinBW.Show();
        }

        private void Show_Task_Click(object sender, RoutedEventArgs e)
        {
            TaskW taskWindow = new TaskW(userMenuVM.SelectedTask, this, userMenuVM.User);
            taskWindow.Show();
            this.Close();
        }
    }
}
