using PawnShop.Core.Enums;
using Prism.Commands;

namespace PawnShop.Modules.Commodity.Models
{
    public class ShowDialogPayload
    {
        public PreviewPutOnSaleDialogMode PreviewPutOnSaleDialogMode { get; set; }
        public DelegateCommand ActionCommand { get; set; }
    }
}