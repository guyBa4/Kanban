using log4net;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class AddTaskVM : Notifiable
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        BackendController controller;
        private BoardVM boardVM;
        public UserModel UserM { get; private set; }
        public BoardModel BoardM { get; private set; }
        public TaskModel TaskM { get; private set; }

        private string title = "";
        public string Title
        {
            get => title;
            set
            {
                this.title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string description = "";
        public string Description
        {
            get => description;
            set
            {
                this.description = value;
                RaisePropertyChanged("Description");
            }
        }

        private string dueDate = "";
        public string DueDate
        {
            get => dueDate;
            set
            {
                this.dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }

        private string error = "";
        internal string Error
        {
            get => error;
            set
            {
                error = value;
            }
        }

        internal AddTaskVM(BoardVM boardVM, UserModel userM)
        {
            this.boardVM = boardVM;
            UserM = userM;
            this.controller = boardVM.Controller;
            BoardM = boardVM.Board;
        }

        /// <summary>
        /// Adding task to the user's tasks list
        /// </summary>
        /// <returns>The task we added, null if failed</returns>
        internal TaskModel AddTask()
        {
            Error = "";
            try
            {
                TaskM = new TaskModel(controller, controller.GetTaskConter(), Title, Description, DateTime.Now, dueDate, UserM.Email, BoardM.EmailCreator, UserM.Email, BoardM.Name, 0);
                BoardM.AddTask(TaskM);
                return TaskM;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return null;
            }
        }
    }
}
