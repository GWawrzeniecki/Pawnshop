using Moq;
using PawnShop.Business.Models;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System.Linq;
using Xunit;

namespace PawnShop.Modules.Contract.UnitTests.ViewModels
{
    public class CreateContractClientDataViewModelTests
    {
        [Fact]
        public void SearchClientCommandShouldBeDisabledWhenClientTextIsNullOrEmpty()
        {
            //Arrange
            var dialogServiceMock = new Mock<IDialogService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var clientServiceMock = new Mock<IClientService>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();

            var vm = new CreateContractClientDataViewModel(dialogServiceMock.Object, containerProviderMock.Object,
                clientServiceMock.Object, messageBoxServiceMock.Object);
            vm.SelectedClientSearchOption = vm.ClientSearchOptions.First();

            //Act
            vm.ClientSearchComboBoxText = null;

            //Assert
            Assert.False(vm.SearchClientCommand.CanExecute());

            //Act
            vm.ClientSearchComboBoxText = string.Empty;

            //Assert
            Assert.False(vm.SearchClientCommand.CanExecute());

            //Act
            vm.ClientSearchComboBoxText = "Test";

            //Assert
            Assert.True(vm.SearchClientCommand.CanExecute());

        }

        [StaFact]
        public void EditClientCommandShouldBeDisabledWhenSelectedClientIsNull()
        {
            //Arrange
            var dialogServiceMock = new Mock<IDialogService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var clientServiceMock = new Mock<IClientService>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();

            var vm = new CreateContractClientDataViewModel(dialogServiceMock.Object, containerProviderMock.Object,
                clientServiceMock.Object, messageBoxServiceMock.Object);
            vm.HamburgerMenuItem = new CreateContractClientDataHamburgerMenuItem();
            //Act
            vm.SelectedClient = null;

            //Assert
            Assert.False(vm.EditClientCommand.CanExecute());

            //Act
            vm.SelectedClient = new Client();

            //Assert
            Assert.True(vm.EditClientCommand.CanExecute());

        }

        [StaFact]
        public void IsNextButtonShouldBeTrueWhenSelectedClientIsNotNull()
        {
            //Arrange
            var dialogServiceMock = new Mock<IDialogService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var clientServiceMock = new Mock<IClientService>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();

            var vm = new CreateContractClientDataViewModel(dialogServiceMock.Object, containerProviderMock.Object,
                clientServiceMock.Object, messageBoxServiceMock.Object);
            vm.HamburgerMenuItem = new CreateContractClientDataHamburgerMenuItem();

            //Act
            vm.SelectedClient = null;

            //Assert
            Assert.False(vm.IsNextButtonEnabled);

            //Act
            vm.SelectedClient = new Client();

            //Assert
            Assert.True(vm.IsNextButtonEnabled);
        }
    }
}