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
    /// Interaction logic for AddTaskW.xaml
    /// </summary>
    public partial class AddTaskW : Window
    {

        private AddTaskVM addTaskVM;
        internal TaskModel taskM;
        internal TaskModel TaskM
        {
            get
            {
                return taskM;
            }
            set
            {
                taskM = value;
            }
        }

        internal AddTaskW(BoardVM boardVM, UserModel userM)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            addTaskVM = new AddTaskVM(boardVM, userM);
            this.DataContext = addTaskVM;
        }


        private void Create_Click(object sender, RoutedEventArgs e)
        {
            TaskM = addTaskVM.AddTask();
            if (TaskM != null)
            {
                this.Close();
            }
            else
            {
                MessageBox.Show("Task couldn't be added");
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
