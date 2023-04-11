using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Presentation.ViewModel
{
    class BoardVM : Notifiable
    {
        public UserModel User { get; private set; }
        public BackendController Controller { get; private set; }
        public BoardModel Board { get; private set; }

        private string error;
        public string Error
        {
            get => error;
            set
            {
                error = value;
                RaisePropertyChanged("Error");
            }
        }

        //Constructor
        internal BoardVM(UserModel userM, BoardModel boardM)
        {
            this.User = userM;
            this.Board = boardM;
            Controller = boardM.Controller;
        }

        /// <summary>
        /// shifting the column to a new ordinal
        /// </summary>
        /// <param name="col">col to move</param>
        /// <param name="newOrdinal">new ordinal for the column</param>
        internal void MoveColumn(ColumnModel col, int newOrdinal)
        {
            try
            {
                Board.MoveColumn(col, newOrdinal);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot move column. " + e.Message);
            }
        }

        /// <summary>
        /// Send the column to the board we wish to remove from
        /// </summary>
        /// <param name="col">column we want to remove</param>
        internal void RemoveColumn(ColumnModel col)
        {
            try
            {
                Board.RemoveColumn(col);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot Remove column. " + e.Message);
            }
        }

        /// <summary>
        /// Sorting the tasks of a board
        /// </summary>
        internal void SortTasks()
        {
            Board.SelectedColumn.SortTasks();
        }

        /// <summary>
        /// Advancing a task one column forward in the board
        /// </summary>
        /// <param name="task">task we want to advance</param>
        internal void AdvanceTask(TaskModel task)
        {
            Error = "";
            try
            {
                Board.AdvanceTask(task);
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
        }
    }
}
