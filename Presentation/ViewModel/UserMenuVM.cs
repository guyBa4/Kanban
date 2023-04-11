using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Presentation.ViewModel
{
    class UserMenuVM : Notifiable
    {
        public UserModel User { get; }
        private BackendController controller;
        public MenuModel Menu { get; private set; }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private BoardModel selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return selectedBoard;
            }
            set
            {
                if (value == null)
                {
                    selectedBoard = value;
                    BoardEnableForward = value != null;
                }
                else
                {
                    selectedBoard = value;
                    SelectedTask = null;
                    BoardEnableForward = value != null;
                }
                RaisePropertyChanged("SelectedBoard");
            }
        }

        private TaskModel selectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return selectedTask;
            }
            set
            {
                if (value == null)
                {
                    selectedTask = value;
                    TaskEnableForward = value != null;
                }
                else
                {
                    selectedTask = value;
                    SelectedBoard = null;
                    TaskEnableForward = value != null;
                }
                RaisePropertyChanged("SelectedTask");
            }
        }

        private bool taskEnableForward = false;
        public bool TaskEnableForward
        {
            get => taskEnableForward;
            private set
            {
                taskEnableForward = value;
                RaisePropertyChanged("TaskEnableForward");
            }
        }

        private bool boardEnableForward = false;
        public bool BoardEnableForward
        {
            get => boardEnableForward;
            private set
            {
                boardEnableForward = value;
                RaisePropertyChanged("BoardEnableForward");
            }
        }

        //Constructor
        public UserMenuVM(UserModel user)
        {
            user.UpdateList();
            this.User = user;
            this.controller = user.Controller;
            Menu = new MenuModel(controller, user);
        }

        /// <summary>
        /// Logging out the user from the system
        /// </summary>
        internal void Logout()
        {
            controller.Logout(User.Email);
        }

        /// <summary>
        /// Removing selected board from user's list
        /// </summary>
        internal void RemoveBoard()
        {
            try
            {
                Menu.RemoveBoard(SelectedBoard);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot remove board. " + e.Message);
            }
        }
    }
}
