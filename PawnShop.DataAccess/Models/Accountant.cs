#nullable disable

namespace PawnShop.DataAccess.Models
{
    public partial class Accountant
    {
        public int AccountantId { get; set; }

        public virtual Person AccountantNavigation { get; set; }
    }
}