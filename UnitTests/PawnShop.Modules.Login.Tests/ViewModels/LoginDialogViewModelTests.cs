using Moq;
using PawnShop.Business.Dtos;
using PawnShop.Core.Events;
using PawnShop.Core.Extensions;
using PawnShop.Core.Interfaces;
using PawnShop.Modules.Login.Validators;
using PawnShop.Modules.Login.ViewModels;
using PawnShop.Services.Interfaces;
using Prism.Events;
using System.Security;
using System.Threading.Tasks;
using Xunit;

namespace PawnShop.Modules.Login.UnitTests.ViewModels
{

    public class LoginDialogViewModelTests
    {
        [StaFact]
        public void LoginButtonShouldNotBeEnabledWithoutEnteredData()
        {
            //Arrange
            var loginServiceMoc = new Mock<ILoginService>();
            var uiServiceMock = new Mock<IUIService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var loginDialogValidatorMock = new Mock<LoginDialogValidator>();
            var havePasswordMock = new Mock<IHavePassword>();
            var messageBoxService = new Mock<IMessageBoxService>();

            //Act
            var vm = new LoginDialogViewModel(loginServiceMoc.Object, uiServiceMock.Object, eventAggregatorMock.Object, loginDialogValidatorMock.Object, messageBoxService.Object);

            //Assert
            Assert.False(vm.LoginCommand.CanExecute(havePasswordMock));
        }

        [StaTheory]
        [InlineData("a", "b")]
        public void PasswordTagShouldBeFalseOnFailedLogin(string login, string password)
        {
            //Arrange
            var loginServiceMoc = new Mock<ILoginService>();
            var uiServiceMock = new Mock<IUIService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var loginDialogValidatorMock = new Mock<LoginDialogValidator>();
            var havePasswordMock = new Mock<IHavePassword>();
            var messageBoxService = new Mock<IMessageBoxService>();
            havePasswordMock.SetupGet(c => c.Password).Returns(password.ToSecureString());
            var vm = new LoginDialogViewModel(loginServiceMoc.Object, uiServiceMock.Object, eventAggregatorMock.Object, loginDialogValidatorMock.Object, messageBoxService.Object)
            {
                UserName = login
            };
            loginServiceMoc.Setup(s => s.LoginAsync(vm.UserName, havePasswordMock.Object.Password)).ReturnsAsync((false, null));

            //Act
            vm.LoginCommand.Execute(havePasswordMock.Object);

            //Assert
            Assert.False(vm.PasswordTag);
        }

        [StaFact]
        public void UserChangedEventShouldBePublishedAfterSuccessfullyLogin()
        {
            //Arrange
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var userChangedEventMock = new Mock<UserChangedEvent>();
            var loginServiceMoc = new Mock<ILoginService>();
            loginServiceMoc.Setup(l => l.LoginAsync(It.IsAny<string>(), It.IsAny<SecureString>()))
                .Returns(Task.FromResult((true, new WorkerBossLoginDto())));
            var uiServiceMock = new Mock<IUIService>();
            var havePasswordMock = new Mock<IHavePassword>();
            havePasswordMock.SetupGet(c => c.Password).Returns("test".ToSecureString);
            var loginDialogValidatorMock = new Mock<LoginDialogValidator>();
            var messageBoxService = new Mock<IMessageBoxService>();
            eventAggregatorMock.
                Setup(x => x.GetEvent<UserChangedEvent>()).
                Returns(userChangedEventMock.Object);
            var vm = new LoginDialogViewModel(loginServiceMoc.Object, uiServiceMock.Object, eventAggregatorMock.Object, loginDialogValidatorMock.Object, messageBoxService.Object);

            //Act
            vm.LoginCommand.Execute(havePasswordMock.Object);

            //Assert
            userChangedEventMock.Verify(x => x.Publish(), Times.Once);
        }

        [StaFact]
        public void StartupProceduresShouldBeInvokedAfterSuccessfullyLogin()
        {
            //Arrange
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var userChangedEventMock = new Mock<UserChangedEvent>();
            var loginServiceMoc = new Mock<ILoginService>();
            var workerBoss = new WorkerBossLoginDto();
            loginServiceMoc.Setup(l => l.LoginAsync(It.IsAny<string>(), It.IsAny<SecureString>()))
                .Returns(Task.FromResult((true, workerBoss)));
            var uiServiceMock = new Mock<IUIService>();
            var havePasswordMock = new Mock<IHavePassword>();
            havePasswordMock.SetupGet(c => c.Password).Returns("test".ToSecureString);
            var loginDialogValidatorMock = new Mock<LoginDialogValidator>();
            var messageBoxService = new Mock<IMessageBoxService>();
            eventAggregatorMock.
                Setup(x => x.GetEvent<UserChangedEvent>()).
                Returns(userChangedEventMock.Object);
            var vm = new LoginDialogViewModel(loginServiceMoc.Object, uiServiceMock.Object, eventAggregatorMock.Object, loginDialogValidatorMock.Object, messageBoxService.Object);

            //Act
            vm.LoginCommand.Execute(havePasswordMock.Object);

            //Assert
            loginServiceMoc.Verify(x => x.LoadStartupData(workerBoss), Times.Once);
        }
    }
}
