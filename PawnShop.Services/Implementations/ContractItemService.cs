using PawnShop.Business.Models;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Services.Implementations
{
    public class ContractItemService : IContractItemService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ContractItemService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IList<ContractItemCategory>> GetContractItemCategories()
        {
            try
            {
                return await TryToGetContractItemCategories();
            }
            catch (Exception e)
            {
                throw new LoadingContractItemCategoriesException(
                    "Wystąpił błąd podczas pobierania kategorii przedmiotow.", e);
            }
        }

        public async Task<IList<UnitMeasure>> GetUnitMeasures()
        {
            try
            {
                return await TryToGetUnitMeasures();
            }
            catch (Exception e)
            {
                throw new LoadingUnitMeasuresException(
                    "Wystąpił błąd podczas pobierania jednostek miar.", e);
            }
        }

        public async Task<IList<ContractItemState>> GetContractItemStates()
        {
            try
            {
                return await TryToGetContractItemStates();
            }
            catch (Exception e)
            {
                throw new LoadingContractItemStatesException(
                    "Wystapił błąd podczas pobierania rodzajów stanu przedmiotu w kontrakcie.", e);
            }
        }



        private async Task<IList<ContractItemCategory>> TryToGetContractItemCategories()
        {
            return (await _unitOfWork.ContractItemCategoryRepository.GetAsync()).ToList();
        }

        private async Task<IList<UnitMeasure>> TryToGetUnitMeasures()
        {
            return (await _unitOfWork.UnitMeasureRepository.GetAsync()).ToList();
        }

        private async Task<List<ContractItemState>> TryToGetContractItemStates()
        {
            return (await _unitOfWork.ContractItemStateRepository.GetAsync()).ToList();
        }
    }
}