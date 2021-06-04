using PawnShop.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawnShop.Services.Interfaces
{
    public interface IContractItemService
    {
        public Task<IList<ContractItemCategory>> GetContractItemCategories();
        public Task<IList<UnitMeasure>> GetUnitMeasures();
        public Task<IList<ContractItemState>> GetContractItemStates();
    }
}