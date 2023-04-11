
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace Presentation.Model
{
    internal class TaskModel : NotifiableModel, IComparable
    {
        public SolidColorBrush BackgroundColor
        {
            get
            {
                if (DateTime.Now.CompareTo(dueDate) >= 0)
                {
                    // if due date passed
                    return new SolidColorBrush(Colors.Red);
                }
                // Check if 75% of time since the creation time has passed
                bool CloseToEnd = (DateTime.Now.Ticks - CreationTime.Ticks) >= 0.75 * (dueDate.Ticks - CreationTime.Ticks);
                if (CloseToEnd)
                {
                    return new SolidColorBrush(Colors.Orange);
                }
                else
                {
                    return null;
                }
            }
        }

        public SolidColorBrush BorderColor
        {
            get
            {
                // Check if the task assigned to the logged in user
                bool assignedToUser = this.Assignee.Equals(UserEmail);
                if (assignedToUser)
                {
                    return new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    return null;
                }
            }
        }

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
                Controller.UpdateTaskTitle(UserEmail, creatorEmail, boardName, columnOrdinal, Id, value);
                this.title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string description;
        public string Description
        {
            get => description;
            set
            {
                Controller.UpdateTaskDescription(UserEmail, creatorEmail, boardName, columnOrdinal, Id, value);
                this.description = value;
                RaisePropertyChanged("Body");
            }
        }

        public DateTime dueDate;
        public string DueDate
        {
            get => dueDate.ToString();
            set
            {
                Controller.UpdateTaskDueDate(UserEmail, creatorEmail, boardName, columnOrdinal, Id, DateTime.Parse(value));
                this.dueDate = DateTime.Parse(value);
                RaisePropertyChanged("DueDate");
                RaisePropertyChanged("BackgroundColor");
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
        internal string Assignee
        {
            get => assignee;
            set
            {
                Controller.AssignTask(UserEmail, creatorEmail, boardName, columnOrdinal, Id, value);
                assignee = value;
                RaisePropertyChanged("Assignee");
                RaisePropertyChanged("BorderColor");
            }
        }
        private string creatorEmail;
        public string CreatorEmail
        {
            get => creatorEmail;
        }

        private string boardName;
        public string BoardName
        {
            get => boardName;
        }

        private int columnOrdinal;
        public int ColumnOrdinal
        {
            get => columnOrdinal;
            set
            {
                this.columnOrdinal = value;
                RaisePropertyChanged("ColumnOrdinal");
            }
        }
        private string UserEmail;

        //Constructor
        internal TaskModel(BackendController controller, int id, string title, string description, DateTime creationTime, string dueDate, string user_email, string creator_email, string asignee_email, string boardName, int columnOrdinal) : base(controller)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.creationTime = creationTime;
            this.dueDate = DateTime.Parse(dueDate);
            this.UserEmail = user_email;
            this.creatorEmail = creator_email;
            this.boardName = boardName;
            this.columnOrdinal = columnOrdinal;
            this.assignee = asignee_email;
        }


        /// <summary>
        /// Compare two task by their due dates
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>0 if equal , 1 if this bigger, -1 if this smaller</returns>
        public int CompareTo(object obj)
        {
            DateTime thisDueDate = DateTime.Parse(DueDate);
            return thisDueDate.CompareTo(DateTime.Parse(((TaskModel)obj).DueDate));
        }
    }
}
