using System;
using System.Collections;
using System.Linq;
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using System.Collections.Generic;
using Castle.Core;
using IntroSE.Kanban.Backend.DataAccessLayer.DTOs;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BackendTest")]

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class Board
    {

        // Fields
        private BoardDTO boardDTO;
        internal BoardDTO BoardDTO
        {
            get { return boardDTO; }
            set { boardDTO = value; }
        }

        private string name;
        internal string Name
        {
            get { return name; }
            set { name = value; }
        }
        // creator - email of cretor
        private string emailCreator;
        internal string EmailCreator
        {
            get { return emailCreator; }
            set { emailCreator = value; }
        }
        private List<Column> columns;
        internal List<Column> Columns
        {
            get { return columns; }
            set { columns = value; }
        }
        private List<string> members;
        internal List<string> Members
        {
            get { return members; }
            set { members = value; }
        }

        private string column0 = "backlog";
        private string column1 = "in progress";
        private string column2 = "done";
        //constructor

        internal Board(string name, string emailCreator, int boardId, BoardDalController boardDalCon, int columnIdCont, ColumnDalController colDalCon)
        {
            BoardDTO boardDal = new BoardDTO(boardId, name, emailCreator);
            boardDalCon.Insert(boardDal);
            this.name = name;
            this.emailCreator = emailCreator;
            this.members = new List<string>();
            members.Add(emailCreator);
            this.columns = new List<Column>();
            columns.Add(new Column(column0, name, emailCreator, 0, columnIdCont, colDalCon));
            columns.Add(new Column(column1, name, emailCreator, 1, columnIdCont + 1, colDalCon));
            columns.Add(new Column(column2, name, emailCreator, 2, columnIdCont + 2, colDalCon));
            boardDTO = boardDal;
        }

        internal Board(string name, string emailCreator, List<string> emails, BoardDTO boardDal, ColumnDalController colDalCon, TaskDalController taskDalCon)
        {
            this.name = name;
            this.emailCreator = emailCreator;
            this.members = emails;
            this.boardDTO = boardDal;
            this.columns = new List<Column>();
            List<ColumnDTO> colDalList = colDalCon.LoadColumns(name, emailCreator);
            foreach (ColumnDTO columnDal in colDalList)
            {
                Columns.Add(new Column(columnDal.ColumnName, columnDal.BoardName, columnDal.EmailCreatorOfBoard, columnDal.ColumnOrdinal, columnDal.TaskLimit, columnDal, taskDalCon));
                Columns.Sort();
            }

        }
        internal Board(string name, string emailCreator) //moq for tests
        {
            this.name = name;
            this.emailCreator = emailCreator;
            this.members = new List<string>();
            this.columns = new List<Column>();
            columns.Add(new Column(column0, name, emailCreator, 0));
            columns.Add(new Column(column1, name, emailCreator, 1));
            columns.Add(new Column(column2, name, emailCreator, 2));
        }

        // Methods

        /// <summary>
        /// get column by the columnOrdinal
        /// </summary>
        /// <param name="columnOrdinal"></param>
        /// <returns>Column with the given columnOrdinal</returns>
        internal Column GetColumn(int columnOrdinal)
        {
            if (columnOrdinal >= columns.Count | columnOrdinal < 0)
                throw new Exception("column does not exist");
            else
                return Columns[columnOrdinal];

        }

        /// <summary>
        /// Add task to this board
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="dueDate"></param>
        /// <param name="emailAssignee"></param>
        /// <returns>The added task</returns>
        internal Task AddTask(int id, string title, string description, DateTime dueDate, string emailAssignee)

        {
            Task task = new Task(id, title, description, dueDate, emailAssignee, Name, EmailCreator);
            columns[0].AddTask(task);
            return task;
        }

        /// <summary>
        /// get the members of this board
        /// </summary>
        /// <returns>List of members of the boards</returns>
        internal List<string> GetBoardMembers()
        {
            return members;
        }

        // <summary>
        /// Move a task to the next column
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="TaskId"></param>
        internal void AdvanceTask(string userEmail, int columnOrdinal, int TaskId)
        {
            if (TaskId < 0)
            {
                throw new Exception("The taskid isn't legal");
            }
            else
            {
                Task task = GetTask(TaskId, columnOrdinal);
                TaskDTO taskD = task.TaskDTO;
                if (task.EmailAssignee.Equals(userEmail))
                {
                    if (columnOrdinal >= Columns.Count - 1)
                    {
                        throw new Exception("the Task is already done");
                    }
                    else
                    {
                        Column colPrev = Columns[columnOrdinal];
                        Column colNext = Columns[columnOrdinal + 1];
                        colPrev.RemoveTask(task);
                        colNext.AddTask(task);
                        task.ColumnOrdinal = colNext.ColumnOrdinal;
                        taskD.ColumnOrdinal = colNext.ColumnOrdinal;
                    }
                }
                else
                {
                    throw new Exception("Task can be advenced only by its assignee");
                }
            }
        }

        /// <summary>
        /// get task by given task id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The task with the given id</returns>
        internal Task GetTask(int id, int columnOrdinal)
        {
            Column col = Columns[columnOrdinal];
            foreach (Task tsk in col.Tasks)
            {
                if (tsk.Id == id)
                {
                    return tsk;
                }
            }
            throw new Exception("the requested task does not excist on this board");
        }

        /// <summary>
        /// Add a new member to the members of the board
        /// </summary>
        /// <param name="joinUser">Email of the user we add to the members</param>
        internal void JoinBoard(string joinUser)
        {
            members.Add(joinUser);
            boardDTO.addMember(joinUser);
        }

        /// <summary>
        /// Remove a member from the members of the board
        /// </summary>
        /// <param name="toDeleteUser">User we want to delete from members</param>
        internal void removeBoard(User toDeleteUser)
        {
            if (!members.Contains(toDeleteUser.Email))
                return;
            if (EmailCreator.Equals(toDeleteUser))
                throw new Exception("cannot delete the board creator!");
            members.Remove(toDeleteUser.Email);
            string newMembers = "";
            foreach (String member in members)
                newMembers = newMembers + "," + member;
            boardDTO.MembersEmails = newMembers;

        }
        //checks if the Task is done
        /// <summary>
        /// Checks if the Task is done
        /// </summary>
        /// <returns>True if the task is done, False if not</returns>
        /// <returns>True if the task is done, False if not</returns>
        internal bool TaskIsDone(int columnOrdinal)
        {
            if (columnOrdinal == (Columns.Count - 1))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Check if a given user is a member of the board
        /// </summary>
        /// <param name="user">User to check if he is member</param>
        /// <returns>True if the user is a member of the board, False if not</returns>
        internal bool IsMember(string email)
        {
            return members.Contains(email);
        }

        internal void AddColumn(int columnOrdinal, Column newCol)
        {
            Columns.Insert(columnOrdinal, newCol);
            for (int i = Columns.Count - 1; i >= 0; i = i - 1)
            {
                Columns[i].ColumnDTO.ColumnOrdinal = i;
                Columns[i].ColumnOrdinal = i;
            }
        }

        internal void RemoveColumn(int columnOrdinal)
        {
            if (Columns.Count <= 2)
            {
                throw new Exception("minimum number of column is 2. the removal of the column can't be execute");
            }
            else
            {
                if (columnOrdinal == 0)
                {
                    foreach (Task task in Columns[0].Tasks)
                    {
                        Columns[1].Tasks.Add(task);
                    }
                }

                else
                {
                    foreach (Task task in Columns[columnOrdinal].Tasks)
                    {

                        Column columnToMove = Columns[columnOrdinal - 1];
                        columnToMove.Tasks.Add(task);
                    }
                }
                Columns.RemoveAt(columnOrdinal);
                for (int i = 0; i < Columns.Count; i = i + 1)
                {
                    Columns[i].ColumnDTO.ColumnOrdinal = i;
                    Columns[i].ColumnOrdinal = i;

                }
            }
        }
        internal void moqRemoveColumn(int columnOrdinal)
        {
            if (Columns.Count() <= 2)
            {
                throw new Exception("minimum number of column is 2. the removal of the column can't be execute");
            }
            else
            {
                if (columnOrdinal == 0)
                {
                    foreach (Task task in Columns[0].Tasks)
                    {
                        Columns[1].Tasks.Add(task);
                    }
                }

                else
                {
                    foreach (Task task in Columns[columnOrdinal].Tasks)
                    {

                        Column columnToMove = Columns[columnOrdinal - 1];
                        columnToMove.Tasks.Add(task);
                    }
                }
                Columns.RemoveAt(columnOrdinal);
                for (int i = 0; i < Columns.Count; i = i + 1)
                {
                    Columns[i].ColumnOrdinal = i;

                }
            }
        }

        public void RenameColumn(int columnOrdinal, string newColumnName)
        {
            if (columnOrdinal < 0 | columnOrdinal >= Columns.Count)
            {
                throw new Exception("this column Ordinal is illegal");
            }
            else
            {
                Column col = Columns[columnOrdinal];
                col.Name = newColumnName;
                col.ColumnDTO.ColumnName = newColumnName;
            }
        }

        public void MoveColumn(int columnOrdinal, int shiftSize)
        {
            if (shiftSize + columnOrdinal >= Columns.Count | shiftSize + columnOrdinal < 0)
            {
                throw new Exception("the shiftSize is illegal");
            }
            else
            {
                if (Columns[columnOrdinal].Tasks.Count == 0)
                {
                    Column[] ColumnsArray = Columns.ToArray();
                    Column colToShift = ColumnsArray[columnOrdinal];
                    int oldColOrdinal = columnOrdinal;
                    int newColOrdinal = columnOrdinal + shiftSize;
                    colToShift.ColumnOrdinal = newColOrdinal;
                    colToShift.ColumnDTO.ColumnOrdinal = newColOrdinal;
                    if (shiftSize < 0)
                    {
                        for (int i = oldColOrdinal; i > newColOrdinal; i = i - 1)
                        {
                            Column temp = ColumnsArray[i - 1];
                            ColumnsArray[i - 1] = ColumnsArray[i];
                            ColumnsArray[i] = temp;
                            ColumnsArray[i].ColumnOrdinal = i;
                            ColumnsArray[i].ColumnDTO.ColumnOrdinal = i;
                        }
                    }
                    else
                    {
                        for (int i = oldColOrdinal; i < newColOrdinal; i = i + 1)
                        {
                            Column temp = ColumnsArray[i + 1];
                            ColumnsArray[i + 1] = ColumnsArray[i];
                            ColumnsArray[i] = temp;
                            ColumnsArray[i].ColumnOrdinal = i;
                            ColumnsArray[i].ColumnDTO.ColumnOrdinal = i;
                        }
                    }
                    Columns = ColumnsArray.ToList();
                }
                else
                {
                    throw new Exception("a column can be moved only if it's empty!");
                }
            }
        }
    }
}

