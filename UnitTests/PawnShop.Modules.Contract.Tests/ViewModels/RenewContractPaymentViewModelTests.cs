using Moq;
using PawnShop.Business.Models;
using PawnShop.Core.Events;
using PawnShop.Exceptions.DBExceptions;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Modules.Contract.Windows.Views;
using PawnShop.Services.DataService.InsertModels;
using PawnShop.Services.Interfaces;
using Prism.Events;
using Prism.Regions;
using System;
using Xunit;

namespace PawnShop.Modules.Contract.UnitTests.ViewModels
{
    public class RenewContractPaymentViewModelTests
    {
        [Fact]
        public void RenewContractCommandShouldCannotBeExecutedWhenSelectedPaymentMethodsIsNull()
        {
            //Arrange
            var contractServiceMoq = new Mock<IContractService>();
            var shellServiceMoq = new Mock<IShellService>();
            var eventAggregatorMoq = new Mock<IEventAggregator>();
            var messageBoxServiceMoq = new Mock<IMessageBoxService>();

            var vm = new RenewContractPaymentViewModel(contractServiceMoq.Object, shellServiceMoq.Object,
                eventAggregatorMoq.Object, messageBoxServiceMoq.Object);

            //Act
            var canExecute = vm.RenewContractCommand.CanExecute();

            //Assert
            Assert.False(canExecute);

            //Act
            vm.SelectedPaymentType = new PaymentType();
            canExecute = vm.RenewContractCommand.CanExecute();

            //Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void DealDocumentShouldBePrintedWhenIsPrintDocumentIsTrue()
        {
            //Arrange
            var contractServiceMoq = new Mock<IContractService>();
            var shellServiceMoq = new Mock<IShellService>();
            var eventAggregatorMoq = new Mock<IEventAggregator>();
            var messageBoxServiceMoq = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            var moneyBalanceChangeEventMock = new Mock<MoneyBalanceChangedEvent>();
            eventAggregatorMoq.Setup(s => s.GetEvent<MoneyBalanceChangedEvent>())
                .Returns(moneyBalanceChangeEventMock.Object);
            var vm = new RenewContractPaymentViewModel(contractServiceMoq.Object, shellServiceMoq.Object,
                eventAggregatorMoq.Object, messageBoxServiceMoq.Object);

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"renewLendingRate", new LendingRate()},
                {"contract", new Business.Models.Contract()}
            }));

            //Act
            vm.IsPrintDealDocument = true;
            vm.SelectedPaymentType = new PaymentType();

            Nito.AsyncEx.AsyncContext.Run(() =>
            {
                vm.RenewContractCommand.Execute();
            });

            //Assert
            contractServiceMoq.Verify(c => c.PrintDealDocument(It.IsAny<Business.Models.Contract>()), Times.Once);
        }

        [Fact]
        public void MoneyBalanceChangeEventShouldBePublishedAfterRenewingContract()
        {
            //Arrange
            var contractServiceMoq = new Mock<IContractService>();
            var shellServiceMoq = new Mock<IShellService>();
            var eventAggregatorMoq = new Mock<IEventAggregator>();
            var messageBoxServiceMoq = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            var moneyBalanceChangeEventMock = new Mock<MoneyBalanceChangedEvent>();
            eventAggregatorMoq.Setup(s => s.GetEvent<MoneyBalanceChangedEvent>())
                .Returns(moneyBalanceChangeEventMock.Object);
            var vm = new RenewContractPaymentViewModel(contractServiceMoq.Object, shellServiceMoq.Object,
                eventAggregatorMoq.Object, messageBoxServiceMoq.Object);

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"renewLendingRate", new LendingRate()},
                {"contract", new Business.Models.Contract()}
            }));

            //Act
            vm.IsPrintDealDocument = true;
            vm.SelectedPaymentType = new PaymentType();

            Nito.AsyncEx.AsyncContext.Run(() =>
            {
                vm.RenewContractCommand.Execute();
            });

            //Assert
            eventAggregatorMoq.Verify(v => v.GetEvent<MoneyBalanceChangedEvent>().Publish(), Times.Once);
        }

        [Fact]
        public void RenewContractWindowShouldBeClosedAfterRenewingContract()
        {
            //Arrange
            var contractServiceMoq = new Mock<IContractService>();
            var shellServiceMoq = new Mock<IShellService>();
            var eventAggregatorMoq = new Mock<IEventAggregator>();
            var messageBoxServiceMoq = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            var moneyBalanceChangeEventMock = new Mock<MoneyBalanceChangedEvent>();
            eventAggregatorMoq.Setup(s => s.GetEvent<MoneyBalanceChangedEvent>())
                .Returns(moneyBalanceChangeEventMock.Object);
            var vm = new RenewContractPaymentViewModel(contractServiceMoq.Object, shellServiceMoq.Object,
                eventAggregatorMoq.Object, messageBoxServiceMoq.Object);

            contractServiceMoq.Setup(s => s.RenewContract(It.IsAny<Business.Models.Contract>(), It.IsAny<InsertContractRenew>(), It.IsAny<PaymentType>(), It.IsAny<decimal>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<decimal?>())).Throws<RenewContractException>();

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"renewLendingRate", new LendingRate()},
                {"contract", new Business.Models.Contract()}
            }));

            //Act
            vm.IsPrintDealDocument = true;
            vm.SelectedPaymentType = new PaymentType();

            Nito.AsyncEx.AsyncContext.Run(() =>
            {
                vm.RenewContractCommand.Execute();
            });

            //Assert
            shellServiceMoq.Verify(c => c.CloseShell<RenewContractWindow>(), Times.Once);
        }

        [Fact]
        public void RenewContractCommandMethodShouldCatchRenewContractException()
        {
            //Arrange
            var contractServiceMoq = new Mock<IContractService>();
            var shellServiceMoq = new Mock<IShellService>();
            var eventAggregatorMoq = new Mock<IEventAggregator>();
            var messageBoxServiceMoq = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            var moneyBalanceChangeEventMock = new Mock<MoneyBalanceChangedEvent>();
            eventAggregatorMoq.Setup(s => s.GetEvent<MoneyBalanceChangedEvent>())
                .Returns(moneyBalanceChangeEventMock.Object);
            var vm = new RenewContractPaymentViewModel(contractServiceMoq.Object, shellServiceMoq.Object,
                eventAggregatorMoq.Object, messageBoxServiceMoq.Object);

            contractServiceMoq.Setup(s => s.RenewContract(It.IsAny<Business.Models.Contract>(), It.IsAny<InsertContractRenew>(), It.IsAny<PaymentType>(), It.IsAny<decimal>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<decimal?>())).Throws<RenewContractException>();

            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"renewLendingRate", new LendingRate()},
                {"contract", new Business.Models.Contract()}
            }));

            //Act
            vm.IsPrintDealDocument = true;
            vm.SelectedPaymentType = new PaymentType();

            Nito.AsyncEx.AsyncContext.Run(() =>
            {
                vm.RenewContractCommand.Execute();
            });

            //Assert
            messageBoxServiceMoq.Verify(c => c.ShowError(It.IsNotNull<string>(), It.IsNotNull<string>()), Times.Once);
        }
    }
}