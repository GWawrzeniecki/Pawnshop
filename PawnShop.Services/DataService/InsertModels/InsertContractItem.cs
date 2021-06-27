using PawnShop.Business.Models;

namespace PawnShop.Services.DataService.InsertModels
{
    public class InsertContractItem
    {
        public int ContractItemId { get; set; }
        public string ContractNumberId { get; set; }
        public int CategoryId { get; set; }
        public int ContractItemStateId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public string TechnicalCondition { get; set; }
        public decimal EstimatedValue { get; set; }

        public GoldProduct GoldProduct { get; set; }
        public Laptop Laptop { get; set; }
        public Telephone Telephone { get; set; }
    }
}