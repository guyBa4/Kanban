
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using System.Data;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class BoardDalController : DalController
    {
        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string boardTableName = "Board";

        public BoardDalController() : base(boardTableName)
        {


        }

        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result = new BoardDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3).Split(',').ToList<string>());
            return result;
        }

        public bool Insert(BoardDTO board)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                int res = -1;
                try
                {
                    // check if board was already inserted
                    if (board.IsPersistent)
                    {
                        return false;
                    }
                    board.IsPersistent = true;

                    connection.Open();
                    command.CommandText = $"INSERT INTO {boardTableName} ({DTO.IDColumnName} ,{BoardDTO.BoardnameColumnName},{BoardDTO.BoardCreatorEmailColumnName},{BoardDTO.BoardmembersEmailsColumnName}) " +
                        $"VALUES (@ID,@Name,@CreatorEmail,@MembersEmail);";

                    SQLiteParameter idParam = new SQLiteParameter(@"ID", board.Id);
                    SQLiteParameter nameParam = new SQLiteParameter(@"Name", board.Name);
                    SQLiteParameter creatorEmailParam = new SQLiteParameter(@"CreatorEmail", board.CreatorEmail);
                    SQLiteParameter membersEmailsParam = new SQLiteParameter(@"MembersEmail", board.MembersEmails);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(creatorEmailParam);
                    command.Parameters.Add(membersEmailsParam);
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
                log.Info("Insert new board successfully");
                return res > 0;
            }
        }

        public bool Delete(string email, string name)
        {

            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {boardTableName} where {BoardDTO.BoardCreatorEmailColumnName}=@CreatorEmail AND {BoardDTO.BoardnameColumnName}=@Name"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter("@CreatorEmail", email));
                    command.Parameters.Add(new SQLiteParameter("@Name", name));
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
            log.Info("Delete board successfully");
            return res > 0;
        }


        public Dictionary<string, List<BoardDTO>> LoadBoards()
        {
            List<BoardDTO> result = SelectAllBoards();
            Dictionary<string, List<BoardDTO>> boards = new Dictionary<string, List<BoardDTO>>();
            foreach (BoardDTO board in result)
            {
                if (boards.ContainsKey(board.CreatorEmail))
                {
                    boards[board.CreatorEmail].Add(board);
                }
                else
                {
                    List<BoardDTO> list = new List<BoardDTO>();
                    list.Add(board);
                    boards.Add(board.CreatorEmail, list);
                }
            }
            log.Info("Load boards Data successfully");
            return boards;
        }


        public List<BoardDTO> SelectAllBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            log.Info("Select boards Data successfully");
            return result;
        }
        public int maxId()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand($"SELECT Max(ID) FROM {boardTableName}", connection);
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
