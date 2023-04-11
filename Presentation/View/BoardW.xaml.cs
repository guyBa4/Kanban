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
    /// Interaction logic for Board.xaml
    /// </summary>
    public partial class BoardW : Window
    {
        private BoardVM boardVM;
        private UserMenuW userMenuWindow;
        internal BoardW(UserModel userM, BoardModel boardM, UserMenuW uMenuW)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            boardVM = new BoardVM(userM, boardM);
            this.DataContext = boardVM;
            userMenuWindow = uMenuW;
        }

        private void New_Column_Click(object sender, RoutedEventArgs e)
        {
            AddColumnW addColWindow = new AddColumnW(boardVM.User, boardVM.Board);
            addColWindow.Show();
        }

        private void Remove_Column_Click(object sender, RoutedEventArgs e)
        {
            boardVM.RemoveColumn(boardVM.Board.SelectedColumn);
        }

        private void Enter_Task_Click(object sender, RoutedEventArgs e)
        {
            TaskW taskWindow = new TaskW(boardVM.Board.SelectedTask, this, boardVM.User);
            taskWindow.Show();
            this.Hide();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            UserMenuW UMenu = new UserMenuW(boardVM.User);
            this.Close();
            UMenu.Show();
        }

        private void Sort_Tasks_Click(object sender, RoutedEventArgs e)
        {
            boardVM.SortTasks();
        }

        private void Filter_Tasks_Click(object sender, RoutedEventArgs e)
        {
            FilterTasksW filterTaskkWindow = new FilterTasksW(boardVM, boardVM.Board.SelectedColumn, this);
            filterTaskkWindow.Show();
            this.Hide();
        }

        private void New_Task_Click(object sender, RoutedEventArgs e)
        {
            AddTaskW taskAddWindow = new AddTaskW(boardVM, boardVM.User);
            taskAddWindow.Show();

        }

        private void Advance_Task_Click(object sender, RoutedEventArgs e)
        {
            boardVM.AdvanceTask(boardVM.Board.SelectedTask);
        }

        private void Move_Column_Click(object sender, RoutedEventArgs e)
        {
            MoveColumnW moveColW = new MoveColumnW(boardVM, boardVM.Board.SelectedColumn, this);
            moveColW.Show();
        }
    }
}
