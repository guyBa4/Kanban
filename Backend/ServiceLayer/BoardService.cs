
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Collections.Generic;
using System;
using System.Linq;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;


namespace IntroSE.Kanban.Backend.ServiceLayer
{

    class BoardService
    {
        private UserService userServ;
        private BoardController boardCon;
        private UserController userCon;
        private ILog log;

        public BoardService()
        {
            userServ = new UserService();
            boardCon = userServ.GetBoardCon();
            userCon = userServ.GetUserCon();
            log = userServ.GetLog();
        }
        internal int GetTaskConter()
        {
            return boardCon.TaskIdCounter;
        }

        public UserService GetUserServ()
        {
            return userServ;
        }

        public Response LoadData()
        {
            try
            {
                userServ.LoadData();
                boardCon.LoadData();
                log.Info("loading data went Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("Couldn't load the data");
                return new Response(e.Message);
            }
        }

        ///<summary>Removes all persistent data.</summary>
        public Response DeleteData()
        {
            try
            {
                userServ.DeleteData();
                boardCon.DeleteData();
                log.Info("deleting data went Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("Couldn't delete the data");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Limit the number of tasks in a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                boardCon.LimitColumn(userEmail, creatorEmail, boardName, columnOrdinal, limit);
                if (boardCon.GetColumnLimit(userEmail, creatorEmail, boardName, columnOrdinal) == limit)
                    log.Info("Limit Column went Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("The column limit is illegal");
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// Get the limit of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The limit of the column.</returns>
        public Response<int> GetColumnLimit(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                int limit = boardCon.GetColumnLimit(userEmail, creatorEmail, boardName, columnOrdinal);
                log.Info("Column limit returned successfully");
                return Response<int>.FromValue(limit);
            }
            catch (Exception e)
            {
                return Response<int>.FromError(e.Message);
            }
        }

        /// <summary>
        /// Get the name of a specific column
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>The name of the column.</returns>
        public Response<string> GetColumnName(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                string name = boardCon.GetColumnName(userEmail, creatorEmail, boardName, columnOrdinal);
                log.Info("Column name returned successfully");
                return Response<string>.FromValue(name);
            }
            catch (Exception e)
            {
                return Response<string>.FromError(e.Message);
            }
        }

        /// <summary>
        /// Add a new task.
        /// </summary>
		/// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="title">Title of the new task</param>
        /// <param name="description">Description of the new task</param>
        /// <param name="dueDate">The due date if the new task</param>
        /// <returns>A response object with a value set to the Task, instead the response should contain a error message in case of an error</returns>

        public Response<Task> AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                BusinessLayer.Task taskB = boardCon.AddTask(userEmail, creatorEmail, boardName, title, description, dueDate);
                Task taskC = new Task(taskB.Id, taskB.CreationTime, taskB.Title, taskB.Description, taskB.DueDate, taskB.EmailAssignee, taskB.BoardName, taskB.BoardCreator, taskB.ColumnOrdinal);
                log.Info("Task returned successfully");
                return Response<Task>.FromValue(taskC);
            }
            catch (Exception e)
            {
                log.Error("Task requesrted does not ecxist (make sure to use 'has task()' method).");
                return Response<Task>.FromError(e.Message);
            }
        }
        /// <summary>
        /// Update the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                boardCon.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskId, dueDate);
                if (boardCon.GetBoard(creatorEmail, boardName).GetTask(taskId, columnOrdinal).DueDate.Equals(dueDate))
                    log.Info("Updating Task Due Date went Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Update task title
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                boardCon.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskId, title);
                if (boardCon.GetBoard(creatorEmail, boardName).GetTask(taskId, columnOrdinal).Title.Equals(title))
                    log.Info("Updating Task title went Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Update the description of a task
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                boardCon.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskId, description);
                if (boardCon.GetBoard(creatorEmail, boardName).GetTask(taskId, columnOrdinal).Description.Equals(description))
                    log.Info("Updating Task description went Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Advance a task to the next column
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                boardCon.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId);
                log.Info("task got advenced Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("there was a problem during advancing task attempet");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Returns a column given it's name
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <returns>A response object with a value set to the Column, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                List<BusinessLayer.Task> listB = boardCon.GetColumn(userEmail, creatorEmail, boardName, columnOrdinal);
                List<Task> listC = new List<Task>();
                log.Info("column return Successfully");

                foreach (BusinessLayer.Task taskB in listB)
                {
                    listC.Add(new Task((int)taskB.Id, taskB.CreationTime, taskB.Title, taskB.Description, taskB.DueDate, taskB.EmailAssignee, taskB.BoardName, taskB.BoardCreator, taskB.ColumnOrdinal));
                }
                return Response<IList<Task>>.FromValue(listC);
            }
            catch (Exception e)
            {
                log.Error("there was a problem during column return attempt");
                return Response<IList<Task>>.FromError(e.Message);
            }
        }
        /// <summary>
        /// Adds a board created by another user to the logged-in user. 
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response JoinBoard(string userEmail, string creatorEmail, string boardName)
        {

            try
            {
                boardCon.JoinBoard(userCon.GetUser(userEmail), creatorEmail, boardName);
                log.Info("Joining the board went Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("there was a problem during Join board attempt");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Adds a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">The name of the new board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AddBoard(string userEmail, string name)
        {
            try
            {
                boardCon.AddBoard(userEmail, name);
                log.Info("board added Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("there was a problem during adding board attempt");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Removes a board to the specific user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="name">The name of the board</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveBoard(string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                boardCon.RemoveBoard(userEmail, creatorEmail, boardName);
                log.Info("board removed Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("there was a problem during removing board attempt");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response object with a value set to the list of tasks, The response should contain a error message in case of an error</returns>
        public Response<IList<Task>> InProgressTasks(string userEmail)
        {
            try
            {
                List<BusinessLayer.Task> listB = boardCon.InProgressTasks(userEmail);
                List<Task> listC = new List<Task>();
                foreach (BusinessLayer.Task taskB in listB)
                {
                    listC.Add(new Task((int)taskB.Id, taskB.CreationTime, taskB.Title, taskB.Description, taskB.DueDate, userEmail, taskB.BoardName, taskB.BoardCreator, taskB.ColumnOrdinal));
                }
                log.Info("getting 'In Progress' Tasks went Successfully");
                return Response<IList<Task>>.FromValue(listC);
            }
            catch (Exception e)
            {
                log.Error("there was a problem during getting 'In Progress' Tasks attempt");
                return Response<IList<Task>>.FromError(e.Message);
            }
        }/// <summary>
         /// Assigns a task to a user
         /// </summary>
         /// <param name="userEmail">Email of the current user. Must be logged in</param>
         /// <param name="creatorEmail">Email of the board creator</param>
         /// <param name="boardName">The name of the board</param>
         /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
         /// <param name="taskId">The task to be updated identified task ID</param>        
         /// <param name="emailAssignee">Email of the user to assign to task to</param>
         /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            try
            {
                boardCon.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, emailAssignee);
                log.Info("task assigned Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("there was a problem during assignning task attempt");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Returns the list of board of a user. The user must be logged-in. The function returns all the board names the user created or joined.
        /// </summary>
        /// <param name="userEmail">The email of the user. Must be logged-in.</param>
        /// <returns>A response object with a value set to the board, instead the response should contain a error message in case of an error</returns>
        public Response<IList<String>> GetBoardNames(string userEmail)
        {
            try
            {
                IList<String> list = boardCon.GetBoardNames(userEmail);
                log.Info("getting 'board names' Tasks went Successfully");
                return Response<IList<String>>.FromValue(list);
            }
            catch (Exception e)
            {
                log.Error("there was a problem during getting the board's names attempt");
                return Response<IList<String>>.FromError(e.Message);
            }
        }

        public Response AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            try
            {
                boardCon.AddColumn(userEmail, creatorEmail, boardName, columnOrdinal, columnName);
                log.Info("column has been added Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("there was a problem during adding a column attempt");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Removes a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                boardCon.RemoveColumn(userEmail, creatorEmail, boardName, columnOrdinal);
                log.Info("column has been removed Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("there was a problem during removing a column attempt");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Renames a specific column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <param name="newColumnName">The new column name</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            try
            {
                boardCon.RenameColumn(userEmail, creatorEmail, boardName, columnOrdinal, newColumnName);
                log.Info("column has been Rename Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("there was a problem during Rename a column attempt");
                return new Response(e.Message);
            }
        }
        /// <summary>
        /// Moves a column shiftSize times to the right. If shiftSize is negative, the column moves to the left
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column location. The first column location is identified by 0, the location increases by 1 for each column</param>
        /// <param name="shiftSize">The number of times to move the column, relativly to its current location. Negative values are allowed</param>  
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public Response MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            try
            {
                boardCon.MoveColumn(userEmail, creatorEmail, boardName, columnOrdinal, shiftSize);
                log.Info("column has been moved Successfully");
                return new Response();
            }
            catch (Exception e)
            {
                log.Error("there was a problem during moving a column attempt");
                return new Response(e.Message);
            }
        }

        /// <summary>
        /// gets a business layer board
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <returns>A service board</returns>
        public Response<Board> GetBoard(string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                BusinessLayer.Board brd = boardCon.GetBoard(creatorEmail, boardName);
                LinkedList<Column> colms = new LinkedList<Column>();
                foreach (BusinessLayer.Column clm in brd.Columns)
                {
                    colms.AddLast(new Column(clm.Name, boardName, GetColumn(userEmail, creatorEmail, boardName, clm.ColumnOrdinal).Value, clm.ColumnOrdinal));
                }
                Board board = new Board(boardName, brd.EmailCreator, colms);
                // set column field to the new board
                foreach (Column clm in colms)
                {
                    clm.Board = board;
                }
                return Response<Board>.FromValue(board);
            }
            catch (Exception e)
            {
                log.Error("there was a problem getting board");
                return Response<Board>.FromError(e.Message);
            }
        }
    }
}
