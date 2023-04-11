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
    /// Interaction logic for MoveColumnW.xaml
    /// </summary>
    public partial class MoveColumnW : Window
    {
        MoveColumnVM moveColVM;
        BoardW boardW;
        internal MoveColumnW(BoardVM brdVM, ColumnModel clmM, BoardW boardW)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            moveColVM = new MoveColumnVM(brdVM, clmM);
            this.DataContext = moveColVM;
            this.boardW = boardW;
        }

        private void Ordinal_Box_Text_Change(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(OrdinalBox.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                OrdinalBox.Text = OrdinalBox.Text.Remove(OrdinalBox.Text.Length - 1);
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            bool moved = moveColVM.MoveColumn();
            if (moved)
            {
                this.Close();
                boardW.Show();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            boardW.Show();
        }
    }
}
