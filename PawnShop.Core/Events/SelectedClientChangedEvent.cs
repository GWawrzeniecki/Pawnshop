using PawnShop.Business.Models;
using Prism.Events;

namespace PawnShop.Core.Events
{
    public class SelectedClientChangedEvent : PubSubEvent<Client>
    {

    }
}