using IntroSE.Kanban.Backend.BusinessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class User
    {
        // Fields


        private string email;
        internal string Email
        {
            get { return email; }
            set { email = value; }
        }
        private Password password;
        internal Password Password
        {
            get { return password; }
            set { password = value; }
        }

        private List<Password> oldPasswords;
        internal List<Password> OldPasswords
        {
            get { return oldPasswords; }
        }
        internal bool logedin;
        internal bool Logedin
        {
            get { return logedin; }
            set { logedin = value; }
        }
        private UserDTO userDTO;
        internal UserDTO UserDTO
        {
            get { return userDTO; }
            set { userDTO = value; }
        }

        //constructer
        internal User(string email, string password, UserDTO userDal)
        {
            Logedin = false;
            this.email = email;
            Password newPassword = new Password(password);
            this.password = newPassword;
            this.oldPasswords = new List<Password>();
            this.oldPasswords.Add(newPassword);
            UserDTO = userDal;

        }
        internal User(string email, string password) // mock for test
        {
            Logedin = false;
            this.email = email;
            Password newPassword = new Password(password);
            this.password = newPassword;
            this.oldPasswords = new List<Password>();
            this.oldPasswords.Add(newPassword);

        }
        internal User(string email, string password, List<string> oldPasswords, UserDTO userDal)
        {
            this.email = email;
            Password newPassword = new Password(password,true);
            this.password = newPassword;
            this.oldPasswords = new List<Password>();
            foreach (string pass in oldPasswords)
            {
                this.oldPasswords.Add(new Password(pass,true));
            }
            UserDTO = userDal;

        }
        // Methods

        /// <summary>
        /// Logging in the user to the system
        /// </summary>
        /// <param name="password">The password of this user</param>
        internal void Login(string password)
        {
            logedin = true;
        }

        /// <summary>
        /// Logging out the user from the system
        /// </summary>
        internal void Logout()
        {
            if (logedin)
                logedin = false;
            else
                throw new Exception("to logout the user need to be logedin");

        }
    }
}
