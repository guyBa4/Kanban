using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IntroSE.Kanban.Backend.BusinessLayer.UserController;
using System.Collections;
using IntroSE.Kanban.Backend.DataAccessLayer;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BackendTest")]

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class BoardController
    {
        // Fields
        private Dictionary<string, List<Board>> boardsCreated;

        internal Dictionary<string, List<Board>> BoardsCreated
        {
            get { return boardsCreated; }
            set { boardsCreated = value; }
        }
        private Dictionary<string, List<Board>> boardsMember;
        internal Dictionary<string, List<Board>> BoardsMember
        {
            get { return boardsMember; }
            set { boardsMember = value; }
        }

        private Dictionary<string, List<Task>> assignedtasks;
        internal Dictionary<string, List<Task>> Assignedtasks
        {
            get { return assignedtasks; }
            set { assignedtasks = value; }
        }
        private UserController UserCon = new UserController();
        private int taskIdCounter;
        internal int TaskIdCounter
        {
            get { return taskIdCounter; }
            set { taskIdCounter = value; }
        }
        private int boardIdCounter;
        internal int BoardIdCounter
        {
            get { return boardIdCounter; }
            set { boardIdCounter = value; }
        }
        private int columnIdCounter;
        internal int ColumnIdCounter
        {
            get { return columnIdCounter; }
            set { columnIdCounter = value; }
        }
        private BoardDalController boardDalCon = new BoardDalController();
        private ColumnDalController columnDalCon = new ColumnDalController();
        private TaskDalController taskDalCon = new TaskDalController();

        //constructor

        internal BoardController()
        {
            BoardsCreated = new Dictionary<string, List<Board>>();
            boardsMember = new Dictionary<string, List<Board>>();
            assignedtasks = new Dictionary<string, List<Task>>();
            boardIdCounter = 0;
            columnIdCounter = 0;
            taskIdCounter = 0;
        }

        // Methods
        internal UserController GetUserController()
        {
            return UserCon;
        }

        /// <summary>
        /// Add board to the user's list of boards 
        /// </summary>
        /// <param name="email">email of user</param>
        /// <param name="name">name of board</param>
        internal void AddBoard(string email, string name)
        {

            if (!UserCon.GetUser(email).logedin)
            {
                throw new Exception("You need to login to exacute this action");
            }
            else
            {
                if (IsExists(email, name))
                {
                    throw new Exception("Board already exists");
                }
                else
                {
                    Object[] arr = { email, name };
                    CheckInput(arr);

                    Board board = new Board(name, email, boardIdCounter, boardDalCon, columnIdCounter, columnDalCon);
                    boardIdCounter++;
                    ColumnIdCounter = ColumnIdCounter + 3;

                    // Check if the email exists
                    if (!boardsCreated.ContainsKey(email))
                    {
                        List<Board> list = new List<Board>();
                        list.Add(board);
                        boardsCreated.Add(email, list);
                    }
                    else
                    {
                        boardsCreated[email].Add(board);

                    }
                    if (!boardsMember.ContainsKey(email))
                    {
                        List<Board> listM = new List<Board>();
                        listM.Add(board);
                        boardsMember.Add(email, listM);
                    }
                    else
                    {
                        boardsMember[email].Add(board);

                    }
                }
            }
        }



        /// <summary>
        /// Remove the board from the user's list of boards
        /// </summary>
        /// <param name="userEmail">email of user, must be logged in</param>
        /// <param name="creatorEmail">email of creator of the board, must be logged in</param>
        /// <param name="boardName">name of the board to remove</param>
        internal void RemoveBoard(string userEmail, string creatorEmail, string boardName)
        {
            if (!userEmail.Equals(creatorEmail))
            {
                throw new Exception("Only the creator can delete the board");
            }
            else
            {
                if (UserCon.GetUser(userEmail).logedin)
                {
                    // Check if the board name exists in this user board list
                    if (!IsExists(creatorEmail, boardName))
                    {
                        throw new Exception("Board doesn't exists");
                    }
                    else
                    {
                        Board brdToDelete = GetBoard(creatorEmail, boardName);
                        boardsCreated[creatorEmail].Remove(brdToDelete);
                        boardsMember[creatorEmail].Remove(brdToDelete);
                        boardDalCon.Delete(brdToDelete.EmailCreator, brdToDelete.Name);
                        foreach (Column col in brdToDelete.Columns)
                        {
                            columnDalCon.Delete(brdToDelete.Name, brdToDelete.EmailCreator, col.ColumnOrdinal);
                            List<Task> list = col.Tasks;
                            foreach (Task task in list)
                            {
                                taskDalCon.Delete(task.Id, task.Title);
                                assignedtasks[task.EmailAssignee].Remove(task);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("You need to login to exacute this action");
                }

            }
        }
        /// <summary>
        /// Check if the inputs are null, throw exception if one of them is null
        /// </summary>
        /// <param name="objArr">array of inputs to check if they are null</param>
        private void CheckInput(Object[] objArr)
        {
            for (int i = 0; i < objArr.Length; i++)
            {
                if (objArr[i] == null)
                {
                    throw new Exception("Can't continue, argument is null");
                }
            }
        }

        /// <summary>
        /// Add a new member to the members of the board
        /// </summary>
        /// <param name="joinUser">User to add</param>
        /// <param name="creatorEmail">email of the creator of the board</param>
        /// <param name="boardName">name of the board we should add the user to its members</param>
        internal void JoinBoard(User joinUser, string creatorEmail, string boardName)
        {
            if (joinUser.logedin)
            {
                Object[] arr = { joinUser, boardName };
                CheckInput(arr);

                string userEmail = joinUser.Email;
                Board board = GetBoard(creatorEmail, boardName);
                if (boardsMember.ContainsKey(userEmail))
                {
                    if (boardsMember[userEmail].Contains(board))
                        throw new Exception("The user is a member in the board so no action has been made");
                }
                board.JoinBoard(joinUser.Email);
                // Check if the email exists
                if (!boardsMember.ContainsKey(userEmail))
                {
                    List<Board> list = new List<Board>();
                    list.Add(board);
                    boardsMember.Add(userEmail, list);
                }
                else
                {
                    boardsMember[userEmail].Add(board);
                }
            }
            else
            {
                throw new Exception("The user needs to log in before he can join a board");
            }
        }

        /// <summary>
        /// get board by given name and email of creator
        /// </summary>
        /// <param name="email">email of creator</param>
        /// <param name="name">name of the board</param>
        /// <returns>The board with the given email creator and name</returns>
        internal Board GetBoard(string email, string name)
        {

            if (!boardsCreated.ContainsKey(email) & !boardsMember.ContainsKey(email))
            {
                throw new Exception("No boards were found under this email");
            }
            else
            {
                if (boardsCreated.ContainsKey(email))
                {
                    foreach (Board board in boardsCreated[email])
                    {
                        if (board.Name.Equals(name))
                        {
                            return board;
                        }
                    }
                }
                if (boardsMember.ContainsKey(email))
                {
                    foreach (Board board in boardsMember[email])
                    {
                        if (board.Name.Equals(name))
                        {
                            return board;
                        }
                    }
                }
                throw new Exception("Board name doesn't exist");
            }
        }

        /// <summary>
        /// Update task's due date
        /// </summary>
        /// <param name="userEmail">email of the user that is updating the task, must be logged in</param>
        /// <param name="creatorEmail">email of creator of the board</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <param name="taskId">id of the task</param>
        /// <param name="dueDate">a new due date we should update to</param>
        internal void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (!UserCon.GetUser(userEmail).logedin)
            {
                throw new Exception("You need to login before he can update the task due date");
            }
            else
            {
                Board board = GetBoard(creatorEmail, boardName);
                if (board.TaskIsDone(columnOrdinal))
                {
                    throw new Exception("The task is done and cannot be changed");
                }
                else
                {
                    Task task = board.GetColumn(columnOrdinal).GetTask(taskId);
                    if (task.EmailAssignee.Equals(userEmail))
                        task.UpdateTaskDueDate(dueDate);
                    else
                        throw new Exception("only the task's assignee can update the task duedate");

                }
            }
        }



        /// <summary>
        /// Update task's title
        /// </summary>
        /// <param name="userEmail">email of the user that is updating the task, must be logged in</param>
        /// <param name="creatorEmail">email of creator of the board</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <param name="taskId">id of the task</param>
        /// <param name="title">a new title we should update to</param>
        internal void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            if (!UserCon.GetUser(userEmail).logedin)
            {
                throw new Exception("You need to login before he can update the task title");
            }
            else
            {
                Board board = GetBoard(creatorEmail, boardName);
                if (board.TaskIsDone(columnOrdinal))
                {
                    throw new Exception("The task is done and cannot be changed");
                }
                else
                {
                    Task task = board.GetColumn(columnOrdinal).GetTask(taskId);
                    if (task.EmailAssignee.Equals(userEmail))
                    {
                        task.UpdateTaskTitle(title);
                    }
                    else
                    {
                        throw new Exception("only the task's assignee can update the task title");
                    }
                }
            }

        }

        /// <summary>
        /// Update task's description
        /// </summary>
        /// <param name="userEmail">email of the user that is updating the task, must be logged in</param>
        /// <param name="creatorEmail">email of creator of the board</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <param name="taskId">id of the task</param>
        /// <param name="description">a new description we should update to</param>
        internal void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {

            if (!UserCon.GetUser(userEmail).logedin)
            {
                throw new Exception("You need to login before he can update the task description");
            }
            else
            {
                Board board = GetBoard(creatorEmail, boardName);
                if (board.TaskIsDone(columnOrdinal))
                {
                    throw new Exception("The task is done and cannot be changed");
                }
                else
                {
                    Task task = board.GetColumn(columnOrdinal).GetTask(taskId);
                    if (task.EmailAssignee.Equals(userEmail))
                        task.UpdateTaskDescription(description);
                    else
                        throw new Exception("only the task's assignee can update the task description");

                }
            }
        }

        // Check if the board name exists in a specific user board list
        /// <summary>
        /// Check if the board name exists in a specific user board list
        /// </summary>
        /// <param name="email">email of user</param>
        /// <param name="name">name of board</param>
        /// <returns>True if the board name exsits, False if not</returns>
        private bool IsExists(string email, string name)
        {
            Object[] arr = { email, name };
            CheckInput(arr);
            if (!boardsCreated.ContainsKey(email))
            {
                return false;
            }
            List<Board> lst = boardsCreated[email];
            foreach (Board board in lst)
            {
                if (board.Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Limit a column to a number of tasks to hold
        /// </summary>
        /// <param name="userEmail">email of the user that is limiting the column, must be logged in</param>
        /// <param name="creatorEmail">email of creator of the board</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <param name="limit">A new limitation to update</param>
        internal void LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            if (UserCon.GetUser(userEmail).logedin)
            {
                GetBoard(creatorEmail, boardName).GetColumn(columnOrdinal).LimitColumn(limit);
            }
            else
            {
                throw new Exception("You need to login before he can limit this column");
            }
        }

        /// <summary>
        /// get the number of tasks the column can hold
        /// </summary>
        /// <param name="userEmail">email of the user, must be logged in</param>
        /// <param name="creatorEmail">email of creator of the board</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <returns>The number of tasks the column can hold</returns>
        internal int GetColumnLimit(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (UserCon.GetUser(userEmail).logedin)
                return GetBoard(creatorEmail, boardName).GetColumn(columnOrdinal).TaskLimit;
            else
                throw new Exception("You need to login before he can make this action");
        }

        /// <summary>
        /// get the column name with given parameters
        /// </summary>
        /// <param name="userEmail">email of the user, must be logged in</param>
        /// <param name="creatorEmail">email of creator of the board</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <returns>The column name with given parameters</returns>
        internal string GetColumnName(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (UserCon.GetUser(userEmail).logedin)
                return GetBoard(creatorEmail, boardName).GetColumn(columnOrdinal).Name;
            else
                throw new Exception("You need to login before he can make this action");
        }

        // get the user who created the board
        /// <summary>
        /// get the user who created the board
        /// </summary>
        /// <param name="board">the board</param>
        /// <returns>The user who created the board</returns>
        internal User GetBoardCreator(Board board)
        {
            if (IsExists(board.EmailCreator, board.Name))
            {
                return UserCon.GetUser(board.EmailCreator);
            }
            else
            {
                throw new Exception("Board doesn't exists");
            }
        }

        /// <summary>
        /// add new task to this board (only if you are board member)
        /// </summary>
        /// <param name="userEmail">email of user that is adding the task, must be logged in</param>
        /// <param name="creatorEmail">email of creator of the board</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="title">title of the task</param>
        /// <param name="description">description of the task</param>
        /// <param name="dueDate">due date of the task</param>
        /// <returns>The task that we added</returns>
        internal Task AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {

            if (UserCon.GetUser(userEmail).logedin)
            {

                Board brd = GetBoard(creatorEmail, boardName);
                if (brd.IsMember(userEmail))
                {

                    Task tsk = brd.AddTask(TaskIdCounter, title, description, dueDate, userEmail);
                    if (Assignedtasks.ContainsKey(userEmail))
                    {
                        Assignedtasks[userEmail].Add(tsk);
                    }
                    else
                    {
                        List<Task> list = new List<Task>();
                        list.Add(tsk);
                        Assignedtasks.Add(userEmail, list);
                    }
                    taskDalCon.Insert(tsk.TaskDTO);
                    TaskIdCounter = TaskIdCounter + 1;
                    return tsk;
                }
                else
                {
                    throw new Exception("You are not a board member");
                }

            }
            else
            {
                throw new Exception("You need to login before he can add task");
            }
        }

        /// <summary>
        /// Move a task to the next column
        /// </summary>
        /// <param name="userEmail">email of user that is advancing the task, must be logged in</param>
        /// <param name="creatorEmail">email of creator of the board</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <param name="taskId">id of the task to advance</param>
        internal void AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {

            if (!UserCon.GetUser(userEmail).logedin)
            {
                throw new Exception("You need to login before he can add task");
            }
            else
            {
                Board board = GetBoard(creatorEmail, boardName);
                if (!board.GetColumn(columnOrdinal).GetTask(taskId).EmailAssignee.Equals(userEmail))
                {
                    throw new Exception("Only the task's assignee can advance the task");
                }
                else
                {
                    board.AdvanceTask(userEmail, columnOrdinal, taskId);
                }
            }
        }

        /// <summary>
        /// get a column given its board and ordinal
        /// </summary>
        /// <param name="userEmail">email of the user</param>
        /// <param name="creatorEmail">email of creator of the board</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <returns>The column given its board and ordinal</returns>
        internal List<Task> GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (UserCon.GetUser(userEmail).logedin)
            {
                return GetBoard(creatorEmail, boardName).GetColumn(columnOrdinal).GetAllTask();
            }
            else
            {
                throw new Exception("You need to login before he can add task");
            }
        }

        /// <summary>
        /// Returns all the In progress tasks of the user.
        /// </summary>
        /// <param name="userEmail">Email of the user. Must be logged in</param>
        /// <returns>A response object with a value set to the list of tasks, The response should contain a error message in case of an error</returns>
        internal List<Task> InProgressTasks(string userEmail)
        {
            if (UserCon.GetUser(userEmail).logedin)
            {
                if (!assignedtasks.ContainsKey(userEmail) || assignedtasks[userEmail].Count == 0)
                {
                    return new List<Task>();
                }
                else
                {
                    List<Task> inProgress = new List<Task>();
                    foreach (Task task in assignedtasks[userEmail])
                    {
                        Console.WriteLine("task title -  " + task.Title + "  //task id - " + task.Id);
                        int lastCol = getBoard(task.BoardName, task.BoardCreator).Columns.Count() - 1;
                        if ((!task.ColumnOrdinal.Equals(0)) && (!task.ColumnOrdinal.Equals(lastCol)))
                        {
                            inProgress.Add(task);

                        }
                    }
                    return inProgress;
                }
            }
            else
            {
                throw new Exception("The user needs to login so he could execute this action");
            }
        }

        /// <summary>
        /// check if a member email is in the members list of given board
        /// </summary>
        /// <param name="boardname">name of the board</param>
        /// <param name="creatorEmail">email of the creator of the board</param>
        /// <param name="memberEmail">email of the member to check</param>
        /// <returns>True if the user is a member of the board, False if not</returns>
        internal bool IsMember(string boardname, string creatorEmail, string memberEmail)
        {
            return GetBoard(creatorEmail, boardname).IsMember(memberEmail);
        }


        /// <summary>
        /// Assigns a task to a user
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>        
        /// <param name="emailAssignee">Email of the user to assign to task to</param>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public void AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            if (!UserCon.GetUser(userEmail).logedin)
            {
                throw new Exception("To assign a task you need to be logged in");
            }
            else
            {
                Board board = GetBoard(creatorEmail, boardName);
                if (!board.IsMember(emailAssignee))
                {
                    throw new Exception("To assign a task to a user the user needs to be a board's member");
                }
                else
                {
                    Task task = board.GetColumn(columnOrdinal).GetTask(taskId);
                    string prevAsignee = task.EmailAssignee;
                    task.EmailAssignee = emailAssignee;
                    task.TaskDTO.Asignee = emailAssignee;
                    Assignedtasks[prevAsignee].Remove(task);
                    if (Assignedtasks.ContainsKey(emailAssignee))
                    {
                        Assignedtasks[emailAssignee].Add(task);
                    }
                    else
                    {
                        List<Task> list = new List<Task>();
                        list.Add(task);
                        Assignedtasks.Add(emailAssignee, list);
                    }
                }
            }
        }


        /// <summary>
        /// Returns the list of board of a user. The user must be logged-in. The function returns all the board names the user created or joined.
        /// </summary>
        /// <param name="userEmail">The email of the user. Must be logged-in.</param>
        /// <returns>A list with all the names of the boards of an user</returns>
        public IList<String> GetBoardNames(string userEmail)
        {
            if (!UserCon.GetUser(userEmail).logedin)
            {
                throw new Exception("To get the boards names you need to be logged in");
            }
            else
            {
                if (!boardsMember.ContainsKey(userEmail))
                {
                    return new List<String>();
                }
                else
                {

                    IList<string> boards = new List<string>();
                    foreach (Board nextBoard in BoardsMember[userEmail])
                    {
                        boards.Add(nextBoard.Name);
                    }
                    return boards;
                }
            }
        }
        private Board getBoard(string boardName, string emailOfCreator)
        {
            foreach (Board board in boardsCreated[emailOfCreator])
            {
                if (board.Name.Equals(boardName))
                {
                    return board;
                }
            }
            throw new Exception("cant find the board in loadData");
        }
        /// <summary>
        /// Removes all persistent data
        /// </summary>
        public void DeleteData()
        {
            columnDalCon.Destroy();
            taskDalCon.Destroy();
            boardDalCon.Destroy();
            foreach (var pair in boardsCreated)
            {
                foreach (Board board in pair.Value)
                {
                    board.BoardDTO.IsPersistent = false;
                }
            }
        }

        /// <summary>
        /// Load all the data from the database
        /// </summary>
        public void LoadData()
        {
            LoadBoardsData();
            if (boardsCreated.Count != 0)
            {
                boardIdCounter = boardDalCon.maxId() + 1;
                columnIdCounter = columnDalCon.maxId() + 1;
            }
            else
            {
                boardIdCounter = 0;
                columnIdCounter = 0;
            }
            if (assignedtasks.Count != 0)
            {
                taskIdCounter = taskDalCon.maxId() + 1;
            }
            else
            {
                taskIdCounter = 0;
            }

        }
        /// <summary>
        /// Load all boards created, and all board members
        /// </summary>
        public void LoadBoardsData()
        {
            try
            {
                Dictionary<string, List<BoardDTO>> boards = boardDalCon.LoadBoards();
                BoardsMember = new Dictionary<string, List<Board>>();
                BoardsCreated = new Dictionary<string, List<Board>>();
                Assignedtasks = new Dictionary<string, List<Task>>();
                foreach (var pair in boards)
                {

                    foreach (BoardDTO board in pair.Value)
                    {
                        Board boardB = new Board(board.Name, board.CreatorEmail, board.MembersEmails.Split(',').ToList(), board, columnDalCon, taskDalCon);
                        if (BoardsCreated.ContainsKey(pair.Key))
                        {
                            BoardsCreated[pair.Key].Add(boardB);
                        }
                        else
                        {
                            List<Board> listB = new List<Board>();
                            listB.Add(boardB);
                            BoardsCreated.Add(pair.Key, listB);
                        }
                        foreach (string memberEmail in boardB.Members)
                        {
                            if (BoardsMember.ContainsKey(memberEmail))
                            {
                                BoardsMember[memberEmail].Add(boardB);
                            }
                            else
                            {
                                List<Board> list = new List<Board>();
                                list.Add(boardB);
                                BoardsMember.Add(memberEmail, list);
                            }
                        }
                        foreach (Column col in boardB.Columns)
                        {
                            foreach (Task task in col.Tasks)
                            {
                                if (Assignedtasks.ContainsKey(task.EmailAssignee))
                                {
                                    Assignedtasks[task.EmailAssignee].Add(task);
                                    Console.WriteLine("---------------check 1-------------- -");
                                    Console.WriteLine("task title -  " + task.Title + "  //task id - " + task.Id);
                                }
                                else
                                {
                                    List<Task> list = new List<Task>();
                                    list.Add(task);
                                    Assignedtasks.Add(task.EmailAssignee, list);
                                    Console.WriteLine("---------------check 2 ---------------");
                                    Console.WriteLine("task title -  " + task.Title + "  //task id - " + task.Id);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Adds a new column
        /// </summary>
        /// <param name="userEmail">Email of the current user. Must be logged in</param>
        /// <param name="creatorEmail">Email of the board creator</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The location of the new column. Location for old columns with index>=columnOrdinal is increased by 1 (moved right). The first column is identified by 0, the location increases by 1 for each column.</param>
        /// <param name="columnName">The name for the new columns</param>        
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public void AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            if (UserCon.GetUser(userEmail).logedin)
            {
                Board board = GetBoard(creatorEmail, boardName);
                if (board.IsMember(userEmail))
                {
                    if ((columnOrdinal <= board.Columns.Count) & (columnOrdinal >= 0))
                    {
                        Column newColumn = new Column(columnName, boardName, creatorEmail, columnOrdinal, columnIdCounter, columnDalCon);
                        board.AddColumn(columnOrdinal, newColumn);
                        columnIdCounter++;
                    }
                    else
                    {
                        throw new Exception("This column ordinal is illegal");
                    }
                }
                else
                {
                    throw new Exception("You are not a board member");
                }
            }
            else
            {
                throw new Exception("To get the boards names you need to be logged in");
            }
        }

        public void RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (UserCon.GetUser(userEmail).logedin)
            {

                Board board = GetBoard(creatorEmail, boardName);
                if (board.IsMember(userEmail))
                {
                    Column colTodelete = board.GetColumn(columnOrdinal);

                    board.RemoveColumn(columnOrdinal);
                    columnDalCon.Delete(colTodelete.ColumnDTO);
                }
                else
                {
                    throw new Exception("You are not a board member");
                }
            }
            else
            {
                throw new Exception("To get the boards names you need to be logged in");
            }
        }

        public void RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            if (UserCon.GetUser(userEmail).logedin)
            {
                Board board = GetBoard(creatorEmail, boardName);
                if (board.IsMember(userEmail))
                {
                    board.RenameColumn(columnOrdinal, newColumnName);
                }
                else
                {
                    throw new Exception("You are not a board member");
                }
            }
            else
            {
                throw new Exception("To get the boards names you need to be logged in");
            }
        }

        public void MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            if (UserCon.GetUser(userEmail).logedin)
            {
                Board board = GetBoard(creatorEmail, boardName);
                if (board.IsMember(userEmail))
                {
                    board.MoveColumn(columnOrdinal, shiftSize);
                }
                else
                {
                    throw new Exception("You are not a board member");
                }
            }
            else
            {
                throw new Exception("To get the boards names you need to be logged in");
            }
        }
    }
}


