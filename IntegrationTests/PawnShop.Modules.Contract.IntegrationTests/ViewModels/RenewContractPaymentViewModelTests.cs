using IntegrationTests.Base;
using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.Core.Extensions;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Services.Implementations;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PawnShop.Modules.Contract.IntegrationTests.ViewModels
{
    [Collection("Sequential")]
    public class RenewContractPaymentViewModelTests : IntegrationTestBase<RenewContractPaymentViewModel>
    {
        [Fact]
        [Isolated]
        public void RenewContractCommandShouldRenewContract()
        {
            //Arrange
            var contractAmount = 1000;
            var moneyBalanceAmount = 1200;
            decimal paymentAmount = 313;
            var contractStartDate = DateTime.Today.AddDays(-30);
            using var pawnShopContext = PawnshopContext;
            var country = new Country() { Country1 = "Test" };
            var contractState = pawnShopContext.ContractStates.Add(new ContractState()
            {
                State = Core.Constants.Constants.CreatedContractState
            });
            var renewContractState = pawnShopContext.ContractStates.Add(new ContractState()
            {
                State = Core.Constants.Constants.RenewedContractState
            });
            var cashPaymentType = pawnShopContext.PaymentTypes.Add(new PaymentType()
            {
                Type = Core.Constants.Constants.CashPaymentType
            });
            var lendingRate = pawnShopContext.LendingRates.Add(new LendingRate()
            {
                Procent = 21,
                Days = 30
            });
            var contractItemCategory = pawnShopContext.ContractItemCategories.Add(new ContractItemCategory()
            {
                Category = "Test",
                Measure = new UnitMeasure()
                {
                    Measure = "Test"
                }
            });
            var contractItemState = pawnShopContext.ContractItemStates.Add(new ContractItemState()
            {
                State = "Test"
            });
            var dealMaker = pawnShopContext.Clients.Add(new Business.Models.Client()
            {
                IdcardNumber = "Test",
                ValidityDateIdcard = DateTime.Today,
                Pesel = "Test",
                ClientNavigation = new Business.Models.Person()
                {
                    FirstName = "Adam",
                    LastName = "Nowak",
                    BirthDate = DateTime.Today,
                    Address = new Business.Models.Address
                    {
                        Street = "Test",
                        HouseNumber = "10",
                        PostCode = "11111",
                        Country = country,
                        City = new Business.Models.City { City1 = "Test", Country = country }
                    }
                }
            });
            var workerBoss = pawnShopContext.WorkerBosses.Add(new Business.Models.WorkerBoss
            {
                Pesel = "11111111111",
                Login = "test",
                WorkerBossType = new WorkerBossType()
                {
                    Type = "Pracownik"
                },
                Hash = ContainerProvider.Resolve<HashService>().Hash("test".ToSecureString()),
                Privilege = new Business.Models.Privilege { PawnShopTabs = true },
                WorkerBossNavigation = new Business.Models.Person
                {
                    FirstName = "Adam",
                    LastName = "Nowak",
                    BirthDate = DateTime.Today,
                    Address = new Business.Models.Address
                    {
                        Street = "Test",
                        HouseNumber = "10",
                        PostCode = "11111",
                        Country = country,
                        City = new Business.Models.City { City1 = "Test", Country = country }
                    }
                }
            });
            var moneyBalance = pawnShopContext.MoneyBalances.Add(new MoneyBalance() { TodayDate = DateTime.Today, MoneyBalance1 = moneyBalanceAmount });

            var contract = pawnShopContext.Contracts.Add(new Business.Models.Contract()
            {
                StartDate = contractStartDate,
                ContractNumberId = "01/2021",
                AmountContract = contractAmount,
                WorkerBoss = workerBoss.Entity,
                WorkerBossId = workerBoss.Entity.WorkerBossId,
                ContractItems = new List<ContractItem>(){new ContractItem()
                {
                    ContractNumberId = "01/2021",
                    Category = contractItemCategory.Entity,
                    CategoryId = contractItemCategory.Entity.Id,
                    ContractItemState = contractItemState.Entity,
                    ContractItemStateId = contractItemState.Entity.Id,
                    Name = "Laptop",
                    Amount = 1,
                    Description = "Test",
                    TechnicalCondition = "Test",
                    EstimatedValue = contractAmount,
                    Laptop = new Laptop()
                    {
                        Brand = "Test",
                        Procesor = "Test",
                        DescriptionKit = "Test",
                        DriveType = "Test",
                        MassStorage = "Test",
                        Ram = "Test"
                    }
                }},
                DealMaker = dealMaker.Entity,
                DealMakerId = dealMaker.Entity.ClientId,
                ContractState = contractState.Entity,
                LendingRate = lendingRate.Entity,
                LendingRateId = lendingRate.Entity.Id,
                CreateContractDealDocument = new DealDocument()
                {
                    MoneyBalance = moneyBalance.Entity,
                    Payment = new Payment()
                    {
                        Date = DateTime.Today,
                        Amount = paymentAmount,
                        PaymentType = cashPaymentType.Entity,
                        Client = dealMaker.Entity
                    }
                }
            });
            pawnShopContext.SaveChanges();

            ViewModel.SelectedPaymentType = cashPaymentType.Entity;
            var param = new NavigationParameters()
            {
                { "contract", contract.Entity },
                { "renewLendingRate", lendingRate.Entity },
                {"renewPrice",paymentAmount},
                {"startDate",contractStartDate.AddDays(lendingRate.Entity.Days+1)}
            };
            var navigationService =
                new RegionNavigationService(ContainerProvider, new RegionNavigationContentLoader(ContainerProvider), new RegionNavigationJournal())
                {
                    Region = new Region()
                };

            var navigationContext = new NavigationContext(navigationService, new Uri("Test", UriKind.RelativeOrAbsolute), param);

            ViewModel.OnNavigatedTo(navigationContext);


            //Act
            Nito.AsyncEx.AsyncContext.Run(() =>
            {
                ViewModel.RenewContractCommand.Execute();
            });

            //Assert
            var updatedContract = pawnShopContext.Contracts
                .AsNoTracking()
                .Include(c => c.ContractState)
                .First(c => c.ContractNumberId.Equals(contract.Entity.ContractNumberId));

            var renewedContract = pawnShopContext.ContractRenews
                .AsNoTracking()
                .Include(c => c.LendingRate)
                .Include(c => c.DealDocument)
                .ThenInclude(d => d.Payment)
                .ThenInclude(p => p.PaymentType)
                .Include(c => c.DealDocument)
                .ThenInclude(d => d.MoneyBalance)
                .First(c => c.ContractNumberId.Equals(contract.Entity.ContractNumberId));

            Assert.NotNull(updatedContract);
            Assert.NotNull(renewedContract);
            Assert.Equal(Core.Constants.Constants.RenewedContractState, updatedContract.ContractState.State);
            Assert.Equal(contractAmount, updatedContract.AmountContract);
            Assert.Equal(contractStartDate.AddDays(lendingRate.Entity.Days + 1), renewedContract.StartDate);
            Assert.Equal(renewedContract.LendingRateId, lendingRate.Entity.Id);
            Assert.Equal(dealMaker.Entity.ClientId, renewedContract.DealDocument.Payment.ClientId);
            Assert.Equal(paymentAmount, renewedContract.DealDocument.Payment.Amount);
            Assert.NotNull(renewedContract.DealDocument.Payment.ClientId);
            Assert.Null(renewedContract.DealDocument.Cost);
            Assert.Null(renewedContract.DealDocument.RepaymentCapital);
            Assert.Null(renewedContract.DealDocument.Profit);
            Assert.Equal(paymentAmount, renewedContract.DealDocument.Income);
            Assert.Equal(moneyBalanceAmount + paymentAmount, renewedContract.DealDocument.MoneyBalance.MoneyBalance1);
        }
    }
}