using IntegrationTests.Base;
using Microsoft.EntityFrameworkCore;
using PawnShop.Business.Models;
using PawnShop.Core.Extensions;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Services.Implementations;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PawnShop.Modules.Contract.IntegrationTests.ViewModels
{
    [Collection("Sequential")]
    public class CreateContractSummaryViewModelTests : IntegrationTestBase<CreateContractSummaryViewModel>
    {
        [StaFact]
        [Isolated]
        public void CreateContractCommandShouldAddValidContractToDb()
        {
            //Arrange
            using var pawnShopContext = PawnshopContext;
            var contractAmount = 1000;
            var moneyBalanceAmount = 1200;
            var contractStartDate = DateTime.Today;
            var country = new Business.Models.Country() { Country1 = "Test" };
            var contractState = pawnShopContext.ContractStates.Add(new ContractState()
            {
                State = Core.Constants.Constants.CreatedContractState
            });
            var paymentType = pawnShopContext.PaymentTypes.Add(new PaymentType()
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
            pawnShopContext.SaveChanges();

            var contract = new Business.Models.Contract()
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
                        Amount = contractAmount,
                        PaymentType = paymentType.Entity
                    }
                }
            };
            ViewModel.Contract = contract;

            //Act
            Nito.AsyncEx.AsyncContext.Run(() =>
            {
                ViewModel.CreateContractCommand.Execute();
            });

            //Assert
            var insertedContract = pawnShopContext.Contracts
                .AsNoTracking()
                .Include(c => c.ContractState)
                .Include(c => c.LendingRate)
                .Include(c => c.CreateContractDealDocument)
                .ThenInclude(d => d.Payment)
                .ThenInclude(p => p.PaymentType)
                .Include(c => c.CreateContractDealDocument)
                .ThenInclude(d => d.MoneyBalance)
                .First(c => c.ContractNumberId.Equals(contract.ContractNumberId));
            Assert.NotNull(insertedContract);
            Assert.Equal(contractAmount, insertedContract.AmountContract);
            Assert.Equal(contractStartDate, insertedContract.StartDate);
            Assert.Equal(lendingRate.Entity.Id, insertedContract.LendingRate.Id);
            Assert.Equal(dealMaker.Entity.ClientId, insertedContract.DealMakerId);
            Assert.Equal(workerBoss.Entity.WorkerBossId, insertedContract.WorkerBossId);
            Assert.Equal(Core.Constants.Constants.CreatedContractState, insertedContract.ContractState.State);
            Assert.Equal(DateTime.Today, insertedContract.CreateContractDealDocument.MoneyBalanceId);
            Assert.Equal(contractAmount, insertedContract.CreateContractDealDocument.Cost);
            Assert.Null(insertedContract.CreateContractDealDocument.Income);
            Assert.Null(insertedContract.CreateContractDealDocument.RepaymentCapital);
            Assert.Null(insertedContract.CreateContractDealDocument.Profit);
            Assert.Equal(Core.Constants.Constants.CashPaymentType, insertedContract.CreateContractDealDocument.Payment.PaymentType.Type);
            Assert.Equal(contractAmount, insertedContract.CreateContractDealDocument.Payment.Amount);
            Assert.Null(insertedContract.CreateContractDealDocument.Payment.ClientId);
            Assert.Equal(moneyBalanceAmount - contractAmount, insertedContract.CreateContractDealDocument.MoneyBalance.MoneyBalance1);

        }
    }
}