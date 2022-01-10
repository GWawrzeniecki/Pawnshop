using Moq;
using PawnShop.Business.Models;
using PawnShop.Core.Tasks;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit;

namespace PawnShop.Modules.Contract.UnitTests.ViewModels
{
    public class RenewContractDataViewModelTests
    {
        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetContractWithExpectedStartDate), MemberType = typeof(ContractDataGenerator))]
        public void ContractStartDateShouldReturnStartDateFromContract(Business.Models.Contract contract, DateTime expectedStartDate)
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
            Assert.Equal(expectedStartDate, vm.ContractStartDate);
        }

        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetContractGeneratorWithExpectedContractDate), MemberType = typeof(ContractDataGenerator))]
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
        [MemberData(nameof(ContractDataGenerator.GetContractGeneratorWithExpectedLateness), MemberType = typeof(ContractDataGenerator))]
        public async Task HowManyDaysLateCalculatedShouldReturnLatenessAsync(Business.Models.Contract contract, int expectedLateness)
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
            Assert.Equal(expectedLateness, await vm.HowManyDaysLateCalculated.Task);
        }

        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetContractGeneratorWithExpectedSumOfEstimatedValues), MemberType = typeof(ContractDataGenerator))]
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
        [MemberData(nameof(ContractDataGenerator.GetContractGeneratorWithContractAmount), MemberType = typeof(ContractDataGenerator))]
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
                .Setup(s => s.CalculateContractAmount(1000, ContractDataGenerator.OneWeekLendingRate)).Returns(1086);
            calculateServiceMock
                .Setup(s => s.CalculateContractAmount(1000, ContractDataGenerator.TwoWeeksLendingRate)).Returns(1197);
            calculateServiceMock
                .Setup(s => s.CalculateContractAmount(1000, ContractDataGenerator.MonthLendingRate)).Returns(1258);


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
        [MemberData(nameof(ContractDataGenerator.GetLendingRatesAndExpectedRePurchaseDate), MemberType = typeof(ContractDataGenerator))]
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
                {"contract", ContractDataGenerator.GetContract()}
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
                {"contract", ContractDataGenerator.GetContract()}
            }));

            var wasNewRepurchaseDateRaised = false;
            var wasNewRePurchasePrice = false;
            var wasIsNextButtonEnabled = false;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            vm.PropertyChanged += delegate (object? sender, PropertyChangedEventArgs args)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
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
            Assert.True(wasNewRePurchasePrice);
            Assert.True(wasIsNextButtonEnabled);
        }

        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetLendingRatesAndExpectedDelay), MemberType = typeof(ContractDataGenerator))]
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
                {"contract", ContractDataGenerator.GetContract()}
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
                {"contract", ContractDataGenerator.GetContract()}
            }));

            var wasRenewPriceRaised = false;


#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            vm.PropertyChanged += delegate (object? sender, PropertyChangedEventArgs args)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
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

        [Fact]
        public async Task RenewPriceShouldReturn0WhenActualLendingRateIsNullAndLendingRatesAsync()
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            calculateServiceMock.Setup(s => s.CalculateRenewCost(It.IsAny<decimal>(), It.IsNotNull<LendingRate>(),
                    It.IsAny<int?>(), It.IsNotNull<IList<LendingRate>>()))
                .Returns(500);
            var vm = new RenewContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object,
                containerProviderMock.Object, messageBoxServiceMock.Object);
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", ContractDataGenerator.GetContract()}
            }));

            //Act
            //_ = vm.ContractDate;
            vm.LendingRates = null;

            //Assert
            Assert.Equal(0, await vm.RenewPrice.Task);

            _ = vm.ContractDate;
            Assert.Equal(0, await vm.RenewPrice.Task);

            vm.LendingRates = NotifyTask.Create(async () =>
           {
               return await Task.Run(() => (IList<LendingRate>)new List<LendingRate>()
               {
                    new LendingRate(){Procent = 16, Days = 14}
               });
           });

            await vm.LendingRates.Task;

            Assert.Equal(500, await vm.RenewPrice.Task);
        }

        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetDelayAndExpectedRenewPrice), MemberType = typeof(ContractDataGenerator))]
        public async Task RenewPriceShouldChangeOnSelectingSelectedDelayLendingRate(LendingRate delay, decimal expectedRenewPrice)
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
                {"contract", ContractDataGenerator.GetContract()}
            }));
            await vm.LendingRates.Task;
            calculateServiceMock.Setup(s => s.CalculateRenewCost(1000, ContractDataGenerator.MonthLendingRate, 0, vm.LendingRates.Result))
                .Returns(258);
            calculateServiceMock.Setup(s => s.CalculateRenewCost(1000, ContractDataGenerator.MonthLendingRate, delay.Days, vm.LendingRates.Result))
                .Returns(expectedRenewPrice);

            //Act
            _ = vm.ContractDate;


            //Assert
            Assert.Equal(258, await vm.RenewPrice.Task);

            //Act
            vm.SelectedDelayLendingRate = delay;

            //Assert
            Assert.Equal(expectedRenewPrice, await vm.RenewPrice.Task);
        }

        [StaTheory]
        [MemberData(nameof(ContractDataGenerator.GetNewLendingRateAndExpectedContractAmountAndMoqTimes), MemberType = typeof(ContractDataGenerator))]
        public void NewRepurchasePriceShouldBe0WhenSelectedNewRepurchaseDateLendingRateIsNull(LendingRate selectedNewRepurchaseDateLendingRate, decimal expectedValue, Moq.Times expectedTimes)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            calculateServiceMock.Setup(s => s.CalculateContractAmount(1000, selectedNewRepurchaseDateLendingRate))
                .Returns(expectedValue);
            containerProviderMock.Setup(c => c.Resolve(typeof(RenewContractPaymentHamburgerMenuItem)))
                .Returns(new RenewContractPaymentHamburgerMenuItem());
            var vm = new RenewContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object,
                containerProviderMock.Object, messageBoxServiceMock.Object);
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", ContractDataGenerator.GetContract()}
            }));

            //Act
            _ = vm.ContractDate;
            vm.SelectedNewRepurchaseDateLendingRate = selectedNewRepurchaseDateLendingRate;

            //Assert
            Assert.Equal(expectedValue, vm.NewRePurchasePrice);
            calculateServiceMock.Verify(v => v.CalculateContractAmount(1000, selectedNewRepurchaseDateLendingRate), expectedTimes);
        }

        [StaFact]
        public void IsNextButtonEnabledShouldBeNullWhenNewRepurchaseDateIsNull()
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
            calculateServiceMock
                .Setup(s => s.CalculateContractAmount(1000, ContractDataGenerator.OneWeekLendingRate)).Returns(1086);
            calculateServiceMock
                .Setup(s => s.CalculateContractAmount(1000, ContractDataGenerator.TwoWeeksLendingRate)).Returns(1197);
            calculateServiceMock
                .Setup(s => s.CalculateContractAmount(1000, ContractDataGenerator.MonthLendingRate)).Returns(1258);

            //Act

            //Assert
            Assert.False(vm.IsNextButtonEnabled);

            //Act
            vm.SelectedNewRepurchaseDateLendingRate = new LendingRate();
            Assert.True(vm.IsNextButtonEnabled);
        }
    }
}