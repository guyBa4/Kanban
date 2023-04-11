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
    class TaskDalController : DalController
    {
        private const string TaskTableName = "Task";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public TaskDalController() : base(TaskTableName)
        {

        }

        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO result = new TaskDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(3), (DateTime)DateTime.Parse(reader.GetString(2)), (DateTime)DateTime.Parse(reader.GetString(4)), reader.GetInt32(5), (string)reader.GetValue(6), (string)reader.GetValue(7), (string)reader.GetValue(8));
            return result;
        }

        public bool Insert(TaskDTO task)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    // check if user was already inserted
                    if (task.IsPersistent)
                    {
                        return false;
                    }
                    task.IsPersistent = true;

                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({DTO.IDColumnName} ,{TaskDTO.TaskTitleColumnName},{TaskDTO.TaskCreationTimeColumnName},{TaskDTO.TaskDescriptionColumnName},{TaskDTO.TaskDueDateColumnName},{TaskDTO.TaskColumnOrdinalColumnName},{TaskDTO.TaskAsigneeColumnName},{TaskDTO.TaskBoardNameColumnName},{TaskDTO.TaskBoardCreatorColumnName}) " +
                        $"VALUES (@ID,@Title,@CreationTime,@Description,@DueDate,@ColumnOrdinal,@Asignee,@BoardName,@BoardCreator);";

                    SQLiteParameter idParam = new SQLiteParameter(@"ID", task.Id);
                    SQLiteParameter titleParam = new SQLiteParameter(@"Title", task.Title);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"CreationTime", task.CreationTime);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"Description", task.Description);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"DueDate", task.DueDate);
                    SQLiteParameter coulmnOrdinalParam = new SQLiteParameter(@"ColumnOrdinal", task.ColumnOrdinal);
                    SQLiteParameter asigneeParam = new SQLiteParameter(@"Asignee", task.Asignee);
                    SQLiteParameter boardNameParam = new SQLiteParameter(@"BoardName", task.BoardName);
                    SQLiteParameter boardCreatorParam = new SQLiteParameter(@"BoardCreator", task.BoardCreator);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(coulmnOrdinalParam);
                    command.Parameters.Add(asigneeParam);
                    command.Parameters.Add(boardNameParam);
                    command.Parameters.Add(boardCreatorParam);
                    command.Prepare();
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
                log.Info("Insert new task successfully");
                return res > 0;
            }
        }

        public bool Delete(int id, string title)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {TaskTableName} where {TaskDTO.TaskTitleColumnName}=@Title AND {DTO.IDColumnName}=@ID"
                };
                try
                {
                    SQLiteParameter idParam = new SQLiteParameter(@"ID", id);
                    SQLiteParameter titleParam = new SQLiteParameter(@"Title", title);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    if (e.Message != null) { log.Info(e); }
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            log.Info("Delete task successfully");
            return res > 0;
        }


        public List<TaskDTO> SelectAllTasks()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();
            log.Info("Select all Tasks successfully");
            return result;
        }

        internal List<TaskDTO> LoadTasks(int ColumnOrdinal, string boardNmae, string emailOfcreator)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {TaskTableName} where {TaskDTO.TaskColumnOrdinalColumnName}=@ColumnOrdinal AND { TaskDTO.TaskBoardNameColumnName}=@BoardName AND { TaskDTO.TaskBoardCreatorColumnName}=@BoardCreator";
                SQLiteParameter coulmnOrdinalParam = new SQLiteParameter(@"ColumnOrdinal", ColumnOrdinal);
                SQLiteParameter boardNameParam = new SQLiteParameter(@"BoardName", boardNmae);
                SQLiteParameter boardCreatorParam = new SQLiteParameter(@"BoardCreator", emailOfcreator);
                command.Parameters.Add(coulmnOrdinalParam);
                command.Parameters.Add(boardNameParam);
                command.Parameters.Add(boardCreatorParam);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add((TaskDTO)(ConvertReaderToObject(dataReader)));
                    }
                    log.Info("Load Tasks Data successfully");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        ;
                        dataReader.Close();
                    }
                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        public int maxId()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand($"SELECT Max(ID) FROM {TaskTableName}", connection);
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