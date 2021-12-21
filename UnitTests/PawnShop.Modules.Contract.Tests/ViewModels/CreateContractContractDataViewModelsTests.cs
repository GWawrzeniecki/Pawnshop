using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using PawnShop.Business.Models;
using PawnShop.Modules.Contract.MenuItem;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Services.Dialogs;
using Smocks;
using Xunit;

namespace PawnShop.Modules.Contract.UnitTests.ViewModels
{
    public class CreateContractContractDataViewModelsTests
    {
        [StaFact]
        public void RepurchaseDateShouldBeValidOnSelectedLendingRateChange()
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
            vm.SelectedLendingRate = (vm.LendingRates.Result).First(l => l.Days == 7);

            //Assert
            Assert.Equal(DateTime.Today.AddDays(7), vm.RepurchaseDate);

            //Act
            vm.SelectedLendingRate = (vm.LendingRates.Result).First(l => l.Days == 14);

            //Assert
            Assert.Equal(DateTime.Today.AddDays(14), vm.RepurchaseDate);

            //Act
            vm.SelectedLendingRate = (vm.LendingRates.Result).First(l => l.Days == 30);

            //Assert
            Assert.Equal(DateTime.Today.AddDays(30), vm.RepurchaseDate);

        }
    }
}