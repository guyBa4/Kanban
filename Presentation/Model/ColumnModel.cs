using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace Presentation.Model
{
    internal class ColumnModel : NotifiableModel
    {
        private string name;
        public string Name
        {
            get => name;
            set
            {
                Controller.RenameColumn(user.Email, Board.EmailCreator, Board.Name, ColumnOrdinal, value);
                name = value;
                RaisePropertyChanged("Name");
            }
        }
        private bool flagAddTask;
        public bool FlagAddTask
        {
            get => flagAddTask;
            set
            {
                flagAddTask = value;
            }
        }

        // we want this text to show only if there is limit
        private string toLimit = "(type -1 for no limit)";
        public string ToLimit
        {
            get
            {
                if (limit > 0)
                {
                    return toLimit;
                }
                else
                {
                    return "";
                }
            }
        }

        private int limit;
        public string Limit
        {
            get
            {
                limit = Controller.GetColumnLimit(user.Email, Board.EmailCreator, Board.Name, ColumnOrdinal);
                if (limit >= 0)
                {
                    return limit.ToString();
                }
                else
                {
                    return "∞";
                }
            }
            set
            {
                try
                {
                    Controller.LimitColumn(user.Email, Board.EmailCreator, Board.Name, ColumnOrdinal, Int32.Parse(value));
                    limit = Controller.GetColumnLimit(user.Email, Board.EmailCreator, Board.Name, ColumnOrdinal);
                    RaisePropertyChanged("Limit");
                    RaisePropertyChanged("ToLimit");
                }
                catch
                {
                    Limit = limit.ToString();
                }
            }
        }

        public BoardModel Board { get; private set; }
        public int columnOrdinal;
        public int ColumnOrdinal
        {
            get => columnOrdinal;
            set
            {
                columnOrdinal = value;
                foreach (TaskModel task in Tasks)
                {
                    task.ColumnOrdinal = value;
                }
                RaisePropertyChanged("ColumnOrdinal");
            }
        }
        public ObservableCollection<TaskModel> Tasks { get; set; }
        private readonly UserModel user;

        private bool sortEnableForward = false;
        public bool SortEnableForward
        {
            get => sortEnableForward;
            set
            {
                if (Tasks.Count > 0)
                {
                    sortEnableForward = value;
                    RaisePropertyChanged("SortEnableForward");
                }
            }
        }

        //Constructor
        public ColumnModel(BackendController controller, string colName, UserModel user, BoardModel board, int columnOrdinal, bool isNew) : base(controller)
        {
            this.limit = -1;
            this.user = user;
            this.name = colName;
            this.Board = board;
            this.columnOrdinal = columnOrdinal;
            FlagAddTask = true;
            if (isNew)
            {
                Tasks = new ObservableCollection<TaskModel>();
            }
            else
            {
                Tasks = new ObservableCollection<TaskModel>(controller.GetColumn(user.Email, Board.EmailCreator, Board.Name, columnOrdinal).
                    Select((c, i) => new TaskModel(controller, c.Id, c.Title, c.Description, c.CreationTime, c.DueDate.ToString(), user.Email, Board.EmailCreator, c.emailAssignee, Board.Name, ColumnOrdinal)).ToList());
            }
            Tasks.CollectionChanged += HandleChange;
        }

        /// <summary>
        /// shifting this column to new ordinal
        /// </summary>
        /// <param name="newOrdinal">ordinal to move the column to</param>
        public void Move(int newOrdinal)
        {
            int shiftSize = newOrdinal - columnOrdinal;
            Controller.MoveColumn(user.Email, Board.EmailCreator, Board.Name, ColumnOrdinal, shiftSize);
        }

        /// <summary>
        /// Sorting tasks list
        /// </summary>
        internal void SortTasks()
        {
            int n = Tasks.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    // "CompareTo" set to compare due dates
                    if (Tasks[j].CompareTo(Tasks[j + 1]) > 0)
                    {
                        // swap arr[j+1] and arr[j]
                        TaskModel temp = Tasks[j];
                        Tasks[j] = Tasks[j + 1];
                        Tasks[j + 1] = temp;
                    }
                }
            }
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            //in case of adding task to the column
            if (flagAddTask & (ColumnOrdinal == 0) & (e.Action == NotifyCollectionChangedAction.Add))
            {

                foreach (TaskModel tsk in e.NewItems)
                {
                    Controller.AddTask(user.Email, Board.EmailCreator, Board.Name, tsk.Title, tsk.Description, DateTime.Parse(tsk.DueDate));
                }
            }
        }

        /// <summary>
        /// advancing the task one column forward if the user advancing is task assignee
        /// </summary>
        /// <param name="task">task to advance</param>
        internal bool AdvanceTask(TaskModel task)
        {
            try
            {
                bool advanced = Controller.AdvanceTask(user.Email, Board.EmailCreator, Board.Name, ColumnOrdinal, task.Id);
                if (advanced)
                {
                    Tasks.Remove(task);
                }
                return advanced;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}