using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    class ColumnDTO : DTO
    {

        public const string ColumnBoardNameColumnName = "BoardName";
        public const string ColumnEmailCreatorColumnName = "EmailCreator";
        public const string ColumnOrdinalColumnName = "ColumnOrdinal";
        public const string ColumnNameColumnName = "ColumnName";
        public const string ColumnTaskLimitColumnName = "TaskLimit";

        private int columnOrdinal;
        private string columnName;
        private readonly string boardName;
        private readonly string emailCreatorOfBoard;
        private int taskLimit = -1;

        internal ColumnDTO(int id,string boardName, string emailCreatorOfBoard, int columnOrdinal,string columnName, int limit) : base(new ColumnDalController())
        {
            Id = id;
            this.boardName = boardName;
            this.emailCreatorOfBoard = emailCreatorOfBoard;
            this.columnOrdinal = columnOrdinal;
            this.columnName = columnName;
            this.taskLimit = limit;
        }


        internal string BoardName
        {
            get { return boardName; }
        }

        internal int ColumnOrdinal
        {
            get { return columnOrdinal; }
            set
            {
                _controller.Update(Id, ColumnOrdinalColumnName, value);
                columnOrdinal = value;
            }
        }
        internal string ColumnName
        {
            get { return columnName; }
            set
            {
                _controller.Update(Id, ColumnNameColumnName, value);
                columnName = value;
            }
        }
        internal string EmailCreatorOfBoard
        {
            get { return emailCreatorOfBoard; }
        }

        internal int TaskLimit
        {
            get { return taskLimit; }
            set
            {
                _controller.Update(Id, ColumnTaskLimitColumnName, value);
                taskLimit = value;
            }
        }
        internal List<TaskDTO> loadTasks(TaskDalController taskDalCon)
        {
            return taskDalCon.LoadTasks(ColumnOrdinal,boardName,emailCreatorOfBoard);
        }
    }
}
