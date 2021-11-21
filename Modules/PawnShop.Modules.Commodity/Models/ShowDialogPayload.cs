using PawnShop.Core.Enums;
using Prism.Commands;

namespace PawnShop.Modules.Commodity.Models
{
    public class ShowDialogPayload
    {
        public DialogMode DialogMode { get; set; }
        public DelegateCommand ActionCommand { get; set; }
    }
}