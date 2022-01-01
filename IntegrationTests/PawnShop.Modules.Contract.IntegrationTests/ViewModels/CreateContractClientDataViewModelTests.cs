using FluentAssertions;
using IntegrationTests.Base;
using PawnShop.Modules.Contract.ViewModels;
using System;
using System.Linq;
using Xunit;
using ClientSearchOption = PawnShop.Modules.Contract.Enums.ClientSearchOption;

namespace PawnShop.Modules.Contract.IntegrationTests.ViewModels
{
    [Collection("Sequential")]
    public class CreateContractClientDataViewModelTests : IntegrationTestBase<CreateContractClientDataViewModel>
    {
        [StaTheory]
        [Isolated]
        [InlineData(1, "Nowak", ClientSearchOption.Surname)]
        [InlineData(2, "Nowak", ClientSearchOption.Surname)]
        [InlineData(1, "11111111111", ClientSearchOption.Pesel)]
        [InlineData(2, "11111111111", ClientSearchOption.Pesel)]

        public void SearchedClientsShouldBeNotNullAndValidCountAfterValidSearch(int amount, string value, ClientSearchOption clientSearchOption)

        {
            //Arrange
            using var pawnshopContext = PawnshopContext;
            var country = new Business.Models.Country() { Country1 = "Test" };
            for (int i = 0; i < amount; i++)
            {
                pawnshopContext.Clients.Add(new Business.Models.Client()
                {
                    IdcardNumber = "Test",
                    ValidityDateIdcard = DateTime.Today,
                    Pesel = value,
                    ClientNavigation = new Business.Models.Person()
                    {
                        FirstName = "Adam",
                        LastName = value,
                        BirthDate = DateTime.Today,
                        Address = new Business.Models.Address
                        {
                            Street = "Test",
                            HouseNumber = "10",
                            PostCode = "11111",
                            Country = country,
                            City = new Business.Models.City { City1 = "Test", Country = country }
                        }
                    }
                });
            }

            pawnshopContext.Clients.Add(new Business.Models.Client()
            {
                IdcardNumber = "Test",
                ValidityDateIdcard = DateTime.Today,
                Pesel = "TEST",
                ClientNavigation = new Business.Models.Person()
                {
                    FirstName = "Adam",
                    LastName = "TEST",
                    BirthDate = DateTime.Today,
                    Address = new Business.Models.Address
                    {
                        Street = "Test",
                        HouseNumber = "10",
                        PostCode = "11111",
                        Country = country,
                        City = new Business.Models.City { City1 = "Test", Country = country }
                    }
                }
            });

            pawnshopContext.SaveChanges();
            ViewModel.ClientSearchComboBoxText = value;
            ViewModel.SelectedClientSearchOption =
                ViewModel.ClientSearchOptions.First(s => s.SearchOption == clientSearchOption);

            //Act
            Nito.AsyncEx.AsyncContext.Run(() =>
            {
                ViewModel.SearchClientCommand.Execute();
            });

            //Assert
            Assert.NotNull(ViewModel.SearchedClients);
            ViewModel.SearchedClients.Count.Should().Be(amount);
        }
    }
}