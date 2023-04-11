using IntroSE.Kanban.Backend.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class Task
    {
        // Fields
        private readonly int id;
        internal int Id
        {
            get { return id; }

        }
        private string emailAssignee;
        internal string EmailAssignee
        {
            get { return emailAssignee; }
            set { emailAssignee = value; }
        }

        private readonly int MAX_TITLE_LENGTH = 50;
        private readonly int MIN_TITLE_LENGTH = 0;
        private readonly int MAX_DESCRIPTION_LENGTH = 300;

        private DateTime creationTime;
        internal DateTime CreationTime
        {
            get { return creationTime; }
            set { creationTime = value; }
        }
        private string title;
        internal string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string description;
        internal string Description
        {
            get { return description; }
            set { description = value; }
        }
        private DateTime dueDate;
        internal DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }
        //private string columnName;
        //internal string ColumnName
        //{
        //    get { return columnName; }
        //    set { columnName = value; }
        //}
        private int columnOrdinal;
        internal int ColumnOrdinal
        {
            get { return columnOrdinal; }
            set { columnOrdinal = value; }
        }
        private string boardName;
        internal string BoardName
        {
            get { return boardName; }
            set { boardName = value; }
        }
        private string boardCreator;
        internal string BoardCreator
        {
            get { return boardCreator; }
            set { boardCreator = value; }
        }
        private TaskDTO taskDTO;
        internal TaskDTO TaskDTO
        {
            get { return taskDTO; }
            set { taskDTO = value; }
        }


        //constructer
        internal Task(int id, string title, string description, DateTime dueDate, string emailAssignee, string boardName, string boardCreator)
        {
            this.id = id;
            this.emailAssignee = emailAssignee;
            this.creationTime = DateTime.Now;
            //this.columnName = "backlog";
            this.columnOrdinal = 0;
            this.boardName = boardName;
            this.boardCreator = boardCreator;


            if (title != null && (title.Length <= MAX_TITLE_LENGTH & title.Length > MIN_TITLE_LENGTH))
            {
                this.title = title;
            }
            else
            {
                throw new Exception("the title is too long or empty");
            }
            if (description != null && description.Length <= MAX_DESCRIPTION_LENGTH)
            {
                this.description = description;
            }
            else
            {
                throw new Exception("the description is too long or empty");
            }
            if (dueDate.CompareTo(DateTime.Now) >= 0)
            {
                this.dueDate = dueDate;
            }
            else
            {
                throw new Exception("The due date has passed");
            }
            taskDTO = new TaskDTO(id, title, description, CreationTime, dueDate, emailAssignee, boardName, boardCreator);

        }
        internal Task(int id, string title, string description, DateTime dueDate, DateTime creationTime, string emailAssignee,int columnOrdinal, TaskDTO taskDal, string boardName, string boardCreator)
        {
            this.id = id;
            this.emailAssignee = emailAssignee;
            this.creationTime = creationTime;
            //this.columnName = columnName;
            this.columnOrdinal = columnOrdinal;
            this.boardName = boardName;
            this.boardCreator = boardCreator;
            this.Title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.taskDTO = taskDal;
        }

        /// Methods

        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="newDescription">A description we should update to</param>
        internal void UpdateTaskDescription(string newDescription)
        {
                if (newDescription != null && newDescription.Length <= 300  )
                {
                    Description = newDescription;
                    taskDTO.Description = newDescription;
                }
                else
                {
                    throw new Exception("the description is too long");
                }
        }

        /// <summary>
        /// Update the title of a task
        /// </summary>
        /// <param name="title">A title we should update to</param>
        internal void UpdateTaskTitle(string title)
        {

                if (title.Length <= 50 & title.Length > 0)
                {
                    this.title = title;
                    taskDTO.Title = title;
                }
                else
                {
                    throw new Exception("the title is too long or empty");
                }
        }
        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="newDueDate">A due date we should update to</param>
        internal void UpdateTaskDueDate(DateTime newDueDate)
        {
                if (newDueDate.CompareTo(DateTime.Now) >= 0)
                {
                    this.DueDate = newDueDate;
                    taskDTO.DueDate = newDueDate;
                }
                else
                {
                    throw new Exception("The new due date has passed");
                }
            }
        
    }
}

