using AutoMapper;
using Moq;
using PawnShop.Business.Dtos;
using PawnShop.Business.Models;
using PawnShop.Core.SharedVariables;
using PawnShop.Modules.Contract.ViewModels;
using PawnShop.Services.Interfaces;
using Prism.Events;
using Prism.Regions;
using System;
using System.ComponentModel;
using Xunit;

namespace PawnShop.Modules.Contract.UnitTests.ViewModels
{
    public class CreateContractSummaryViewModelTests
    {
        [Theory]
        [InlineData(500, 0)]
        [InlineData(1000, 20)]
        [InlineData(1500, 30)]
        [InlineData(1300, 26)]
        [InlineData(13500, 270)]
        public void PccPropertyShouldReturn2PercentWhenAbove1000Otherwise0(decimal contractAmount, decimal pccExpected)
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var sessionContextMock = new Mock<ISessionContext>();
            var contractServiceMock = new Mock<IContractService>();
            var mapperMock = new Mock<IMapper>();
            var shellServiceMock = new Mock<IShellService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var vm = new CreateContractSummaryViewModel(calculateServiceMock.Object, sessionContextMock.Object,
                contractServiceMock.Object,
                mapperMock.Object, shellServiceMock.Object, eventAggregatorMock.Object, messageBoxServiceMock.Object);

            vm.Contract.ContractItems.Add(new ContractItem() { EstimatedValue = contractAmount });

            //Act
            var pcc = vm.PCC;

            //Assert
            Assert.Equal(pccExpected, pcc);
        }

        [Fact]
        public void PropertiesShouldBeRaisedOnNavigatedTo()
        {
            //Arrange
            var calculateServiceMock = new Mock<ICalculateService>();
            var sessionContextMock = new Mock<ISessionContext>();
            var contractServiceMock = new Mock<IContractService>();
            var mapperMock = new Mock<IMapper>();
            var shellServiceMock = new Mock<IShellService>();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var messageBoxServiceMock = new Mock<IMessageBoxService>();
            var regionNavigateServiceMock = new Mock<IRegionNavigationService>();
            regionNavigateServiceMock.Setup(p => p.Region).Returns(new Region());
            sessionContextMock.Setup(s => s.LoggedPerson).Returns(new WorkerBossLoginDto());
            var vm = new CreateContractSummaryViewModel(calculateServiceMock.Object, sessionContextMock.Object,
                contractServiceMock.Object,
                mapperMock.Object, shellServiceMock.Object, eventAggregatorMock.Object, messageBoxServiceMock.Object);
            var wasPccRaised = false;
            var wasSumOfEstimatedValuesRaised = false;
            var wasNetStorageCostRaised = false;
            var wasRePurchasePriceRaised = false;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            vm.PropertyChanged += delegate (object? sender, PropertyChangedEventArgs args)
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
            {
                switch (args.PropertyName)
                {
                    case nameof(vm.PCC):
                        wasPccRaised = true;
                        break;
                    case nameof(vm.SumOfEstimatedValues):
                        wasSumOfEstimatedValuesRaised = true;
                        break;
                    case nameof(vm.NetStorageCost):
                        wasNetStorageCostRaised = true;
                        break;
                    case nameof(vm.RePurchasePrice):
                        wasRePurchasePriceRaised = true;
                        break;
                }
            };

            //Act
            vm.OnNavigatedTo(new NavigationContext(regionNavigateServiceMock.Object, new Uri("Test", UriKind.RelativeOrAbsolute), new NavigationParameters()
            {
                {"LendingRate", new LendingRate()},
                {"DealMaker", new Client()}
            }));

            //Assert
            Assert.True(wasPccRaised);
            Assert.True(wasSumOfEstimatedValuesRaised);
            Assert.True(wasNetStorageCostRaised);
            Assert.True(wasRePurchasePriceRaised);

        }
    }
}