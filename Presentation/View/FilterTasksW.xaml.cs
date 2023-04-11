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
    /// Interaction logic for FilterTasksW.xaml
    /// </summary>
    public partial class FilterTasksW : Window
    {
        FilterTasksVM FtasksVM;
        BoardVM boardVM;
        ColumnModel colModel;
        private BoardW boardWindow;
        internal FilterTasksW(BoardVM boardVM, ColumnModel colM ,BoardW boardW)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.boardVM = boardVM;
            boardWindow = boardW;
            colModel = colM;
            FtasksVM = new FilterTasksVM(this.boardVM, colModel);
            this.DataContext = FtasksVM;

            colTasks.ItemsSource = FtasksVM.Tasks;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(colTasks.ItemsSource);
            view.Filter = TaskFilter;
        }

        public bool TaskFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as TaskModel).Title.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase)) || ((item as TaskModel).Description.Contains(txtFilter.Text, StringComparison.OrdinalIgnoreCase));
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(colTasks.ItemsSource).Refresh();
        }

        private void Open_Button_Click(object sender, RoutedEventArgs e)
        {
            TaskW taskWindow = new TaskW(FtasksVM.SelectedTask, this,boardVM.User);
            taskWindow.Show();
            this.Hide();
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            boardWindow.Show();
        }
    }
}
