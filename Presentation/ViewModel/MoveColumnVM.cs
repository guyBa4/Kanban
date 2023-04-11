using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class MoveColumnVM : Notifiable
    {
        public BoardVM BoardVM { get; private set; }
        public ColumnModel ColumnM { get; private set; }
        public BackendController Controller { get; private set; }
        private int newOrdinal;
        public int NewOrdinal
        {
            get => newOrdinal;
            set
            {
                newOrdinal = value;
                RaisePropertyChanged("NewOrdinal");
            }
        }
        private string stringNewOrdinal;
        public string StringNewOrdinal
        {
            get => stringNewOrdinal;
            set
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(value, "[0-9]"))
                {
                    NewOrdinal = int.Parse(value);
                    stringNewOrdinal = value;
                    RaisePropertyChanged("StringNewOrdinal");
                }
            }
        }

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
        public MoveColumnVM(BoardVM brdVM, ColumnModel clmM)
        {
            this.BoardVM = brdVM;
            this.ColumnM = clmM;
            this.Controller = BoardVM.Controller;
        }

        /// <summary>
        /// shifting the column to a new ordinal
        /// </summary>
        /// <param name="ColumnM">col to move</param>
        /// <param name="newOrdinal">new ordinal for the column</param>
        internal bool MoveColumn()
        {
            Error = "";
            try
            {
                BoardVM.Board.MoveColumn(ColumnM, NewOrdinal);
                return true;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return false;
            }
        }
    }
}
