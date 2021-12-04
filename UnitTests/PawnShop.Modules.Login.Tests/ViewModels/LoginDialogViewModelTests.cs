using Moq;
using PawnShop.Core.Extensions;
using PawnShop.Core.Interfaces;
using PawnShop.Modules.Login.Validators;
using PawnShop.Modules.Login.ViewModels;
using PawnShop.Services.Interfaces;
using Prism.Events;
using Xunit;

namespace PawnShop.Modules.Login.Tests.ViewModels
{

    public class LoginDialogViewModelTests
    {
        [StaFact]
        public void LoginButtonShouldNotBeEnabledWithoutEnteredData()
        {
            var loginServiceMoc = new Mock<ILoginService>();
            var uiServiceMock = new Mock<IUIService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var loginDialogValidatorMock = new Mock<LoginDialogValidator>();
            var havePasswordMock = new Mock<IHavePassword>();
            var vm = new LoginDialogViewModel(loginServiceMoc.Object, uiServiceMock.Object, eventAggregatorMock.Object, loginDialogValidatorMock.Object);

            Assert.False(vm.LoginCommand.CanExecute(havePasswordMock));
        }

        [StaFact]
        public void PasswordTagShouldBeFalseOnFailedLogin()
        {

            var loginServiceMoc = new Mock<ILoginService>();
            loginServiceMoc.Setup(s => s.LoginAsync("a", "a".ToSecureString())).ReturnsAsync((false, null));
            var uiServiceMock = new Mock<IUIService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var loginDialogValidatorMock = new Mock<LoginDialogValidator>();
            var havePasswordMock = new Mock<IHavePassword>();
            havePasswordMock.SetupGet(c => c.Password).Returns("a".ToSecureString());
            var vm = new LoginDialogViewModel(loginServiceMoc.Object, uiServiceMock.Object, eventAggregatorMock.Object, loginDialogValidatorMock.Object)
            {
                UserName = "a"
            };

            vm.LoginCommand.Execute(havePasswordMock.Object);

            Assert.False(vm.PasswordTag);
        }
    }
}
