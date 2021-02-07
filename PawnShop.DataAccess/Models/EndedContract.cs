#nullable disable

namespace PawnShop.EF.Models
{
    public partial class EndedContract
    {
        public string EndedContractId { get; set; }

        public virtual Contract EndedContractNavigation { get; set; }
    }
}