#nullable disable

namespace PawnShop.DataAccess.Models
{
    public partial class EndedContract
    {
        public string EndedContractId { get; set; }

        public virtual Contract EndedContractNavigation { get; set; }
    }
}