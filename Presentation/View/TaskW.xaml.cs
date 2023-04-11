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
    /// Interaction logic for TaskW.xaml
    /// </summary>
    public partial class TaskW : Window
    {
        private TaskVM taskVM;
        private Window backWindow;
        internal TaskW(TaskModel taskM, Window w, UserModel userM)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            taskVM = new TaskVM(taskM, userM);
            this.DataContext = taskVM;
            backWindow = w;

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (backWindow.GetType() == typeof(UserMenuW))
            {
                UserMenuW UMenu = new UserMenuW(taskVM.UserM);
                this.Close();
                UMenu.Show();
            }
            else
            {
                this.Close();
                backWindow.Show();
            }
        }
    }
}
