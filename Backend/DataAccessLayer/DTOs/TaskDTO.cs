using System;
namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskDTO : DTO
    {
        public const string TaskTitleColumnName = "Title";
        public const string TaskCreationTimeColumnName = "CreationTime";
        public const string TaskDescriptionColumnName = "Description";
        public const string TaskDueDateColumnName = "DueDate";
        public const string TaskColumnOrdinalColumnName = "ColumnOrdinal";
        public const string TaskAsigneeColumnName = "Asignee";
        public const string TaskBoardNameColumnName = "BoardName";
        public const string TaskBoardCreatorColumnName = "BoardCreator";

        private string title;
        private readonly System.DateTime creationTime;
        private string description;
        private System.DateTime dueDate;
        private int columnOrdinal;
        private string asignee;
        private string boardName;
        private string boardCreator;


        internal TaskDTO(int id, string title, string description, DateTime creationTime, DateTime dueDate, string asignee, string boardName, string boardCreator) : base(new TaskDalController())
        {
            Id = id;
            this.creationTime = creationTime;
            this.columnOrdinal = 0;
            this.title = title;
            this.description = description;
            this.creationTime = creationTime;
            this.dueDate = dueDate;
            this.asignee = asignee;
            this.boardName = boardName;
            this.boardCreator = boardCreator;
        }
        internal TaskDTO(int id, string title, string description, DateTime creationTime, DateTime dueDate,int columnOrdinal, string asignee, string boardName, string boardCreator) : base(new TaskDalController())
        {
            Id = id;
            this.creationTime = creationTime;
            this.columnOrdinal = columnOrdinal;
            this.title = title;
            this.description = description;
            this.creationTime = creationTime;
            this.dueDate = dueDate;
            this.asignee = asignee;
            this.boardName = boardName;
            this.boardCreator = boardCreator;
            isPersistent = true;
        }

        
        internal string BoardName
        {
            get { return boardName; }
            set
            {
                _controller.Update(Id, TaskBoardNameColumnName, value);
                boardName = value;
            }
        }
        internal string BoardCreator
        {
            get { return boardCreator; }
            set
            {
                _controller.Update(Id, TaskBoardCreatorColumnName, value);
                boardCreator = value;
            }
        }
        internal string Title
        {
            get { return title; }
            set
            {
                _controller.Update(Id, TaskTitleColumnName, value);
                title = value;
            }
        }

        internal string Description
        {
            get { return description; }
            set
            {
                _controller.Update(Id, TaskDescriptionColumnName, value);
                description = value;
            }
        }

        internal int ColumnOrdinal
        {
            get { return columnOrdinal; }
            set
            {
                _controller.Update(Id, TaskColumnOrdinalColumnName, value);
                columnOrdinal = value;
            }
        }
        internal DateTime CreationTime
        {
            get { return creationTime; }
        }
        internal DateTime DueDate
        {
            get { return dueDate; }
            set
            {
                _controller.Update(Id, TaskDueDateColumnName, value.ToString());
                dueDate = value;
            }
        }
        internal string Asignee
        {
            get { return asignee; }
            set
            {
                _controller.Update(Id, TaskAsigneeColumnName, value);
                asignee = value;
            }
        }
    }
}