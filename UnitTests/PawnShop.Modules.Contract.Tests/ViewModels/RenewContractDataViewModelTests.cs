using Moq;
using PawnShop.Business.Models;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace PawnShop.Modules.Contract.UnitTests.ViewModels
{
    public class RenewContractDataViewModelTests
    {
        private static LendingRate _sevenDaysLendingRate = new LendingRate() { Days = 7, Procent = 7 };
        private static LendingRate _twoWeeksLendingRate = new LendingRate() { Days = 14, Procent = 16 };
        private static LendingRate _monthLendingRate = new LendingRate() { Days = 30, Procent = 21 };

        private static Business.Models.Contract GetContract(DateTime? startDate = null)
        {
            startDate ??= DateTime.Today;

            var country = new Business.Models.Country() { Country1 = "Test" };
            var contractAmount = 1258;
            var estimatedItemValue = 1000;
            var contractState = new ContractState()
            {
                State = Core.Constants.Constants.CreatedContractState
            };
            var paymentType = new PaymentType()
            {
                Type = Core.Constants.Constants.CashPaymentType
            };
            var lendingRate = _monthLendingRate;
            var contractItemCategory = new ContractItemCategory()
            {
                Category = "Test",
                Measure = new UnitMeasure()
                {
                    Measure = "Test"
                }
            };
            var contractItemState = new ContractItemState()
            {
                State = "Test"
            };
            var dealMaker = new Business.Models.Client()
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
            };
            var workerBoss = new Business.Models.WorkerBoss
            {
                Pesel = "11111111111",
                Login = "test",
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
            };
            var moneyBalance = new MoneyBalance() { TodayDate = DateTime.Today, MoneyBalance1 = 500 };

            return new Business.Models.Contract()
            {
                StartDate = startDate.Value,
                ContractNumberId = "01/2021",
                AmountContract = contractAmount,
                WorkerBoss = workerBoss,
                WorkerBossId = workerBoss.WorkerBossId,
                ContractItems = new List<ContractItem>(){new ContractItem()
                {
                    ContractNumberId = "01/2021",
                    Category = contractItemCategory,
                    CategoryId = contractItemCategory.Id,
                    ContractItemState = contractItemState,
                    ContractItemStateId = contractItemState.Id,
                    Name = "Laptop",
                    Amount = 1,
                    Description = "Test",
                    TechnicalCondition = "Test",
                    EstimatedValue = estimatedItemValue,
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
                DealMaker = dealMaker,
                DealMakerId = dealMaker.ClientId,
                ContractState = contractState,
                LendingRate = lendingRate,
                LendingRateId = lendingRate.Id,
                CreateContractDealDocument = new DealDocument()
                {
                    MoneyBalance = moneyBalance,
                    Payment = new Payment()
                    {
                        Date = DateTime.Today,
                        Amount = estimatedItemValue,
                        PaymentType = paymentType
                    }
                }
            };
        }

        private static Business.Models.Contract GetContractWithContractRenews(params ContractRenew[] contractRenews)
        {
            var contract = GetContract();
            foreach (var contractRenew in contractRenews)
            {
                contract.ContractRenews.Add(contractRenew);
            }

            return contract;
        }

        private static Business.Models.Contract GetContractWithContractRenews(DateTime startDate, params ContractRenew[] contractRenews)
        {
            var contract = GetContract(startDate);
            foreach (var contractRenew in contractRenews)
            {
                contract.ContractRenews.Add(contractRenew);
            }

            return contract;
        }

        private static Business.Models.Contract GetContractWithContractItems(params ContractItem[] contractRenews)
        {
            var contract = GetContract();
            foreach (var contractRenew in contractRenews)
            {
                contract.ContractItems.Add(contractRenew);
            }

            return contract;
        }

        public static IEnumerable<object[]> GetContractGenerator()
        {
            return new List<object[]>
            {
                new object[] { GetContract() },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = GetContract().StartDate.AddDays(30),
                    LendingRate = new LendingRate() { Days = 30 }
                }) },
            };
        }

        public static IEnumerable<object[]> GetLendingRatesAndExpectedRePurchaseDate()
        {
            return new List<object[]>
            {
                new object[] { _monthLendingRate,DateTime.Today.AddDays(30).AddDays(_monthLendingRate.Days) },
                new object[] { _twoWeeksLendingRate,DateTime.Today.AddDays(30).AddDays(_twoWeeksLendingRate.Days) },
                new object[] { _sevenDaysLendingRate,DateTime.Today.AddDays(30).AddDays(_sevenDaysLendingRate.Days) }
            };
        }

        public static IEnumerable<object[]> GetLendingRatesAndExpectedDelay()
        {
            return new List<object[]>
            {   new object[] { null,0},
                new object[] { _monthLendingRate,_monthLendingRate.Days},
                new object[] { _twoWeeksLendingRate,_twoWeeksLendingRate.Days },
                new object[] { _sevenDaysLendingRate,_sevenDaysLendingRate.Days },
                new object[] { new LendingRate() {Days = 10},10},

            };
        }

        public static IEnumerable<object[]> GetContractGeneratorWithContractAmount()
        {
            return new List<object[]>
            {
                new object[] { GetContract(),1258},
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = GetContract().StartDate.AddDays(30),
                    LendingRate = _twoWeeksLendingRate
                }),1197 },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = GetContract().StartDate.AddDays(30),
                    LendingRate = _sevenDaysLendingRate
                }),1086 },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = GetContract().StartDate.AddDays(30),
                    LendingRate = _twoWeeksLendingRate
                }, new ContractRenew()
                {
                    RenewContractId = 2,
                    StartDate = GetContract().StartDate.AddDays(30).AddDays(14),
                    LendingRate = _sevenDaysLendingRate
                }),1086 }
            };
        }

        public static IEnumerable<object[]> GetContractGeneratorWithExpectedSumOfEstimatedValues()
        {
            return new List<object[]>
            {
                new object[] { GetContract(), 1000 },
                new object[] { GetContractWithContractItems(new ContractItem()
                {
                 EstimatedValue = 1000
                }), 2000 },
                new object[] { GetContractWithContractItems(new ContractItem()
                {
                    EstimatedValue = 1000
                }, new ContractItem()
                {
                    EstimatedValue = 3500
                } ), 5500 }

            };
        }

        public static IEnumerable<object[]> GetContractGeneratorWithExpectedContractDate()
        {
            var contract = GetContract();

            return new List<object[]>
            {
                new object[] { contract, contract.StartDate.AddDays(contract.LendingRate.Days) },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = contract.StartDate.AddDays(contract.LendingRate.Days),
                    LendingRate = new LendingRate() { Days = 30 }
                }),contract.StartDate.AddDays(contract.LendingRate.Days + 30) },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = contract.StartDate.AddDays(contract.LendingRate.Days),
                    LendingRate = new LendingRate() { Days = 30 }
                }),contract.StartDate.AddDays(contract.LendingRate.Days + 30) },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = contract.StartDate.AddDays(contract.LendingRate.Days),
                    LendingRate = new LendingRate() { Days = 30 }
                }, new ContractRenew()
                {
                    RenewContractId = 2,
                    StartDate = contract.StartDate.AddDays(contract.LendingRate.Days).AddDays(30),
                    LendingRate = new LendingRate() { Days = 14}

                }), contract.StartDate.AddDays(contract.LendingRate.Days + 30 + 14)}
            };
        }

        public static IEnumerable<object[]> GetContractGeneratorWithExpectedLateness()
        {
            var contract = GetContract();

            return new List<object[]>
            {
                new object[] { GetContract(), 0 },
                new object[] { GetContract(new DateTime(2021,11,25)), 1 },
                new object[] { GetContract(new DateTime(2021,11,24)), 2 },
                new object[] { GetContract(new DateTime(2021,11,14)), 12 },
                new object[] { GetContract(new DateTime(2021,10,14)), 43 },
                new object[] { GetContract(new DateTime(2021,7,5)), 144 },
                new object[] { GetContractWithContractRenews(new DateTime(2021,11,25), new ContractRenew()
                {
                    RenewContractId = 1,
                    LendingRate = new LendingRate() {Days = 7},
                    StartDate = new DateTime(2021,11,25).AddDays(30)
                }), 0 },
                new object[] { GetContractWithContractRenews(new DateTime(2021,10,25), new ContractRenew()
                {
                    RenewContractId = 1,
                    LendingRate = new LendingRate() {Days = 7},
                    StartDate = new DateTime(2021,10,25).AddDays(30)
                }), 25 },
                new object[] { GetContractWithContractRenews(new DateTime(2021,10,25), new ContractRenew()
                {
                    RenewContractId = 1,
                    LendingRate = new LendingRate() {Days = 30},
                    StartDate = new DateTime(2021,10,25).AddDays(30)
                }), 2 },
                new object[] { GetContractWithContractRenews(new DateTime(2021,10,25), new ContractRenew()
                {
                    RenewContractId = 1,
                    LendingRate = new LendingRate() {Days = 7},
                    StartDate = new DateTime(2021,10,25).AddDays(30)
                }, new ContractRenew()
                {
                    RenewContractId = 2,
                    LendingRate = new LendingRate() {Days = 7},
                    StartDate = new DateTime(2021,10,25).AddDays(37)
                }), 18 }
            };
        }

        [Theory]
        [MemberData(nameof(GetContractGenerator))]
        public void ContractStartDateShouldReturnStartDateFromContract(Business.Models.Contract contract)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            var vm = new RenewContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object,
                containerProviderMock.Object, messageBoxServiceMock.Object);

            //Act
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", contract}
            }));
            _ = vm.ContractDate;

            //Assert
            Assert.Equal(contract.StartDate, vm.ContractStartDate);
        }

        [Theory]
        [MemberData(nameof(GetContractGeneratorWithExpectedContractDate))]
        public void ContractDateShouldReturnDatePlusLendingRateWhenThereAreNotContractRenews(Business.Models.Contract contract, DateTime expectedDate)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            var vm = new RenewContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object,
                containerProviderMock.Object, messageBoxServiceMock.Object);

            //Act
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", contract}
            }));

            //Assert
            Assert.Equal(expectedDate, vm.ContractDate);
        }

        [Theory]
        [MemberData(nameof(GetContractGeneratorWithExpectedLateness))]
        public void HowManyDaysLateCalculatedShouldReturnLateness(Business.Models.Contract contract, int expectedLateness)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            var vm = new RenewContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object,
                containerProviderMock.Object, messageBoxServiceMock.Object);

            //Act
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", contract}
            }));

            //Assert
            Assert.Equal(expectedLateness, vm.HowManyDaysLateCalculated);
        }

        [Theory]
        [MemberData(nameof(GetContractGeneratorWithExpectedSumOfEstimatedValues))]
        public void SumOfEstimatedValuesShouldReturnSumOfContractItemsEstimatedValues(Business.Models.Contract contract, decimal expectedValue)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            var vm = new RenewContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object,
                containerProviderMock.Object, messageBoxServiceMock.Object);

            //Act
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", contract}
            }));

            //Assert
            Assert.Equal(expectedValue, vm.SumOfEstimatedValues);
        }

        [Theory]
        [MemberData(nameof(GetContractGeneratorWithContractAmount))]
        public void RePurchasePriceShouldUseValidActualLendingRate(Business.Models.Contract contract, decimal expectedValue)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            var vm = new RenewContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object,
                containerProviderMock.Object, messageBoxServiceMock.Object);
            calculateServiceMock
                .Setup(s => s.CalculateContractAmount(1000, _sevenDaysLendingRate)).Returns(1086);
            calculateServiceMock
                .Setup(s => s.CalculateContractAmount(1000, _twoWeeksLendingRate)).Returns(1197);
            calculateServiceMock
                .Setup(s => s.CalculateContractAmount(1000, _monthLendingRate)).Returns(1258);


            //Act
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", contract}
            }));

            _ = vm.ContractDate;

            //Assert
            Assert.Equal(expectedValue, vm.RePurchasePrice);
        }

        [StaTheory]
        [MemberData(nameof(GetLendingRatesAndExpectedRePurchaseDate))]
        public void SelectingNewRePurchaseLendingRateShouldSetRePurchaseDate(LendingRate lendingRate, DateTime? expectedRePurchaseDate)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            containerProviderMock.Setup(c => c.Resolve(typeof(RenewContractPaymentHamburgerMenuItem)))
                .Returns(new RenewContractPaymentHamburgerMenuItem());
            var vm = new RenewContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object,
                containerProviderMock.Object, messageBoxServiceMock.Object);
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", GetContract()}
            }));

            //Act
            _ = vm.ContractDate;
            vm.SelectedNewRepurchaseDateLendingRate = lendingRate;

            //Assert
            Assert.Equal(expectedRePurchaseDate, vm.NewRepurchaseDate);
        }

        [StaFact]
        public void SelectingNewRePurchaseLendingRateShouldRaiseAnotherProperties()
        {
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            containerProviderMock.Setup(c => c.Resolve(typeof(RenewContractPaymentHamburgerMenuItem)))
                .Returns(new RenewContractPaymentHamburgerMenuItem());
            var vm = new RenewContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object,
                containerProviderMock.Object, messageBoxServiceMock.Object);

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", GetContract()}
            }));

            var wasNewRepurchaseDateRaised = false;
            var wasNewRePurchasePrice = false;
            var wasIsNextButtonEnabled = false;

            vm.PropertyChanged += delegate (object? sender, PropertyChangedEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case nameof(vm.NewRepurchaseDate):
                        wasNewRepurchaseDateRaised = true;
                        break;
                    case nameof(vm.RePurchasePrice):
                        wasNewRePurchasePrice = true;
                        break;
                    case nameof(vm.IsNextButtonEnabled):
                        wasIsNextButtonEnabled = true;
                        break;

                }
            };

            //Act
            _ = vm.ContractDate;
            vm.SelectedNewRepurchaseDateLendingRate = new LendingRate() { Days = 7, Procent = 7 };

            //Assert
            Assert.True(wasNewRepurchaseDateRaised);
            Assert.True(wasNewRepurchaseDateRaised);
            Assert.True(wasIsNextButtonEnabled);
        }

        [Theory]
        [MemberData(nameof(GetLendingRatesAndExpectedDelay))]
        public void SelectingSelectedDelayLendingRateShouldSetHowManyDaysLate(LendingRate lendingRate, int expectedDelay)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            var vm = new RenewContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object,
                containerProviderMock.Object, messageBoxServiceMock.Object);
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", GetContract()}
            }));

            //Act
            _ = vm.ContractDate;
            vm.SelectedDelayLendingRate = lendingRate;

            //Assert
            Assert.Equal(expectedDelay, vm.HowManyDaysLate);
        }

        [Fact]
        public void HowManyDaysLateShouldRaiseRenewPrice()
        {
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            var vm = new RenewContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object,
                containerProviderMock.Object, messageBoxServiceMock.Object);

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", GetContract()}
            }));

            var wasRenewPriceRaised = false;


            vm.PropertyChanged += delegate (object? sender, PropertyChangedEventArgs args)
            {
                wasRenewPriceRaised = args.PropertyName switch
                {
                    nameof(vm.RenewPrice) => true,
                    _ => wasRenewPriceRaised
                };
            };

            //Act
            vm.SelectedDelayLendingRate = new LendingRate() { Days = 7, Procent = 7 };

            //Assert
            Assert.True(wasRenewPriceRaised);
        }


    }
}