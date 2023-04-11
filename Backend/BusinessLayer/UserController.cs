using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using IntroSE.Kanban.Backend.DataAccessLayer;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("BackendTest")]

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class UserController
    {
        // Fields
        private int userIdCounter = 0;
        internal int UserIdCounter
        {
            get { return userIdCounter; }
            set { userIdCounter = value; }
        }
        private Dictionary<string, User> _users = new Dictionary<string, User>();
        UserDalController uDalCon = new UserDalController();
        internal UserDalController UDalCon
        {
            get { return uDalCon; }
        }

        // Methods----
        internal UserController()
        {
        }

        //dictionary that keeps the user's email as a key and the user as a value 
        internal Dictionary<string, User> GetDict()
        {
            return _users;
        }

        // Methods

        /// <summary>
        /// Get a user according to the given email
        /// </summary>
        /// <param name="email">Email of the user we want</param>
        /// <returns>A user with the given email</returns>
        internal User GetUser(string email)
        {
            if (!GetDict().ContainsKey(email))
                throw new Exception("this user doesn't exist");
            else
                return GetDict()[email];
        }

        /// <summary>
        /// Checks if there's a user with the given email
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>True if there is user with this email, False if not</returns>
        internal bool HasUser(string email)
        {
            return _users.ContainsKey(email);
        }

        /// <summary>
        /// Registers a new user to the system.
        /// </summary>
        /// <param name="email">the user email address, used as the username for logging the system.</param>
        /// <param name="password">the user password.</param>
        /// <returns>The user that registered to the system</returns>
        internal User Register(string email, string password)
        {
            if (email is null | password is null)
            {
                throw new Exception($"Email or password is null");
            }
            if (IsUniqueEmail(email) & LegalEmail(email))
            {
                UserIdCounter++;
                UserDTO userDal = new UserDTO(userIdCounter, email, password, password.ToString());
                User u = new User(email, password, userDal);
                uDalCon.Insert(userDal);
                
                _users.Add(email, u); //adding to dictionary of existing users
                return u;
            }
            else
            {
                throw new Exception($"Email {email} already exists or not valid context");
            }
        }
        internal User mockRegister(string email, string password)
        {
            if (email is null | password is null)
            {
                throw new Exception($"Email or password is null");
            }
                UserIdCounter++;
                User u = new User(email, password);
                _users.Add(email, u); //adding to dictionary of existing users
                return u;
        }

        /// <summary>
        /// Checks if this email is unique in the user dictionary
        /// </summary>
        /// <param name="email">Email to check if exsits</param>
        /// <returns>True if the email doesn't exists, False if email exsits</returns>
        internal bool IsUniqueEmail(string email)
        {
            if (_users.ContainsKey(email))
            {
                throw new Exception($"Email {email} already exists");
            }
            return true;
        }

        /// <summary>
        /// Checks if the email answers the requirements of an email adress structure
        /// </summary>
        /// <param name="email">Email to check if is legal</param>
        /// <returns>True if the email answers the requirements of an email adress structure, False if not</returns>
        internal bool LegalEmail(string email)
        {
            var r = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
            return !String.IsNullOrEmpty(email) && r.IsMatch(email);
        }

        /// <summary>
        /// Check if email and password match to each other
        /// </summary>
        /// <param name="email">email of user</param>
        /// <param name="password">password of user</param>
        /// <returns>True if email and password match to each other, False if not</returns>
        internal bool ValidatePasswordMatch(string email, string password)
        {
            if (_users[email].Password.ToString().Equals(password))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Logging in the user to the system
        /// </summary>
        /// <param name="email">email of user to log in</param>
        /// <param name="password">password of user</param>
        internal void Login(string email, string password)
        {
            if (!_users.ContainsKey(email) | email is null)
            {
                throw new Exception("This user doesn't exists");
            }
            else
            {
                //Password pass = new Password(password);
                if (ValidatePasswordMatch(email, password))
                {
                    GetUser(email).Login(password);
                }
                else
                {
                    throw new Exception("the password doesn't match this user's password");
                }
            }
        }

        /// <summary>
        /// Logging out the user from the system
        /// </summary>
        /// <param name="email">email of logged in user</param>
        internal void Logout(string email)
        {
            if (!_users.ContainsKey(email) | email is null)
            {
                throw new Exception("This user doesn't exists");
            }
            else
            {
                GetUser(email).Logout();
            }
        }
        /// <summary>
        /// Removes all persistent data
        /// </summary>
        public void DeleteData()
        {
            uDalCon.Destroy();
            foreach (var pair in _users)
            {
                pair.Value.UserDTO.IsPersistent = false;
            }
        }
        /// <summary>
        /// Load data from database
        /// </summary>
        public void LoadData()
        {
            Dictionary<string, UserDTO> users = uDalCon.LoadUsers();
            _users = new Dictionary<string, User>();
            foreach (var pair in users)
            {
                UserDTO userD = pair.Value;
                string userEmail = pair.Key;
                _users.Add(userEmail, new User(userD.Email, userD.Password, userD.OldPasswords.Split(',').ToList(), userD));
            }
            if (_users.Count != 0)
            {
                userIdCounter = uDalCon.maxId();
            }
            else
            {
                userIdCounter = 0;
            }
        }
    }
}
