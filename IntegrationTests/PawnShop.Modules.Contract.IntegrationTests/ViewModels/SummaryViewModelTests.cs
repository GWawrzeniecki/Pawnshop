using IntegrationTests.Base;
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
    public class SummaryViewModelTests : IntegrationTestBase<SummaryViewModel>
    {
        [StaFact]
        [Isolated]
        public void CreateContractCommandShouldAddValidContractToDb()
        {
            //Arrange
            using var pawnShopContext = PawnshopContext;
            var country = new Business.Models.Country() { Country1 = "Test" };
            var contractState = pawnShopContext.ContractStates.Add(new ContractState()
            {
                State = "Założona"
            });
            var paymentType = pawnShopContext.PaymentTypes.Add(new PaymentType()
            {
                Type = "Gotówka"
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
            pawnShopContext.MoneyBalances.Add(new MoneyBalance() { TodayDate = DateTime.Today, MoneyBalance1 = 1000 });
            pawnShopContext.SaveChanges();

            var contract = new Business.Models.Contract()
            {
                StartDate = DateTime.Today.AddDays(-60),
                ContractNumberId = "01/2021",
                AmountContract = 1000,
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
                    EstimatedValue = 100,
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
                    MoneyBalance = new MoneyBalance()
                    {
                        MoneyBalance1 = 5000,
                        TodayDate = DateTime.Today
                    },
                    Payment = new Payment()
                    {
                        Date = DateTime.Today,
                        Amount = 2000,
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
            var contractCount =
                pawnShopContext.Contracts.Count(c => c.ContractNumberId.Equals(contract.ContractNumberId));
            Assert.Equal(1, contractCount);
        }
    }
}