using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Board
    {
        public readonly string Name;
        public readonly string EmailCreator;
        public LinkedList<Column> Columns;
        internal Board(string name, string emailCreator, LinkedList<Column> columns)
        {
            this.Name = name;
            this.EmailCreator = emailCreator;
            this.Columns = columns;
        }
    }

}
