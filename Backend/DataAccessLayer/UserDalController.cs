using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class UserDalController : DalController
    {
        private const string UserTableName = "User";
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public UserDalController() : base(UserTableName)
        {

        }

        public bool Insert(UserDTO userDal)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {

                    // check if user was already inserted
                    if (userDal.IsPersistent)
                    {
                        return false;
                    }
                    userDal.IsPersistent = true;


                    command.CommandText = $"INSERT INTO {UserTableName} ({DTO.IDColumnName} ,{UserDTO.UserEmailColumnName} ,{UserDTO.UserPasswordColumnName},{UserDTO.UserOldPasswordsColumnName}) " +
                        $"VALUES (@ID,@Email,@Password,@OldPasswords);";

                    SQLiteParameter idParam = new SQLiteParameter(@"ID", userDal.Id);
                    SQLiteParameter emailParam = new SQLiteParameter(@"Email", userDal.Email);
                    SQLiteParameter passParam = new SQLiteParameter(@"Password", userDal.Password);
                    SQLiteParameter oldPassParam = new SQLiteParameter(@"OldPasswords", userDal.OldPasswords);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passParam);
                    command.Parameters.Add(oldPassParam);
                    command.Prepare();
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (e != null)
                    {
                        log.Info(e);
                        res = -1;
                    }
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                log.Info("Insert new user successfully");
                return res > 0;
            }
        }

        public bool Delete(string email, string password)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE from {UserTableName} WHERE {UserDTO.UserEmailColumnName}=@Email AND {UserDTO.UserPasswordColumnName}=@passVal"
                };
                try
                {
                    SQLiteParameter emailParam = new SQLiteParameter(@"Email", email);
                    SQLiteParameter passParam = new SQLiteParameter(@"passVal", password);

                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passParam);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (e.Message != null)
                    {
                        log.Info(e);
                    }
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            log.Info("Delete user successfully");
            return res > 0;
        }
       
        public List<UserDTO> SelectAllUsers()
        {
            log.Info("Select all users successfully");
            return Select().Cast<UserDTO>().ToList();
        }

        public Dictionary<string, UserDTO> LoadUsers()
        {
            List<UserDTO> users = SelectAllUsers();
            Dictionary<string, UserDTO> dictionary = new Dictionary<string, UserDTO>();
            foreach (UserDTO user in users)
            {
                dictionary.Add(user.Email, user);
            }
            log.Info("Loads all users successfully");
            return dictionary;
        }

        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO user = new UserDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));

            return user;

        }
        public int maxId()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand($"SELECT Max(ID) FROM {UserTableName}", connection);
                SQLiteDataReader dataReader = null;
                int maxValue = -1;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        maxValue = dataReader.GetInt32(0);
                    }
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

                return maxValue;
            }
        }
    }
}
