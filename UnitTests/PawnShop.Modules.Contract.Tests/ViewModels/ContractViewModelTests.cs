using AutoMapper;
using Moq;
using PawnShop.Core.SharedVariables;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Services.Dialogs;
using Xunit;

namespace PawnShop.Modules.Contract.UnitTests.ViewModels
{
    public class ContractViewModelTests
    {
        [Fact]
        public void FormValidationShouldWorkAsExpected()
        {
            //Arrange
            var contractServiceMock = new Mock<IContractService>();
            var dialogServiceMock = new Mock<IDialogService>();
            var shellServiceMock = new Mock<IShellService>();
            var containerProviderMock = new Mock<IContainerProvider>();
            var sessionContextMock = new Mock<ISessionContext>();
            var userSettings = new Mock<IUserSettings>();
            var messageBoxService = new Mock<IMessageBoxService>();
            var mapperMock = new Mock<IMapper>();

            var vm = new ContractViewModel(contractServiceMock.Object, dialogServiceMock.Object, userSettings.Object, shellServiceMock.Object, containerProviderMock.Object, new Validators.ContractValidator(), sessionContextMock.Object, messageBoxService.Object, mapperMock.Object);

            //Act
            vm.ContractNumber = "01x2021";

            //Assert
            Assert.True(vm.HasErrors);

            //Act
            vm.ContractNumber = "01/2021";

            //Assert
            Assert.True(!vm.HasErrors);

            //Act
            vm.ContractAmount = "test";

            //Assert
            Assert.True(vm.HasErrors);

            //Act
            vm.ContractAmount = "1500";

            //Assert
            Assert.True(!vm.HasErrors);

            //Act
            vm.Client = "AdamNowak";

            //Assert
            Assert.True(vm.HasErrors);

            //Act
            vm.Client = "Adam Nowak";

            //Assert
            Assert.True(!vm.HasErrors);

            //Act
            vm.Client = "Łukasz Tęst";

            //Assert
            Assert.True(!(vm.HasErrors));

            //Act
            vm.Client = "Łukasz Tęst Test";

            //Assert
            Assert.True(!(vm.HasErrors));

            //Act
            vm.Client = "Łukasz Tęst Tęst";

            //Assert
            Assert.True(!(vm.HasErrors));

            //Act
            vm.Client = "Adam";

            //Assert
            Assert.True(!vm.HasErrors);

            //Act
            vm.Client = "Nowak";

            //Assert
            Assert.True(!vm.HasErrors);

            //Act
            vm.Client = "NowakK";

            //Assert
            Assert.True(vm.HasErrors);

            //Act
            vm.Client = "Adam NowakK";

            //Assert
            Assert.True(vm.HasErrors);


        }
    }
}