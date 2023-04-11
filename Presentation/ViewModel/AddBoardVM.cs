using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class AddBoardVM : Notifiable
    {
        public UserModel UserM { get; private set; }
        public BoardModel BoardM { get; private set; }
        private BackendController controller;
        private UserMenuVM userMenuVM;

        private string name;
        public string Name
        {
            get => name;
            set
            {
                this.name = value;
                RaisePropertyChanged("Name");
            }
        }
        private string emailCreator;
        public string EmailCreator
        {
            get => emailCreator;
            set
            {
                this.emailCreator = value;
                RaisePropertyChanged("EmailCreator");
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
        internal AddBoardVM(UserModel userM, UserMenuVM umVM)
        {
            UserM = userM;
            controller = userM.Controller;
            userMenuVM = umVM;
            EmailCreator = userM.Email;
        }

        /// <summary>
        /// Adding board to the user's boards list
        /// </summary>
        /// <param name="isNewBoard">determine if the board is new to the system or should be loaded from database</param>
        /// <returns>the board we added, null if failed</returns>
        internal BoardModel AddBoard(bool isNewBoard)
        {
            Error = "";
            try
            {
                BoardM = new BoardModel(controller, UserM, Name, EmailCreator, isNewBoard);
                userMenuVM.Menu.AddBoard(BoardM);
                return BoardM;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return null;
            }
        }
    }
}
