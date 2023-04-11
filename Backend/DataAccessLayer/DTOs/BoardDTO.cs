
using IntroSE.Kanban.Backend.BusinessLayer;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer;





namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    class BoardDTO : DTO
    {


        public const string BoardnameColumnName = "Name";
        public const string BoardCreatorEmailColumnName = "CreatorEmail";
        public const string BoardmembersEmailsColumnName = "MembersEmail";


        internal string name;
        internal string Name
        {
            get { return name; }
            set
            {
                _controller.Update(Id, BoardnameColumnName, value);
                name = value;
            }
        }
        private string creatorEmail;
        internal string CreatorEmail { get { return creatorEmail; } }
        private string membersEmails;
        internal string MembersEmails
        {
            get { return membersEmails; }
            set
            {
                _controller.Update(Id, BoardmembersEmailsColumnName, value);
                membersEmails = value;
            }
        }


        internal BoardDTO(int id,string name, string email) : base(new BoardDalController())

        {
            Id = id;
            this.name = name;
            this.creatorEmail = email;
            this.membersEmails = email;
        }
        internal BoardDTO(int id, string name, string creator, List<string> members) : base(new BoardDalController())
        {
            Id = id;
            this.name = name;
            this.creatorEmail = creator;
            membersEmails = "";
            foreach (string email in members)
                membersEmails = membersEmails + "," + email;
            membersEmails = membersEmails.Substring(1); // delete the first "," .
            isPersistent = true;
        }

        internal void addMember(string email)
        {
            MembersEmails = membersEmails + "," + email;
        }

    }
}