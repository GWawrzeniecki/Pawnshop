using System.Collections.Generic;

#nullable disable

namespace PawnShop.Business.Models
{
    public partial class ContractItem
    {
        public ContractItem()
        {
            Sales = new HashSet<Sale>();
        }

        public int ContractItemId { get; set; }
        public string ContractNumberId { get; set; }
        public int CategoryId { get; set; }
        public int ContractItemStateId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string TechnicalCondition { get; set; }
        public decimal EstimatedValue { get; set; }

        public virtual ContractItemCategory Category { get; set; }
        public virtual ContractItemState ContractItemState { get; set; }
        public virtual Contract ContractNumber { get; set; }
        public virtual GoldProduct GoldProduct { get; set; }
        public virtual Laptop Laptop { get; set; }
        public virtual Telephone Telephone { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
