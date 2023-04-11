using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class FilterTasksVM : Notifiable
    {
        public BoardVM boardVM;
        public List<TaskModel> Tasks;
        private TaskModel selectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return selectedTask;
            }
            set
            {
                selectedTask = value;
                TaskEnableForward = value != null;
                RaisePropertyChanged("SelectedTask");
            }
        }

        private bool taskEnableForward = false;
        public bool TaskEnableForward
        {
            get => taskEnableForward;
            internal set
            {
                taskEnableForward = value;
                RaisePropertyChanged("TaskEnableForward");
            }
        }


        //Constructor
        internal FilterTasksVM(BoardVM boardVM, ColumnModel colM)
        {
            this.boardVM = boardVM;
            Tasks = new List<TaskModel>();
            foreach (TaskModel tsk in colM.Tasks)
            {
                Tasks.Add(tsk);
            }
        }
    }
}
