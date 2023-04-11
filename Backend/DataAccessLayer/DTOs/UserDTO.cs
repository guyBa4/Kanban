
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class UserDTO : DTO
    {
        public const string UserEmailColumnName = "Email";
        public const string UserPasswordColumnName = "Password";
        public const string UserOldPasswordsColumnName = "OldPasswords";


        private string email;
        internal string Email
        {
            get { return email; }

        }

        private string password;
        internal string Password
        {
            get { return password; }
            set
            {
                _controller.Update(Id, UserPasswordColumnName, value);
                email = value;
            }
        }

        private string oldPasswords;
        internal string OldPasswords
        {
            get { return oldPasswords; }
            set
            {
                _controller.Update(Id, UserOldPasswordsColumnName, value);
                email = value;
            }
        }


        internal UserDTO(int id,string email, string pass, string oldPasswords) : base(new UserDalController())
        {
            Id = id;
            this.email = email;
            password = pass;
            this.oldPasswords = oldPasswords;

        }

    }
}
