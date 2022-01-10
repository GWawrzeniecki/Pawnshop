using FluentAssertions;
using Moq;
using PawnShop.Business.Models;
using PawnShop.Core.SharedVariables;
using PawnShop.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PawnShop.Services.UnitTests
{
    public class CalculateServiceTests
    {
        private readonly IList<LendingRate> _lendingRates;
        public CalculateServiceTests()
        {
            _lendingRates = new List<LendingRate>()
            {
                new LendingRate() { Days = 7, Procent = 7 },
                new LendingRate() { Days = 14, Procent = 16 },
                new LendingRate() { Days = 30, Procent = 21 }
            };
        }

        [Fact]

        public void CalculateContractAmountWhenLendingRateIsNullShouldThrowArgumentNullException()
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            Action action = () => calculateService.CalculateContractAmount(1, null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(23, 7, 1000, 1086)]
        [InlineData(23, 16, 1000, 1197)]
        [InlineData(23, 21, 1000, 1258)]
        public void CalculateContractAmountWhenLendingRateIsNotNullShouldReturnVatAndRoundedEstimatedValueMultiplyByPercentAndPlusEstimatedValue(int vatPercent, int lendingRatePercent, decimal estimatedValue, decimal expectedResult)
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            userSettingsMock.Setup(s => s.VatPercent).Returns(vatPercent);
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            var result =
                calculateService.CalculateContractAmount(estimatedValue, new LendingRate() { Procent = lendingRatePercent });
            //Assert
            result.Should().Be(expectedResult);
        }

        [Fact]

        public void CalculateNetStorageCostWhenLendingRateIsNullShouldThrowArgumentNullException()
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            Action action = () => calculateService.CalculateNetStorageCost(1, null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(7, 1000, 70)]
        [InlineData(16, 1000, 160)]
        [InlineData(21, 1000, 210)]
        public void GetNetStorageCostWhenLendingRateIsNotNullShouldReturnNetAndRoundedEstimatedValueMultiplyByPercentAnd(int lendingRatePercent, decimal estimatedValue, decimal expectedResult)
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            var result =
                calculateService.CalculateNetStorageCost(estimatedValue, new LendingRate() { Procent = lendingRatePercent });
            //Assert
            result.Should().Be(expectedResult);
        }

        [Fact]

        public void CalculateNetRenewCostWhenLendingRateIsNullShouldThrowArgumentNullException()
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            Action action = () => calculateService.CalculateNetRenewCost(1, null, null, Enumerable.Empty<LendingRate>().ToList());

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]

        public void CalculateNetRenewCostWhenLendingRatesIsNullShouldThrowArgumentNullException()
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            Action action = () => calculateService.CalculateNetRenewCost(1, new LendingRate(), null, null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]

        public void CalculateNetRenewCostWhenLendingRatesIsEmptyShouldThrowArgumentNullException()
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            Action action = () => calculateService.CalculateNetRenewCost(1, new LendingRate(), null, Enumerable.Empty<LendingRate>().ToList());

            //Assert
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Theory]
        [InlineData(23, 1000, 21, 0, 210)]
        [InlineData(23, 1000, 21, null, 210)]
        [InlineData(23, 1000, 21, 30, 254.1)]
        [InlineData(23, 1000, 16, 0, 160)]
        [InlineData(23, 1000, 16, null, 160)]
        [InlineData(23, 1000, 16, 14, 185.6)]
        [InlineData(23, 1000, 7, 0, 70)]
        [InlineData(23, 1000, 7, null, 70)]
        [InlineData(23, 1000, 7, 7, 74.9)]
        [InlineData(23, 1000, 7, 10, 81.2)]
        [InlineData(23, 1000, 7, 100, 143.8507532)]
        [InlineData(23, 1000, 7, 91, 132.6899189)]
        [InlineData(23, 1000, 7, 80, 124.00927)]
        [InlineData(23, 1000, 7, 40, 98.252)]
        [InlineData(23, 1000, 7, 37, 90.629)]
        [InlineData(23, 1000, 16, 10, 185.6)]
        [InlineData(23, 1000, 16, 100, 328.8017216)]
        [InlineData(23, 1000, 16, 91, 303.2912432)]
        [InlineData(23, 1000, 16, 80, 283.44976)]
        [InlineData(23, 1000, 16, 40, 224.576)]
        [InlineData(23, 1000, 16, 37, 207.152)]
        public void CalculateNetRenewCostWhenLendingRatesAreNotNullShouldReturnProportionallyNetCostToDelay(int vatPercent, decimal estimatedValue, int lendingRatePercent, int? delay, decimal expectedResult)
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            userSettingsMock.Setup(s => s.VatPercent).Returns(vatPercent);
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            var result =
                calculateService.CalculateNetRenewCost(estimatedValue, new LendingRate() { Procent = lendingRatePercent }, delay, _lendingRates);

            //Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void CalculateRenewCostWhenLendingRateIsNullShouldThrowArgumentNullException()
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            Action action = () => calculateService.CalculateRenewCost(1, null, null, Enumerable.Empty<LendingRate>().ToList());

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]

        public void CalculateRenewCostWhenLendingRatesIsNullShouldThrowArgumentNullException()
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            Action action = () => calculateService.CalculateRenewCost(1, new LendingRate(), null, null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]

        public void CalculateRenewCostWhenLendingRatesIsEmptyShouldThrowArgumentNullException()
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            Action action = () => calculateService.CalculateRenewCost(1, new LendingRate(), null, Enumerable.Empty<LendingRate>().ToList());

            //Assert
            action.Should().ThrowExactly<ArgumentException>();
        }


        [Theory]
        [InlineData(23, 1000, 21, 0, 258)]
        [InlineData(23, 1000, 21, null, 258)]
        [InlineData(23, 1000, 21, 30, 313)]
        [InlineData(23, 1000, 16, 0, 197)]
        [InlineData(23, 1000, 16, null, 197)]
        [InlineData(23, 1000, 16, 14, 228)]
        [InlineData(23, 1000, 7, 0, 86)]
        [InlineData(23, 1000, 7, null, 86)]
        [InlineData(23, 1000, 7, 7, 92)]
        [InlineData(23, 1000, 7, 10, 100)]
        [InlineData(23, 1000, 7, 100, 177)]
        [InlineData(23, 1000, 7, 91, 163)]
        [InlineData(23, 1000, 7, 80, 153)]
        [InlineData(23, 1000, 7, 40, 121)]
        [InlineData(23, 1000, 7, 37, 111)]
        [InlineData(23, 1000, 16, 10, 228)]
        [InlineData(23, 1000, 16, 100, 404)]
        [InlineData(23, 1000, 16, 91, 373)]
        [InlineData(23, 1000, 16, 80, 349)]
        [InlineData(23, 1000, 16, 40, 276)]
        [InlineData(23, 1000, 16, 37, 255)]
        public void CalculateRenewCostWhenLendingRatesAreNotNullShouldReturnProportionallyRoundedVatCostToDelay(int vatPercent, decimal estimatedValue, int lendingRatePercent, int? delay, decimal expectedResult)
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            userSettingsMock.Setup(s => s.VatPercent).Returns(vatPercent);
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            var result =
                calculateService.CalculateRenewCost(estimatedValue, new LendingRate() { Procent = lendingRatePercent }, delay, _lendingRates);

            //Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void CalculateBuyBackCostWhenLendingRateIsNullShouldThrowArgumentNullException()
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            Action action = () => calculateService.CalculateBuyBackCost(1, null, null, Enumerable.Empty<LendingRate>().ToList());

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]

        public void CalculateBuyBackCostWhenLendingRatesIsNullShouldThrowArgumentNullException()
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            Action action = () => calculateService.CalculateBuyBackCost(1, new LendingRate(), null, null);

            //Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]

        public void CalculateBuyBackCostWhenLendingRatesIsEmptyShouldThrowArgumentNullException()
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            Action action = () => calculateService.CalculateBuyBackCost(1, new LendingRate(), null, Enumerable.Empty<LendingRate>().ToList());

            //Assert
            action.Should().ThrowExactly<ArgumentException>();
        }

        [Theory]
        [InlineData(23, 1000, 21, 0, 1258)]
        [InlineData(23, 1000, 21, null, 1258)]
        [InlineData(23, 1000, 21, 30, 1313)]
        [InlineData(23, 1000, 16, 0, 1197)]
        [InlineData(23, 1000, 16, null, 1197)]
        [InlineData(23, 1000, 16, 14, 1228)]
        [InlineData(23, 1000, 7, 0, 1086)]
        [InlineData(23, 1000, 7, null, 1086)]
        [InlineData(23, 1000, 7, 7, 1092)]
        [InlineData(23, 1000, 7, 10, 1100)]
        [InlineData(23, 1000, 7, 100, 1177)]
        [InlineData(23, 1000, 7, 91, 1163)]
        [InlineData(23, 1000, 7, 80, 1153)]
        [InlineData(23, 1000, 7, 40, 1121)]
        [InlineData(23, 1000, 7, 37, 1111)]
        [InlineData(23, 1000, 16, 10, 1228)]
        [InlineData(23, 1000, 16, 100, 1404)]
        [InlineData(23, 1000, 16, 91, 1373)]
        [InlineData(23, 1000, 16, 80, 1349)]
        [InlineData(23, 1000, 16, 40, 1276)]
        [InlineData(23, 1000, 16, 37, 1255)]
        public void CalculateBuyBackCostWhenLendingRatesAreNotNullShouldReturnProportionallyRoundedVatCostToDelayPlusEstimatedValue(int vatPercent, decimal estimatedValue, int lendingRatePercent, int? delay, decimal expectedResult)
        {
            //Arrange
            var userSettingsMock = new Mock<IUserSettings>();
            userSettingsMock.Setup(s => s.VatPercent).Returns(vatPercent);
            var calculateService = new CalculateService(userSettingsMock.Object);

            //Act
            var result =
                calculateService.CalculateBuyBackCost(estimatedValue, new LendingRate() { Procent = lendingRatePercent }, delay, _lendingRates);

            //Assert
            result.Should().Be(expectedResult);
        }

    }
}