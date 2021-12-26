using FluentAssertions;
using Moq;
using PawnShop.Business.Models;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PawnShop.Modules.Contract.UnitTests.ViewModels
{
    public class CreateContractContractDataViewModelsTests
    {
        [StaTheory]
        [InlineData(7)]
        [InlineData(14)]
        [InlineData(30)]
        public void RepurchaseDateShouldBeValidOnSelectedLendingRateChange(int days)
        {

            //Arrange
            var contractServiceMock = new Mock<IContractService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var calculateServiceMock = new Mock<ICalculateService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxService = new Mock<IMessageBoxService>();
            contractServiceMock.Setup(s => s.LoadLendingRates()).ReturnsAsync(new List<LendingRate>()
               {
                    new LendingRate() { Days = 7, Procent = 7 },
                    new LendingRate() { Days = 14, Procent = 16 },
                    new LendingRate() { Days = 30, Procent = 21 }

               });

            containerProviderMock.Setup(c => c.Resolve(typeof(CreateContractSummaryHamburgerMenuItem)))
                .Returns(new CreateContractSummaryHamburgerMenuItem());

            var vm = new CreateContractContractDataViewModel(contractServiceMock.Object, dialogServiceMock.Object,
                calculateServiceMock.Object, containerProviderMock.Object, messageBoxService.Object);

            //Act
            vm.SelectedLendingRate = (vm.LendingRates.Result).First(l => l.Days == days);

            //Assert
            Assert.Equal(DateTime.Today.AddDays(days), vm.RepurchaseDate);
        }

        [StaFact]
        public void RepurchasePriceShouldBeRaisedOnBoughtContractItemsAdd()
        {
            //Arrange
            var contractServiceMock = new Mock<IContractService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var calculateServiceMock = new Mock<ICalculateService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxService = new Mock<IMessageBoxService>();
            int invocation = 0;

            calculateServiceMock.Setup(cs => cs.CalculateContractAmount(It.IsAny<decimal>(), It.IsAny<LendingRate>()))
                .Returns(10);

            contractServiceMock.Setup(s => s.LoadLendingRates()).ReturnsAsync(new List<LendingRate>()
            {
                new LendingRate() { Days = 7, Procent = 7 },
                new LendingRate() { Days = 14, Procent = 16 },
                new LendingRate() { Days = 30, Procent = 21 }

            });

            containerProviderMock.Setup(c => c.Resolve(typeof(CreateContractSummaryHamburgerMenuItem)))
                .Returns(new CreateContractSummaryHamburgerMenuItem());

            var vm = new CreateContractContractDataViewModel(contractServiceMock.Object, dialogServiceMock.Object,
                calculateServiceMock.Object, containerProviderMock.Object, messageBoxService.Object);

            vm.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName is nameof(vm.RePurchasePrice))
                    invocation++;
            };

            dialogServiceMock.Setup(s => s.ShowDialog(It.IsNotNull<string>(), It.IsNotNull<IDialogParameters>(),
                    It.IsNotNull<Action<IDialogResult>>()))
                .Callback((string p, IDialogParameters d, Action<DialogResult> action) => action.Invoke(
                    new DialogResult(ButtonResult.OK,
                        new DialogParameters
                        {
                            { "contractItem", new ContractItem() }
                        })));

            //Act
            vm.AddContractItemCommand.Execute();

            //Assert
            invocation.Should().Be(1);
        }

        [StaFact]
        public void RepurchasePriceShouldBeRaisedOnSelectingLendingRate()
        {
            //Arrange
            var contractServiceMock = new Mock<IContractService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var calculateServiceMock = new Mock<ICalculateService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxService = new Mock<IMessageBoxService>();
            int invocation = 0;

            calculateServiceMock.Setup(cs => cs.CalculateContractAmount(It.IsAny<decimal>(), It.IsAny<LendingRate>()))
                .Returns(10);

            contractServiceMock.Setup(s => s.LoadLendingRates()).ReturnsAsync(new List<LendingRate>()
            {
                new LendingRate() { Days = 7, Procent = 7 },
                new LendingRate() { Days = 14, Procent = 16 },
                new LendingRate() { Days = 30, Procent = 21 }

            });

            containerProviderMock.Setup(c => c.Resolve(typeof(CreateContractSummaryHamburgerMenuItem)))
                .Returns(new CreateContractSummaryHamburgerMenuItem());

            var vm = new CreateContractContractDataViewModel(contractServiceMock.Object, dialogServiceMock.Object,
                calculateServiceMock.Object, containerProviderMock.Object, messageBoxService.Object);

            vm.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName is nameof(vm.RePurchasePrice))
                    invocation++;
            };

            dialogServiceMock.Setup(s => s.ShowDialog(It.IsNotNull<string>(), It.IsNotNull<IDialogParameters>(),
                    It.IsNotNull<Action<IDialogResult>>()))
                .Callback((string p, IDialogParameters d, Action<DialogResult> action) => action.Invoke(
                    new DialogResult(ButtonResult.OK,
                        new DialogParameters
                        {
                            { "contractItem", new ContractItem() }
                        })));

            //Act
            vm.SelectedLendingRate = vm.LendingRates.Result.First();

            //Assert
            invocation.Should().Be(1);
        }

        [StaFact]
        public void CalculateServiceShouldBeUseInRepurchasePriceProperty()
        {
            //Arrange
            var contractServiceMock = new Mock<IContractService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var calculateServiceMock = new Mock<ICalculateService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxService = new Mock<IMessageBoxService>();

            contractServiceMock.Setup(s => s.LoadLendingRates()).ReturnsAsync(new List<LendingRate>()
            {
                new LendingRate() { Days = 7, Procent = 7 },
                new LendingRate() { Days = 14, Procent = 16 },
                new LendingRate() { Days = 30, Procent = 21 }

            });

            containerProviderMock.Setup(c => c.Resolve(typeof(CreateContractSummaryHamburgerMenuItem)))
                .Returns(new CreateContractSummaryHamburgerMenuItem());

            var vm = new CreateContractContractDataViewModel(contractServiceMock.Object, dialogServiceMock.Object,
                calculateServiceMock.Object, containerProviderMock.Object, messageBoxService.Object);

            dialogServiceMock.Setup(s => s.ShowDialog(It.IsNotNull<string>(), It.IsNotNull<IDialogParameters>(),
                    It.IsNotNull<Action<IDialogResult>>()))
                .Callback((string p, IDialogParameters d, Action<DialogResult> action) => action.Invoke(
                    new DialogResult(ButtonResult.OK,
                        new DialogParameters
                        {
                            { "contractItem", new ContractItem() }
                        })));

            //Act
            vm.SelectedLendingRate = null;
            decimal k = vm.RePurchasePrice;

            //Assert
            calculateServiceMock.Verify(c => c.CalculateContractAmount(It.IsAny<decimal>(), It.IsAny<LendingRate>()), Times.Never);

            //Act
            vm.SelectedLendingRate = vm.LendingRates.Result.First();
            k = vm.RePurchasePrice;

            //Assert
            calculateServiceMock.Verify(c => c.CalculateContractAmount(It.IsAny<decimal>(), It.IsAny<LendingRate>()), Times.Once);

        }

        public static IEnumerable<object[]> GetLendingRateAndContractItemGenerator()
        {
            return new List<object[]>
            {
                new object[] { null, null, false },
                new object[] { new LendingRate(), null, false },
                new object[] { null, new ContractItem(), false },
                new object[] { new LendingRate(), new ContractItem(), true },
            };


        }

        [StaTheory]
        [MemberData(nameof(GetLendingRateAndContractItemGenerator))]
        public void IsNextButtonEnabledShouldBeTrueWhenLendingRateAndBoughtItemsAreNotEmpty(LendingRate lendingRate, ContractItem contractItem, bool expected)
        {
            //Arrange
            var contractServiceMock = new Mock<IContractService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var calculateServiceMock = new Mock<ICalculateService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var messageBoxService = new Mock<IMessageBoxService>();

            contractServiceMock.Setup(s => s.LoadLendingRates()).ReturnsAsync(new List<LendingRate>()
            {
                new LendingRate() { Days = 7, Procent = 7 },
                new LendingRate() { Days = 14, Procent = 16 },
                new LendingRate() { Days = 30, Procent = 21 }

            });

            containerProviderMock.Setup(c => c.Resolve(typeof(CreateContractSummaryHamburgerMenuItem)))
                .Returns(new CreateContractSummaryHamburgerMenuItem());

            var vm = new CreateContractContractDataViewModel(contractServiceMock.Object, dialogServiceMock.Object,
                calculateServiceMock.Object, containerProviderMock.Object, messageBoxService.Object);

            //Act
            vm.SelectedLendingRate = lendingRate;
            if (contractItem is not null)
                vm.BoughtContractItems.Add(contractItem);

            //Assert
            Assert.Equal(expected, vm.IsNextButtonEnabled);
        }
    }
}