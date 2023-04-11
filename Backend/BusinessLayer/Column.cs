using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class Column : IComparable
    {
        // Fields
        private  string name;
        internal String Name
        {
            get { return name; }
            set { name = value; }
        }
        private List<Task> tasks;
        internal List<Task> Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }
        private int taskLimit = -1;
        internal int TaskLimit
        {
            get { return taskLimit; }
            set { taskLimit = value; }
        }
        private int columnOrdinal;
        internal int ColumnOrdinal
        {
            get { return columnOrdinal; }
            set {
                columnOrdinal = value;
                foreach(Task task in Tasks)
                {
                    task.ColumnOrdinal = value;
                    task.TaskDTO.ColumnOrdinal = value;
                }
            }
        }
        private string boardName;
        internal string BoardName { get; }
        private string emailCreatorOfBoard;
        internal string EmailCreatorOfBoard { get; }
        private ColumnDTO columnDTO;
        internal ColumnDTO ColumnDTO
        {
            get { return columnDTO; }
            set { columnDTO = value; }
        }


        //constructor
        internal Column(string name, string boardName, string emailCreatorOfBoard , int columnOrdinal,int id, ColumnDalController columnDalCon)
        {
            this.name = name;
            this.boardName = boardName;
            this.emailCreatorOfBoard = emailCreatorOfBoard;
            this.tasks = new List<Task>();
            this.columnOrdinal = columnOrdinal;
            ColumnDTO = new ColumnDTO(id, boardName, emailCreatorOfBoard, columnOrdinal, name, -1);
            columnDalCon.Insert(ColumnDTO);
        }

        internal Column(string name, string boardName, string emailCreatorOfBoard,int columnOrdinal, int taskLimit, ColumnDTO colDal,TaskDalController taskDalCon)
        {
            this.name = name;
            this.boardName = boardName;
            this.emailCreatorOfBoard = emailCreatorOfBoard;
            this.tasks = new List<Task>();
            this.columnOrdinal = columnOrdinal;
            this.taskLimit = taskLimit;
            ColumnDTO = colDal;
            List<TaskDTO> TaskDalList = taskDalCon.LoadTasks(columnOrdinal, boardName ,emailCreatorOfBoard);
            foreach (TaskDTO taskD in TaskDalList)
            {
                Console.WriteLine("------------check loading tasks from column! colomn ordinal " + columnOrdinal +"---------------------------");
                Console.WriteLine("------------check loading tasks from column! boardName " + boardName + "---------------------------");
                Console.WriteLine("------------check loading tasks from column! email of creator " + emailCreatorOfBoard + "---------------------------");
                Task taskB = new Task((int)(long)taskD.Id, taskD.Title, taskD.Description, taskD.DueDate, taskD.CreationTime, taskD.Asignee, taskD.ColumnOrdinal, taskD, taskD.BoardName, taskD.BoardCreator);
                Console.WriteLine("task title -  " + taskB.Title + "  //task id - " + taskB.Id + "  //task columnOrdinal - " + taskB.ColumnOrdinal);
                tasks.Add(taskB);
            }
        }
        internal Column(string name, string boardName, string emailCreatorOfBoard, int columnOrdinal) //moq for tests
        {
            this.name = name;
            this.boardName = boardName;
            this.emailCreatorOfBoard = emailCreatorOfBoard;
            this.tasks = new List<Task>();
            this.columnOrdinal = columnOrdinal;
        }

        // Methods

        /// <summary>
        /// get a list with all the tasks of this column
        /// </summary>
        /// <returns>A list with all the tasks of this column</returns>
        internal List<Task> GetAllTask()
        {
            return tasks;
        }

        /// <summary>
        /// Add a task to this column
        /// </summary>
        /// <param name="toAdd">The task we should add</param>
        internal void AddTask(Task toAdd)
        {
            CheckInput(toAdd);

            // check if there is space for new task according to limitation
            if (taskLimit == -1 || tasks.Count() < taskLimit)
            {
                this.tasks.Add(toAdd);
            }
            else
            {
                throw new Exception("This column has reached the maximum number of tasks");
            }
        }

        /// <summary>
        /// Remove a task from this column
        /// </summary>
        /// <param name="toRemove">The task we should remove</param>
        internal void RemoveTask(Task toRemove)
        {
            CheckInput(toRemove);
            if (!this.tasks.Contains(toRemove))
            {
                throw new Exception("This task doesn't exist in this column");
            }
            this.tasks.Remove(toRemove);
        }

        /// <summary>
        /// Get a task according to a given id
        /// </summary>
        /// <param name="taskId">id of a task to get</param>
        /// <returns>The task that has the given id</returns>
        internal Task GetTask(int taskId)
        {
            foreach (Task tsk in tasks)
            {
                if (tsk.Id == taskId)
                {
                    return tsk;
                }
            }
            throw new Exception("This task id doesn't exist in this column");
        }

        /// <summary>
        /// Check if a task is in the column with given task id
        /// </summary>
        /// <param name="id">id of the task we should check</param>
        /// <returns>True if the column has this task, False if not</returns>
        internal bool HasTask(int id)
        {
            foreach (Task tsk in tasks)
            {
                if (tsk.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if task input is null and throw an exception if it is
        /// </summary>
        /// <param name="tsk"></param>
        private void CheckInput(Task tsk)
        {
            if (tsk == null)
            {
                throw new Exception("Can't continue, the task is null");
            }
        }

        /// <summary>
        /// Set a new limit number of tasks to this column
        /// </summary>
        /// <param name="limit">A new limit to insert</param>
        internal void LimitColumn(int limit)
        {
            // Limit must be at least 1 and not more than the number of tasks in the column
            if (limit > 0 & limit >= tasks.Count | limit == -1)
            {
                this.taskLimit = limit;
                columnDTO.TaskLimit = limit;
            }

            else
                throw new Exception("A column limit is always bigger then zero and bigger then the size of the column ");
        }

        public int CompareTo(Column col)
        {
            return columnOrdinal.CompareTo(col.ColumnOrdinal);
        }

        public int CompareTo(object obj)
        {
            if (obj.GetType().Equals(this.GetType()))
            {
                return columnOrdinal.CompareTo(((Column)obj).ColumnOrdinal);
            }
            else
            {
                throw new Exception("types cannot be compared");
            }
        }

    }
}
