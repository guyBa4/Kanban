using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Model
{
    class MenuModel : NotifiableModel
    {
        private readonly UserModel user;
        public ObservableCollection<BoardModel> Boards { get; set; }

        //Constructor
        public MenuModel(BackendController controller, UserModel user) : base(controller)
        {
            this.user = user;
            Boards = new ObservableCollection<BoardModel>(controller.GetBoardNames(user.Email).
                Select((c, i) => new BoardModel(controller, user, c, controller.GetBoard(user.Email, user.Email, c).First.Value.Board.EmailCreator, false)).ToList());
            Boards.CollectionChanged += HandleChange;
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            // in case of removing board
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (BoardModel brd in e.OldItems)
                {
                    Controller.RemoveBoard(user.Email, brd.EmailCreator, brd.Name);
                }
            }

            // in case of adding board
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (BoardModel brd in e.NewItems)
                {
                    if ((brd.EmailCreator).Equals(this.user.Email))
                    {
                        Controller.AddBoard(user.Email, brd.Name);
                    }
                    else
                    {
                        Controller.JoinBoard(user.Email, brd.EmailCreator, brd.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Removing board from the user's the list and from database
        /// </summary>
        /// <param name="brd">board to remove</param>
        internal void RemoveBoard(BoardModel brd)
        {
            Boards.Remove(brd);
        }

        /// <summary>
        /// Adding board to the user's the list and to database
        /// </summary>
        /// <param name="brd">board to add</param>
        internal void AddBoard(BoardModel brd)
        {
            Boards.Add(brd);
        }
    }
}
