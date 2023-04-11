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
    /// Interaction logic for CreateBoardW.xaml
    /// </summary>
    public partial class CreateBoardW : Window
    {
        private AddBoardVM addBoardVM;
        private UserMenuVM userMVM;
        internal CreateBoardW(UserModel userM, UserMenuVM umVM)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            addBoardVM = new AddBoardVM(userM, umVM);
            this.DataContext = addBoardVM;
            this.userMVM = umVM;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            BoardModel boardM = addBoardVM.AddBoard(true);
            if (boardM != null)
            {
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
