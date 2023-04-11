using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Presentation.Model
{
    internal class BoardModel : NotifiableModel
    {

        public string Name { get; }
        public string EmailCreator { get; }
        private readonly UserModel user;

        public ObservableCollection<ColumnModel> Columns { get; set; }

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

        private ColumnModel selectedColumn;
        public ColumnModel SelectedColumn
        {
            get
            {
                return selectedColumn;
            }
            set
            {
                if (selectedColumn != null)
                {
                    SelectedColumn.SortEnableForward = false;
                }
                selectedColumn = value;
                ColEnableForward = value != null;
                if (selectedColumn != null)
                {
                    selectedColumn.SortEnableForward = value != null;
                }
                RaisePropertyChanged("SelectedColumn");
            }
        }

        private bool colEnableForward = false;
        public bool ColEnableForward
        {
            get => colEnableForward;
            private set
            {
                colEnableForward = value;
                RaisePropertyChanged("ColEnableForward");
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
        public BoardModel(BackendController controller, UserModel user, string name, string emailCreator, bool isNew) : base(controller)
        {

            this.user = user;
            Name = name;
            EmailCreator = emailCreator;
            IList<ColumnModel> boardColumns = new List<ColumnModel>();
            // in case the board is new to the system
            if (isNew)
            {
                ColumnModel clm0 = new ColumnModel(controller, "backlog", user, this, 0, isNew);
                ColumnModel clm1 = new ColumnModel(controller, "in progress", user, this, 1, isNew);
                ColumnModel clm2 = new ColumnModel(controller, "done", user, this, 2, isNew);
                boardColumns.Add(clm0);
                boardColumns.Add(clm1);
                boardColumns.Add(clm2);
            }
            else // in case the board should be loaded from data base
            {
                boardColumns = (controller.GetBoard(user.Email, EmailCreator, Name).
                    Select((c, i) => new ColumnModel(controller, c.Name, user, this, c.ColOrdinal, false)).ToList());
            }
            Columns = new ObservableCollection<ColumnModel>(boardColumns);
            Columns.CollectionChanged += HandleChange;
        }

        /// <summary>
        /// removing column from UI list and from database
        /// </summary>
        /// <param name="colM">column to remove</param>
        public void RemoveColumn(ColumnModel colM)
        {
            Controller.RemoveColumn(user.Email, EmailCreator, Name, colM.ColumnOrdinal);
            if (colM.ColumnOrdinal == 0)
            {
                foreach (TaskModel task in colM.Tasks)
                {
                    Columns[1].Tasks.Add(task);
                }
            }
            else
            {
                foreach (TaskModel task in colM.Tasks)
                {
                    Columns[colM.columnOrdinal - 1].FlagAddTask = false;
                    Columns[colM.columnOrdinal - 1].Tasks.Add(task);
                    Columns[colM.columnOrdinal - 1].FlagAddTask = true;
                }

            }
            Columns.Remove(colM);
            for (int i = Columns.Count - 1; i >= 0; i = i - 1)
            {
                Columns[i].ColumnOrdinal = i;

            }
        }

        /// <summary>
        /// adding column to UI list and to database
        /// </summary>
        /// <param name="clm">column to add</param>
        public void AddColumn(ColumnModel clm)
        {
            Columns.Add(clm);
            Columns.Move(Columns.Count - 1, clm.ColumnOrdinal);
            for (int i = Columns.Count - 1; i >= 0; i = i - 1)
            {
                Columns[i].ColumnOrdinal = i;

            }
            RaisePropertyChanged("SelectedBoard");
        }

        /// <summary>
        /// adding task to UI list and to database in the leftmost column
        /// </summary>
        /// <param name="task">task to add</param>
        public void AddTask(TaskModel task)
        {
            // check so we dont insert to the list without inserting to database
            if (task.dueDate.CompareTo(DateTime.Now) >= 0)
            {
                Columns[0].Tasks.Add(task);
            }
            else
            {
                throw new Exception("Due date has passed");
            }
        }

        /// <summary>
        /// shifting the column to the new ordinal
        /// </summary>
        /// <param name="col">column to move</param>
        /// <param name="newOrdinal">new ordinal for the column</param>
        public void MoveColumn(ColumnModel col, int newOrdinal)
        {

            int oldOrdinal = col.ColumnOrdinal;
            col.Move(newOrdinal);
            col.ColumnOrdinal = newOrdinal;
            Columns.Move(oldOrdinal, newOrdinal);
            if (oldOrdinal < newOrdinal)
            {
                for (int i = 0; i < newOrdinal; i = i + 1)
                {
                    Columns[i].ColumnOrdinal = i;

                }
            }
            if (newOrdinal < oldOrdinal)
            {
                for (int i = Columns.Count - 1; i > newOrdinal; i = i - 1)
                {
                    Columns[i].ColumnOrdinal = i;

                }
            }
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            // in case of removing column
            //if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    foreach (ColumnModel colM in e.OldItems)
            //    {
            //        Controller.RemoveColumn(user.Email, EmailCreator, Name, colM.ColumnOrdinal);
            //    }
            //}

            // in case of adding column
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (ColumnModel colM in e.NewItems)
                {
                    Controller.AddColumn(user.Email, EmailCreator, Name, colM.ColumnOrdinal, colM.Name);
                }
            }
        }

        /// <summary>
        /// advancing the task one column forward if the user advancing is task assignee
        /// </summary>
        /// <param name="task">task to advance</param>
        internal void AdvanceTask(TaskModel task)
        {
            try
            {
                bool advanced = Columns[task.ColumnOrdinal].AdvanceTask(task);
                if (advanced)
                {
                    task.ColumnOrdinal = task.ColumnOrdinal + 1;
                    Columns[task.ColumnOrdinal].Tasks.Add(task);
                }
                SelectedTask = null;
            }
            catch (Exception e)
            {
                SelectedTask = null;
                throw new Exception(e.Message);
            }
        }
    }
}