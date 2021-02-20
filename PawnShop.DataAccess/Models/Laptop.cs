#nullable disable

namespace PawnShop.DataAccess.Models
{
    public partial class Laptop
    {
        public int ContractItemId { get; set; }
        public string Brand { get; set; }
        public string Procesor { get; set; }
        public string Ram { get; set; }
        public string MassStorage { get; set; }
        public int DriveType { get; set; }
        public string DescriptionKit { get; set; }

        public virtual ContractItem ContractItem { get; set; }
    }
}