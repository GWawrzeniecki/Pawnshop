using Moq;
using PawnShop.Business.Models;
using PawnShop.Core.Tasks;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Services.Interfaces;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit;

namespace PawnShop.Modules.Contract.UnitTests.ViewModels
{
    public class BuyBackContractDataTests
    {
        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetContractWithExpectedStartDate), MemberType = typeof(ContractDataGenerator))]
        public void ContractStartDateShouldReturnContractStartDateWhenThereAreNotContractRenews(Business.Models.Contract contract, DateTime expectedContractStartDate)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractService = new Mock<IContractService>();
            var messageBoxService = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            var vm = new BuyBackContractDataViewModel(calculateServiceMock.Object, contractService.Object,
                messageBoxService.Object);
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", contract}
            }));

            //Act
            _ = vm.ContractDate;

            //Assert
            Assert.Equal(expectedContractStartDate, vm.ContractStartDate);
        }

        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetContractGeneratorWithExpectedContractDate), MemberType = typeof(ContractDataGenerator))]
        public void ContractDateShouldReturnStartDatePlusLendingRateWhenThereAreNotContractRenews(Business.Models.Contract contract, DateTime expectedContractDate)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractService = new Mock<IContractService>();
            var messageBoxService = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            var vm = new BuyBackContractDataViewModel(calculateServiceMock.Object, contractService.Object,
                messageBoxService.Object);
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", contract}
            }));

            //Act


            //Assert
            Assert.Equal(expectedContractDate, vm.ContractDate);
        }

        [Fact]
        public void ContractDateShouldRaiseBuyBackPrice()
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractService = new Mock<IContractService>();
            var messageBoxService = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            var vm = new BuyBackContractDataViewModel(calculateServiceMock.Object, contractService.Object,
                messageBoxService.Object);
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", ContractDataGenerator.GetContract()}
            }));

            var wasBuyBackPriceRaised = false;


#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            vm.PropertyChanged += delegate (object? sender, PropertyChangedEventArgs args)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            {
                wasBuyBackPriceRaised = args.PropertyName switch
                {
                    nameof(vm.BuyBackPrice) => true,
                    _ => wasBuyBackPriceRaised
                };
            };

            //Act
            _ = vm.ContractDate;

            //Assert
            Assert.True(wasBuyBackPriceRaised);
        }

        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetContractGeneratorWithExpectedLateness), MemberType = typeof(ContractDataGenerator))]
        public void HowManyDaysLateCalculatedShouldReturn0WhenThereIsNotDelay(Business.Models.Contract contract, int expectedDelay)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractService = new Mock<IContractService>();
            var messageBoxService = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            var vm = new BuyBackContractDataViewModel(calculateServiceMock.Object, contractService.Object,
                messageBoxService.Object);
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", contract}
            }));


            //Act
            _ = vm.ContractDate;

            //Assert
            Assert.Equal(expectedDelay, vm.HowManyDaysLateCalculated.Result);
        }

        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetContractGeneratorWithExpectedLateness), MemberType = typeof(ContractDataGenerator))]
        public void HowManyDaysLateCalculatedShouldSetSelectedDelayLendingRate(Business.Models.Contract contract, int expectedDelay)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractService = new Mock<IContractService>();
            var messageBoxService = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            var vm = new BuyBackContractDataViewModel(calculateServiceMock.Object, contractService.Object,
                messageBoxService.Object);
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", contract}
            }));


            //Act
            _ = vm.HowManyDaysLateCalculated.Result;

            //Assert
            Assert.NotNull(vm.SelectedDelayLendingRate);
            Assert.Equal(expectedDelay, vm.SelectedDelayLendingRate.Days);
        }

        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetContractGeneratorWithExpectedLateness), MemberType = typeof(ContractDataGenerator))]
        public void HowManyDaysLateCalculatedShouldSetHowManyDaysLate(Business.Models.Contract contract, int expectedDelay)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractService = new Mock<IContractService>();
            var messageBoxService = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            var vm = new BuyBackContractDataViewModel(calculateServiceMock.Object, contractService.Object,
                messageBoxService.Object);
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", contract}
            }));

            //Act
            _ = vm.HowManyDaysLateCalculated.Result;

            //Assert
            Assert.Equal(expectedDelay, vm.HowManyDaysLate);
        }

        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetContractGeneratorWithExpectedSumOfEstimatedValues), MemberType = typeof(ContractDataGenerator))]
        public void SumOfEstimatedValuesShouldReturnSumOfContractItemsEstimatedValues(Business.Models.Contract contract, decimal expectedValue)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractService = new Mock<IContractService>();
            var messageBoxService = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            var vm = new BuyBackContractDataViewModel(calculateServiceMock.Object, contractService.Object,
                messageBoxService.Object);
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            //Act
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", contract}
            }));

            //Assert
            Assert.Equal(expectedValue, vm.SumOfEstimatedValues);
        }

        [Fact]
        public void HowManyDaysLateShouldRaiseBuyBackPrice()
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractService = new Mock<IContractService>();
            var messageBoxService = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            var vm = new BuyBackContractDataViewModel(calculateServiceMock.Object, contractService.Object,
                messageBoxService.Object);
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", ContractDataGenerator.GetContract()}
            }));

            var wasBuyBackPriceRaised = false;


