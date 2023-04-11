using Presentation;

namespace Presentation.Model
{
    internal abstract class NotifiableModel : Notifiable
    {
        public BackendController Controller { get; private set; }
        protected NotifiableModel(BackendController controller)
        {
            this.Controller = controller;
        }
    }
}
