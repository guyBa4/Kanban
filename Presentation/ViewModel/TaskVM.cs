using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class TaskVM : Notifiable
    {
        public BackendController bController { get; private set; }
        public TaskModel TaskM { get; private set; }
        public UserModel UserM { get; private set; }
        public BoardModel BoardM { get; private set; }
        public BoardVM BoardViewM { get; private set; }
        private int id;
        public int Id
        {
            get => id;
            set
            {
                this.id = value;
                RaisePropertyChanged("Id");
            }
        }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                try
                {
                    TaskM.Title = value;
                    this.title = value;
                    RaisePropertyChanged("Title");
                }
                catch (Exception e)
                {
                    Error = e.Message;
                }
            }
        }

        private string description;
        public string Description
        {
            get => description;
            set
            {
                try
                {
                    TaskM.Description = value;
                    this.description = value;
                    RaisePropertyChanged("Description");
                }
                catch (Exception e)
                {
                    Error = e.Message;
                }
            }
        }

        private string dueDate;
        public string DueDate
        {
            get => dueDate;
            set
            {
                try
                {
                    TaskM.DueDate = value;
                    this.dueDate = value;
                    RaisePropertyChanged("DueDate");
                }
                catch (Exception e)
                {
                    Error = e.Message;
                }
            }
        }



        private DateTime creationTime;
        public DateTime CreationTime
        {
            get => creationTime;
            set
            {
                this.creationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }

        private string assignee;
        public string Assignee
        {
            get => assignee;
            set
            {
                try
                {
                    TaskM.Assignee = value;
                    assignee = value;
                    RaisePropertyChanged("Assignee");
                }
                catch (Exception e)
                {
                    Error = e.Message;
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
        public TaskVM(TaskModel taskM, UserModel userM)
        {
            TaskM = taskM;
            UserM = userM;
            this.id = taskM.Id;
            this.title = taskM.Title;
            this.description = taskM.Description;
            this.dueDate = taskM.DueDate;
            this.creationTime = taskM.CreationTime;
            this.assignee = taskM.Assignee;
            this.bController = taskM.Controller;
        }
    }
}
