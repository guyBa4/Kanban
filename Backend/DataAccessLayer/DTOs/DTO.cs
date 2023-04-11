using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal abstract class DTO
    {
        public const string IDColumnName = "ID";
        protected DalController _controller;
        public long Id { get; set; } = -1;
        protected bool isPersistent;
        internal bool IsPersistent
        {
            get { return isPersistent; }
            set { isPersistent = value; }
        }
        protected DTO(DalController controller)
        {
            _controller = controller;
            this.isPersistent = false;
        }

    }
}