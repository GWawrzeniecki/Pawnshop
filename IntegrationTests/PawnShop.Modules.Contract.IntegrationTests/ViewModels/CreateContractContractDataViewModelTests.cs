using IntegrationTests.Base;
using PawnShop.Modules.Contract.ViewModels;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PawnShop.Modules.Contract.IntegrationTests.ViewModels
{
    [Collection("Sequential")]
    public class CreateContractContractDataViewModelTests : IntegrationTestBase<CreateContractContractDataViewModel>
    {
        [StaFact]
        [Isolated]
        public async Task ContractNumberShouldBe01YearForFirstDealDocumentAsync()
        {
            //Arrange

            //Act
            var contractNumber = await ViewModel.ContractNumber.Task;

            //Assert            
            Assert.Equal($"01/{DateTime.Now.Year}", contractNumber);
        }
    }
}