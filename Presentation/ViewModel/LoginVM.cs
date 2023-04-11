using IntroSE.Kanban.Backend.ServiceLayer;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ViewModel
{
    class LoginVM : Notifiable
    {
        public BackendController bController { get; private set; }
        private string email = "";
        public string Email
        {
            get => email;
            set
            {
                email = value;
                RaisePropertyChanged("Email");
            }
        }
        private string password = "";
        public string Password
        {
            get => password;
            set
            {
                this.password = value;
                RaisePropertyChanged("Password");
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
        public LoginVM()
        {
            this.bController = new BackendController();
        }

        /// <summary>
        /// Login the user to the system
        /// </summary>
        /// <returns>User that logged in if succeded</returns>
        public UserModel Login()
        {
            Error = "";
            try
            {
                return bController.Login(Email, Password);
            }
            catch (Exception e)
            {
                Error = e.Message;
                return null;
            }
        }

        /// <summary>
        /// Register the user to the system
        /// </summary>
        public void Register()
        {
            Error = "";
            try
            {
                bController.Register(Email, Password);
                Error = "Registered successfully";
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
        }

        /// <summary>
        /// Loading data from database
        /// </summary>
        internal void LoadData()
        {
            Error = "";
            try
            {
                bController.LoadData();
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
        }
    }
}