#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            vm.PropertyChanged += delegate (object? sender, PropertyChangedEventArgs args)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            {
                wasBuyBackPriceRaised = args.PropertyName switch
                {
                    nameof(vm.BuyBackPrice) => true,
                    _ => wasBuyBackPriceRaised
                };
            };

            //Act
            vm.HowManyDaysLate = 0;

            //Assert
            Assert.True(wasBuyBackPriceRaised);
        }

        [Fact]
        public void SettingSelectedDelayLendingRateShouldSetHowManyDaysLate()
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractService = new Mock<IContractService>();
            var messageBoxService = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            var vm = new BuyBackContractDataViewModel(calculateServiceMock.Object, contractService.Object,
                messageBoxService.Object);
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            //Act
            vm.SelectedDelayLendingRate = new LendingRate() { Days = 20 };

            //Assert
            Assert.Equal(20, vm.HowManyDaysLate);
        }

        [Theory]
        [MemberData(nameof(ContractDataGenerator.GetDelayAndExpectedBuyBackPrice), MemberType = typeof(ContractDataGenerator))]
        public async Task BuyBackPriceShouldChangeOnSelectingSelectedDelayLendingRateAsync(LendingRate delay, decimal expectedBuyBackPrice)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractServiceMock = new Mock<IContractService>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());

            var vm = new BuyBackContractDataViewModel(calculateServiceMock.Object, contractServiceMock.Object, messageBoxServiceMock.Object);
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", ContractDataGenerator.GetContract()}
            }));
            await vm.LendingRates.Task;
            calculateServiceMock.Setup(s => s.CalculateBuyBackCost(1000, ContractDataGenerator.MonthLendingRate, 0, vm.LendingRates.Result))
                .Returns(1258);
            calculateServiceMock.Setup(s => s.CalculateBuyBackCost(1000, ContractDataGenerator.MonthLendingRate, delay.Days, vm.LendingRates.Result))
                .Returns(expectedBuyBackPrice);

            //Act
            _ = vm.ContractDate;

            //Assert
            Assert.Equal(1258, await vm.BuyBackPrice.Task);

            //Act
            vm.SelectedDelayLendingRate = delay;

            //Assert
            Assert.Equal(expectedBuyBackPrice, await vm.BuyBackPrice.Task);
        }

        [Fact]
        public async Task BuyBackPriceShouldReturn0WhenActualLendingRateOrLendingRatesAreNull()
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var contractService = new Mock<IContractService>();
            var messageBoxService = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            var vm = new BuyBackContractDataViewModel(calculateServiceMock.Object, contractService.Object,
                messageBoxService.Object);
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            calculateServiceMock.Setup(s => s.CalculateBuyBackCost(1000, It.IsNotNull<LendingRate>(),
                    It.IsNotNull<int?>(), It.IsNotNull<IList<LendingRate>>()))
                .Returns(1258);

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"contract", ContractDataGenerator.GetContract()}
            }));
            await vm.LendingRates.Task;

            //Act
            vm.LendingRates = null;

            //Assert
            Assert.Equal(0, await vm.BuyBackPrice.Task);
            calculateServiceMock.Verify(c => c.CalculateBuyBackCost(It.IsAny<decimal>(), It.IsAny<LendingRate>(), It.IsAny<int?>(), It.IsAny<IList<LendingRate>>()), Times.Never);

            vm.LendingRates = NotifyTask.Create(async () =>
            {
                return await Task.Run(() => (IList<LendingRate>)new List<LendingRate>()
                {
                    new LendingRate(){Procent = 16, Days = 14}
                });
            });

            //Assert
            Assert.Equal(0, await vm.BuyBackPrice.Task);
            calculateServiceMock.Verify(c => c.CalculateBuyBackCost(It.IsAny<decimal>(), It.IsAny<LendingRate>(), It.IsAny<int?>(), It.IsAny<IList<LendingRate>>()), Times.Never);

            //Act
            vm.LendingRates = null;
            _ = vm.ContractDate;

            //Assert
            Assert.Equal(0, await vm.BuyBackPrice.Task);
            calculateServiceMock.Verify(c => c.CalculateBuyBackCost(It.IsAny<decimal>(), It.IsAny<LendingRate>(), It.IsAny<int?>(), It.IsAny<IList<LendingRate>>()), Times.Never);

            //Act
            vm.LendingRates = NotifyTask.Create(async () =>
            {
                return await Task.Run(() => (IList<LendingRate>)new List<LendingRate>()
                {
                    new LendingRate(){Procent = 16, Days = 14}
                });
            });

            _ = vm.ContractDate;

            Assert.Equal(1258, await vm.BuyBackPrice.Task);
            calculateServiceMock.Verify(c => c.CalculateBuyBackCost(It.IsAny<decimal>(), It.IsNotNull<LendingRate>(), It.IsNotNull<int?>(), It.IsNotNull<IList<LendingRate>>()), Times.Once);


        }

    }
}