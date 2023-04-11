using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
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
    class ColumnDalController : DalController
    {
        private const string ColumnTableName = "Column";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ColumnDalController() : base(ColumnTableName)
        {

        }

        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            ColumnDTO result = new ColumnDTO(reader.GetInt32(0), reader.GetString(2), reader.GetString(1), (int)(long)reader.GetValue(3), reader.GetString(4),(int)(long)reader.GetValue(5));
            return result;
        }
        public bool Insert(ColumnDTO columnDal)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    // check if user was already inserted
                    if (columnDal.IsPersistent)
                    {
                        return false;
                    }
                    columnDal.IsPersistent = true;

                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} ({DTO.IDColumnName} ,{ColumnDTO.ColumnBoardNameColumnName},{ColumnDTO.ColumnEmailCreatorColumnName} ,{ColumnDTO.ColumnOrdinalColumnName} ,{ColumnDTO.ColumnNameColumnName},{ColumnDTO.ColumnTaskLimitColumnName}) " +
                        $"VALUES (@ID,@BoardName,@EmailCreator,@ColumnOrdinal,@ColumnName,@TaskLimit);";

                    SQLiteParameter idParam = new SQLiteParameter(@"ID", columnDal.Id);
                    SQLiteParameter boardNameParam = new SQLiteParameter(@"BoardName", columnDal.BoardName);
                    SQLiteParameter emailCreatorParam = new SQLiteParameter(@"EmailCreator", columnDal.EmailCreatorOfBoard);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"ColumnOrdinal", columnDal.ColumnOrdinal);
                    SQLiteParameter columnNameParam = new SQLiteParameter(@"ColumnName", columnDal.ColumnName);
                    SQLiteParameter taskLimitParam = new SQLiteParameter(@"TaskLimit", columnDal.TaskLimit);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(boardNameParam);
                    command.Parameters.Add(emailCreatorParam);
                    command.Parameters.Add(columnOrdinalParam);
                    command.Parameters.Add(columnNameParam);
                    command.Parameters.Add(taskLimitParam);
                    command.Prepare();
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
                log.Info("Insert new column successfully");
                return res > 0;
            }
        }

        // Delete all colomns of a given board
        public bool Delete(string boardName, string emailCreator, int columnOrdinal)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"DELETE from {ColumnTableName} WHERE {ColumnDTO.ColumnBoardNameColumnName}=@boardName AND {ColumnDTO.ColumnEmailCreatorColumnName}=@EmailCreator AND {ColumnDTO.ColumnOrdinalColumnName}=@ColumnOrdinal"
                };
                try
                {
                    SQLiteParameter boardNameParam = new SQLiteParameter(@"BoardName", boardName);
                    SQLiteParameter emailCreatorParam = new SQLiteParameter(@"EmailCreator", emailCreator);
                    SQLiteParameter columnOrdParam = new SQLiteParameter(@"ColumnOrdinal", columnOrdinal);
                    command.Parameters.Add(boardNameParam);
                    command.Parameters.Add(emailCreatorParam);
                    command.Parameters.Add(columnOrdParam);
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
            log.Info("delete column went successfully");
            return res > 0;
        }
        internal List<ColumnDTO> LoadColumns(string boardName, string emailCreator)
        {
            List<ColumnDTO> results = new List<ColumnDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"SELECT * FROM {ColumnTableName} WHERE {ColumnDTO.ColumnBoardNameColumnName}=@boardName AND {ColumnDTO.ColumnEmailCreatorColumnName}=@EmailCreator";
                SQLiteParameter boardNameParam = new SQLiteParameter(@"BoardName", boardName);
                SQLiteParameter emailCreatorParam = new SQLiteParameter(@"EmailCreator", emailCreator);
                command.Parameters.Add(boardNameParam);
                command.Parameters.Add(emailCreatorParam);
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add((ColumnDTO)(ConvertReaderToObject(dataReader)));
                    }
                    log.Info("Load Columns Data successfully");
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
                SQLiteCommand command = new SQLiteCommand($"SELECT Max(ID) FROM {ColumnTableName}", connection);
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