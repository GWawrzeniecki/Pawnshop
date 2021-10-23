using PawnShop.Core.Models.QueryDataModels;
using Prism.Events;

namespace PawnShop.Modules.Commodity.Events
{
    public class RefreshDataGridEvent : PubSubEvent<ContractItemQueryData>
    {

    }
}