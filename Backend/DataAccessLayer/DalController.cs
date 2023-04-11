using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class DalController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected readonly string _connectionString;
        private readonly string _tableName;
        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
        }

        public bool Update(long id, string attributeName, string attributeValue)
        {
           
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where ID=@ID"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@ID", id));
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (e != null)
                        res = -1;
                    log.Error("Error while 'Update' action in DalController!");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        public bool Update(long id, string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }

            }
            return res > 0;
        }

        protected List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                }
                finally
                {
                    if (dataReader != null)
                    {;
                        dataReader.Close();
                    }
                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);

        public bool Delete(DTO DTOObj)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where id={DTOObj.Id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }


        public bool Destroy()
        {
            using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                {
                    try
                    {
                        command.CommandText = $"delete from {_tableName}";
                        command.Prepare();
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        if (e != null)
                            res = -1;
                    }
                    finally
                    {
                        command.Dispose();
                        connection.Close();
                    }
                }
                return res > 0;

            }
        }
    }
}