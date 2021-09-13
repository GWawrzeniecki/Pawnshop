using Microsoft.Data.SqlClient;
using PawnShop.Business.Models;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using System;
using System.Threading.Tasks;

namespace PawnShop.Services.Implementations
{
    public class LendingRateService : ILendingRateService
    {
        private readonly IContainerProvider _containerProvider;

        public LendingRateService(IContainerProvider containerProvider)
        {
            _containerProvider = containerProvider;
        }

        public async Task AddLendingRate(int days, int percentage)
        {
            try
            {
                await TryToAddLendingRate(days, percentage);
            }
            catch (Exception e)
            {
                throw new AddingLendingRateException(
                    "Wystąpił błąd podczas dodawania oprocentowania.", e);
            }
        }



        public async Task EditLendingRate(LendingRate lendingRate)
        {
            try
            {
                await TryToEditLendingRate(lendingRate);
            }
            catch (Exception e)
            {
                throw new EditingLendingRateException(
                    "Wystąpił błąd podczas edytowania oprocentowania.", e);
            }
        }

        public async Task DeleteLendingRate(LendingRate lendingRate)
        {
            try
            {
                await TryToDeleteLendingRate(lendingRate);
            }
            catch (Exception e) when (e.InnerException is SqlException { Number: 547 })
            {
                throw new DeletingLendingRateException(
                    $"Wystąpił błąd podczas usuwania oprocentowania.{Environment.NewLine}Te oprocentowanie jest używane w bieżących umowach.", e);
            }
            catch (Exception e)
            {
                throw new DeletingLendingRateException(
                    "Wystąpił błąd podczas usuwania oprocentowania.", e);
            }
        }

        private async Task TryToAddLendingRate(int days, int percentage)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            await unitOfWork.LendingRateRepository.InsertAsync(new LendingRate { Days = days, Procent = percentage });
            await unitOfWork.SaveChangesAsync();
        }

        private async Task TryToEditLendingRate(LendingRate lendingRate)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            unitOfWork.LendingRateRepository.Update(lendingRate);
            await unitOfWork.SaveChangesAsync();
        }

        private async Task TryToDeleteLendingRate(LendingRate lendingRate)
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            unitOfWork.LendingRateRepository.Delete(lendingRate);
            await unitOfWork.SaveChangesAsync();
        }
    }
}