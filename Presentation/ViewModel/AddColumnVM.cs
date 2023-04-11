using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class AddColumnVM : Notifiable
    {
        private BoardModel board;
        private UserModel user;
        private BackendController controller;
        private string colName;
        public string ColName
        {
            get
            {
                return colName;
            }
            set
            {
                colName = value;
                RaisePropertyChanged("ColName");
            }
        }

        private int colOrdinal;
        public int ColOrdinal
        {
            get
            {
                return colOrdinal;
            }
            set
            {
                colOrdinal = value;
                RaisePropertyChanged("ColOrdinal");
            }
        }

        private string error;
        internal string Error
        {
            get => error;
            set
            {
                error = value;
                RaisePropertyChanged("Error");
            }
        }


        //Constructor
        internal AddColumnVM(UserModel userM, BoardModel boardM)
        {
            this.user = userM;
            this.board = boardM;
            this.controller = boardM.Controller;
        }

        /// <summary>
        /// Adding column to the user's columns list
        /// </summary>
        /// <returns>True if added succesfully, false if failed</returns>
        internal bool AddColumn()
        {
            Error = "";
            try
            {
                ColumnModel c = new ColumnModel(controller, ColName, user, board, colOrdinal, true);
                board.AddColumn(c);
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
