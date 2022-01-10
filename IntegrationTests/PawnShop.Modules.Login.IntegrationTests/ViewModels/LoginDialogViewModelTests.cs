using IntegrationTests.Base;
using Microsoft.EntityFrameworkCore;
using Moq;
using PawnShop.Business.Models;
using PawnShop.Core.Extensions;
using PawnShop.Core.Interfaces;
using PawnShop.Core.SharedVariables;
using PawnShop.Modules.Login.ViewModels;
using PawnShop.Services.Implementations;
using Prism.Ioc;
using System;
using System.Linq;
using Xunit;

namespace PawnShop.Modules.Login.IntegrationTests.ViewModels
{
    public class LoginDialogViewModelTests : IntegrationTestBase<LoginDialogViewModel>
    {
        [StaTheory]
        [Isolated]
        [InlineData("test", "test")]
        public void UserShouldBeLoggedInAfterEnteringValidDataAsync(string login, string password)
        {
            //Arrange         
            using var pawnshopContext = PawnshopContext;
            var country = new Business.Models.Country() { Country1 = "Test" };
            var worker = pawnshopContext.WorkerBosses.Add(new Business.Models.WorkerBoss
            {
                Pesel = "11111111111",
                Login = login,
                WorkerBossType = new WorkerBossType()
                {
                    Type = "Pracownik"
                },
                Hash = ContainerProvider.Resolve<HashService>().Hash(password.ToSecureString()),
                Privilege = new Business.Models.Privilege { PawnShopTabs = true },
                WorkerBossNavigation = new Business.Models.Person
                {
                    FirstName = "Adam",
                    LastName = "Nowak",
                    BirthDate = DateTime.Today
                  ,
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
            pawnshopContext.SaveChanges();
            var iHavePasswordMoq = new Mock<IHavePassword>();
            iHavePasswordMoq.SetupGet(x => x.Password).Returns(password.ToSecureString());
            ViewModel.UserName = login;

            //Act            
            Nito.AsyncEx.AsyncContext.Run(() => ViewModel.LoginCommand.Execute(iHavePasswordMoq.Object));

            //Assert
            Assert.Equal(worker.Entity.WorkerBossId, ContainerProvider.Resolve<ISessionContext>().LoggedPerson.WorkerBossId);
        }


        [StaTheory]
        [Isolated]
        [InlineData("test", "test")]
        public void MoneyBalanceShouldBeCreatedAfterSuccessfullyLogin(string login, string password)
        {
            //Arrange         
            using var pawnshopContext = PawnshopContext;
            var country = new Business.Models.Country() { Country1 = "Test" };
            pawnshopContext.WorkerBosses.Add(new Business.Models.WorkerBoss
            {
                Pesel = "11111111111",
                Login = login,
                WorkerBossType = new WorkerBossType()
                {
                    Type = "Pracownik"
                },
                Hash = ContainerProvider.Resolve<HashService>().Hash(password.ToSecureString()),
                Privilege = new Business.Models.Privilege { PawnShopTabs = true },
                WorkerBossNavigation = new Business.Models.Person
                {
                    FirstName = "Adam",
                    LastName = "Nowak",
                    BirthDate = DateTime.Today
                    ,
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
            pawnshopContext.SaveChanges();
            var iHavePasswordMoq = new Mock<IHavePassword>();
            iHavePasswordMoq.SetupGet(x => x.Password).Returns(password.ToSecureString());
            ViewModel.UserName = login;
            var todayMb = pawnshopContext.MoneyBalances.FirstOrDefault(mb => DateTime.Compare(mb.TodayDate, DateTime.Today) == 0);
            if (todayMb is not null)
            {
                pawnshopContext.Remove(todayMb);
                pawnshopContext.SaveChanges();
            }

            //Act            
            Nito.AsyncEx.AsyncContext.Run(() => ViewModel.LoginCommand.Execute(iHavePasswordMoq.Object));

            //Assert
            Assert.NotNull(ContainerProvider.Resolve<ISessionContext>().TodayMoneyBalance);
            Assert.Equal(ContainerProvider.Resolve<ISessionContext>().TodayMoneyBalance.TodayDate, DateTime.Today);

        }

        [StaTheory]
        [Isolated]
        [InlineData("test", "test")]
        public void ContractStatesShouldBeUpdatedAfterSuccessfullyLogin(string login, string password)
        {
            //Arrange         
            using var pawnshopContext = PawnshopContext;
            var country = new Business.Models.Country() { Country1 = "Test" };
            pawnshopContext.ContractStates.Add(new ContractState() { State = Core.Constants.Constants.NotBoughtBackContractState });
            pawnshopContext.ContractStates.Add(new ContractState() { State = Core.Constants.Constants.RenewedContractState });
            var workerBoss = pawnshopContext.WorkerBosses.Add(new Business.Models.WorkerBoss
            {
                Pesel = "11111111111",
                Login = login,
                WorkerBossType = new WorkerBossType()
                {
                    Type = "Pracownik"
                },
                Hash = ContainerProvider.Resolve<HashService>().Hash(password.ToSecureString()),
                Privilege = new Business.Models.Privilege { PawnShopTabs = true },
                WorkerBossNavigation = new Business.Models.Person
                {
                    FirstName = "Adam",
                    LastName = "Nowak",
                    BirthDate = DateTime.Today
                     ,
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

            var contract = pawnshopContext.Contracts.Add(new Business.Models.Contract()
            {
                StartDate = DateTime.Today.AddDays(-60),
                ContractNumberId = "01/2021",
                AmountContract = 1000,
                WorkerBoss = workerBoss.Entity,
                DealMaker = new Business.Models.Client()
                {
                    IdcardNumber = "Test",
                    ValidityDateIdcard = DateTime.Today,
                    Pesel = "Test",
                    ClientNavigation = new Business.Models.Person()
                    {
                        FirstName = "Adam",
                        LastName = "Nowak",
                        BirthDate = DateTime.Today
                            ,
                        Address = new Business.Models.Address
                        {
                            Street = "Test",
                            HouseNumber = "10",
                            PostCode = "11111",
                            Country = country,
                            City = new Business.Models.City { City1 = "Test", Country = country }
                        }
                    }
                },
                ContractState = new ContractState()
                {
                    State = Core.Constants.Constants.CreatedContractState
                },
                LendingRate = new LendingRate()
                {
                    Procent = 21,
                    Days = 30
                },
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
                        PaymentType = new PaymentType()
                        {
                            Type = "Karta"
                        }
                    }
                }
            });
            pawnshopContext.SaveChanges();
            var iHavePasswordMoq = new Mock<IHavePassword>();
            iHavePasswordMoq.SetupGet(x => x.Password).Returns(password.ToSecureString());
            ViewModel.UserName = login;

            //Act            
            Nito.AsyncEx.AsyncContext.Run(() => ViewModel.LoginCommand.Execute(iHavePasswordMoq.Object));
            var contractAfter = pawnshopContext.Contracts
                .Include(c => c.ContractState)
                .AsNoTracking()
                .First(c => c.ContractNumberId.Equals(contract.Entity.ContractNumberId));

            //Assert
            Assert.NotEqual(Core.Constants.Constants.CreatedContractState, contractAfter.ContractState.State);
        }

    }
}
