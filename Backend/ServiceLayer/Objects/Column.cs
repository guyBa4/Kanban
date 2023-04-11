using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Column
    {
        public string Name { get; }
        public Board Board { get; set; }
        public readonly string BoardName;
        public readonly IList<Task> TaskList;
        public int ColOrdinal;
        internal Column(string name, string boardName, IList<Task> tasks, int cOrdinal)
        {
            this.Name = name;
            this.BoardName = boardName;
            this.TaskList = tasks;
            this.ColOrdinal = cOrdinal;
        }
    }

}
