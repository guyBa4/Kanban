using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Presentation.Model
{
    internal class UserModel : NotifiableModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }

        public ObservableCollection<TaskModel> InProgTasks { get; set; }

        //Constructor
        public UserModel(BackendController controller, string email) : base(controller)
        {
            this.Email = email;
            InProgTasks = new ObservableCollection<TaskModel>(controller.InProgressTasks(Email).
                Select((c, i) => new TaskModel(controller, c.Id, c.Title, c.Description, c.CreationTime, c.DueDate.ToString(), Email, c.BoardEmailCreator, c.emailAssignee, c.BoardName, c.ColumnOrdinal)).ToList());
        }

        /// <summary>
        /// Updating in progress tasks user list
        /// </summary>
        public void UpdateList()
        {
            InProgTasks = new ObservableCollection<TaskModel>(Controller.InProgressTasks(Email).
                Select((c, i) => new TaskModel(Controller, c.Id, c.Title, c.Description, c.CreationTime, c.DueDate.ToString(), Email, c.BoardEmailCreator, c.emailAssignee, c.BoardName, c.ColumnOrdinal)).ToList());
        }

    }
}
