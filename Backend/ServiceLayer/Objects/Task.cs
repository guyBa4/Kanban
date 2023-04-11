using System;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Task
    {
        public readonly int Id;
        public readonly DateTime CreationTime;
        public readonly string Title;
        public readonly string Description;
        public readonly DateTime DueDate;
        public readonly string emailAssignee;
        public readonly string BoardEmailCreator;
        public readonly string BoardName;
        public readonly int ColumnOrdinal;

        internal Task(int id, DateTime creationTime, string title, string description, DateTime DueDate, string emailAssignee)
        {
            this.Id = id;
            this.CreationTime = creationTime;
            this.Title = title;
            this.Description = description;
            this.DueDate = DueDate;
            this.emailAssignee = emailAssignee;
        }

        internal Task(int id, DateTime creationTime, string title, string description, DateTime DueDate, string emailAssignee, string boardName, string boardCreator, int columnOrdinal)
        {
            this.Id = id;
            this.CreationTime = creationTime;
            this.Title = title;
            this.Description = description;
            this.DueDate = DueDate;
            this.emailAssignee = emailAssignee;
            this.BoardName = boardName;
            this.BoardEmailCreator = boardCreator;
            this.ColumnOrdinal = columnOrdinal;
        }
    }
}

