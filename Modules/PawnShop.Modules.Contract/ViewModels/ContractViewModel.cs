using Prism.Mvvm;
using System.Collections.Generic;
using PawnShop.Business.Models;

namespace PawnShop.Modules.Contract.ViewModels
{
    public class ContractViewModel : BindableBase
    {
        public ContractViewModel()
        {
            Contracts = new List<Business.Models.Contract>();
        }

        private List<Business.Models.Contract> _contracts;
        public List<Business.Models.Contract> Contracts
        {
            get { return _contracts; }
            set { SetProperty(ref _contracts, value); }
        }
    }
}