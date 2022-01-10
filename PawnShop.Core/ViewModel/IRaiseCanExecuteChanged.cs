using Prism.Commands;
using System.Collections.Generic;

namespace PawnShop.Core.ViewModel
{
    public interface IRaiseCanExecuteChanged
    {
        public IList<DelegateCommand> Commands { get; set; }

        public void RaiseAllCommands();
    }
}
