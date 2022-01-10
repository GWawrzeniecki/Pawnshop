using Moq;
using PawnShop.Business.Models;
using System;
using System.Collections.Generic;

namespace PawnShop.Modules.Contract.UnitTests
{
    public static class ContractDataGenerator
    {
        public static readonly LendingRate OneWeekLendingRate = new LendingRate() { Days = 7, Procent = 7 };
        public static readonly LendingRate TwoWeeksLendingRate = new LendingRate() { Days = 14, Procent = 16 };
        public static readonly LendingRate MonthLendingRate = new LendingRate() { Days = 30, Procent = 21 };

        public static Business.Models.Contract GetContract(DateTime? startDate = null)
        {
            startDate ??= DateTime.Today;

            var country = new Business.Models.Country() { Country1 = "Test" };
            var contractAmount = 1258;
            var estimatedItemValue = 1000;
            var contractState = new ContractState()
            {
                State = Core.Constants.Constants.CreatedContractState
            };
            var paymentType = new PaymentType()
            {
                Type = Core.Constants.Constants.CashPaymentType
            };
            var lendingRate = MonthLendingRate;
            var contractItemCategory = new ContractItemCategory()
            {
                Category = "Test",
                Measure = new UnitMeasure()
                {
                    Measure = "Test"
                }
            };
            var contractItemState = new ContractItemState()
            {
                State = "Test"
            };
            var dealMaker = new Business.Models.Client()
            {
                IdcardNumber = "Test",
                ValidityDateIdcard = DateTime.Today,
                Pesel = "Test",
                ClientNavigation = new Business.Models.Person()
                {
                    FirstName = "Adam",
                    LastName = "Nowak",
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
            };
            var workerBoss = new Business.Models.WorkerBoss
            {
                Pesel = "11111111111",
                Login = "test",
                Privilege = new Business.Models.Privilege { PawnShopTabs = true },
                WorkerBossNavigation = new Business.Models.Person
                {
                    FirstName = "Adam",
                    LastName = "Nowak",
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
            };
            var moneyBalance = new MoneyBalance() { TodayDate = DateTime.Today, MoneyBalance1 = 500 };

            return new Business.Models.Contract()
            {
                StartDate = startDate.Value,
                ContractNumberId = "01/2021",
                AmountContract = contractAmount,
                WorkerBoss = workerBoss,
                WorkerBossId = workerBoss.WorkerBossId,
                ContractItems = new List<ContractItem>(){new ContractItem()
                {
                    ContractNumberId = "01/2021",
                    Category = contractItemCategory,
                    CategoryId = contractItemCategory.Id,
                    ContractItemState = contractItemState,
                    ContractItemStateId = contractItemState.Id,
                    Name = "Laptop",
                    Amount = 1,
                    Description = "Test",
                    TechnicalCondition = "Test",
                    EstimatedValue = estimatedItemValue,
                    Laptop = new Laptop()
                    {
                        Brand = "Test",
                        Procesor = "Test",
                        DescriptionKit = "Test",
                        DriveType = "Test",
                        MassStorage = "Test",
                        Ram = "Test"
                    }
                }},
                DealMaker = dealMaker,
                DealMakerId = dealMaker.ClientId,
                ContractState = contractState,
                LendingRate = lendingRate,
                LendingRateId = lendingRate.Id,
                CreateContractDealDocument = new DealDocument()
                {
                    MoneyBalance = moneyBalance,
                    Payment = new Payment()
                    {
                        Date = DateTime.Today,
                        Amount = estimatedItemValue,
                        PaymentType = paymentType
                    }
                }
            };
        }

        public static Business.Models.Contract GetContractWithContractRenews(params ContractRenew[] contractRenews)
        {
            var contract = GetContract();
            foreach (var contractRenew in contractRenews)
            {
                contract.ContractRenews.Add(contractRenew);
            }

            return contract;
        }

        public static Business.Models.Contract GetContractWithContractRenews(DateTime startDate, params ContractRenew[] contractRenews)
        {
            var contract = GetContract(startDate);
            foreach (var contractRenew in contractRenews)
            {
                contract.ContractRenews.Add(contractRenew);
            }

            return contract;
        }

        public static Business.Models.Contract GetContractWithContractItems(params ContractItem[] contractRenews)
        {
            var contract = GetContract();
            foreach (var contractRenew in contractRenews)
            {
                contract.ContractItems.Add(contractRenew);
            }

            return contract;
        }

        public static IEnumerable<object[]> GetContractWithExpectedStartDate()
        {
            return new List<object[]>
            {
                new object[] { GetContract(), GetContract().StartDate },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = GetContract().StartDate.AddDays(30),
                    LendingRate = new LendingRate() { Days = 30 }
                }) , GetContract().StartDate.AddDays(30)},
            };
        }

        public static IEnumerable<object[]> GetLendingRatesAndExpectedRePurchaseDate()
        {
            return new List<object[]>
            {
                new object[] { null,null },
                new object[] { MonthLendingRate,DateTime.Today.AddDays(30).AddDays(MonthLendingRate.Days) },
                new object[] { TwoWeeksLendingRate,DateTime.Today.AddDays(30).AddDays(TwoWeeksLendingRate.Days) },
                new object[] { OneWeekLendingRate,DateTime.Today.AddDays(30).AddDays(OneWeekLendingRate.Days) }
            };
        }

        public static IEnumerable<object[]> GetLendingRatesAndExpectedDelay()
        {
            return new List<object[]>
            {   new object[] { null,0},
                new object[] { MonthLendingRate,MonthLendingRate.Days},
                new object[] { TwoWeeksLendingRate,TwoWeeksLendingRate.Days },
                new object[] { OneWeekLendingRate,OneWeekLendingRate.Days },
                new object[] { new LendingRate() {Days = 10},10},

            };
        }

        public static IEnumerable<object[]> GetContractGeneratorWithContractAmount()
        {
            return new List<object[]>
            {
                new object[] { GetContract(),1258},
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = GetContract().StartDate.AddDays(30),
                    LendingRate = TwoWeeksLendingRate
                }),1197 },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = GetContract().StartDate.AddDays(30),
                    LendingRate = OneWeekLendingRate
                }),1086 },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = GetContract().StartDate.AddDays(30),
                    LendingRate = TwoWeeksLendingRate
                }, new ContractRenew()
                {
                    RenewContractId = 2,
                    StartDate = GetContract().StartDate.AddDays(30).AddDays(14),
                    LendingRate = OneWeekLendingRate
                }),1086 }
            };
        }

        public static IEnumerable<object[]> GetContractGeneratorWithExpectedSumOfEstimatedValues()
        {
            return new List<object[]>
            {
                new object[] { GetContract(), 1000 },
                new object[] { GetContractWithContractItems(new ContractItem()
                {
                 EstimatedValue = 1000
                }), 2000 },
                new object[] { GetContractWithContractItems(new ContractItem()
                {
                    EstimatedValue = 1000
                }, new ContractItem()
                {
                    EstimatedValue = 3500
                } ), 5500 }

            };
        }

        public static IEnumerable<object[]> GetContractGeneratorWithExpectedContractDate()
        {
            var contract = GetContract();

            return new List<object[]>
            {
                new object[] { contract, contract.StartDate.AddDays(contract.LendingRate.Days) },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = contract.StartDate.AddDays(contract.LendingRate.Days),
                    LendingRate = new LendingRate() { Days = 30 }
                }),contract.StartDate.AddDays(contract.LendingRate.Days + 30) },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = contract.StartDate.AddDays(contract.LendingRate.Days),
                    LendingRate = new LendingRate() { Days = 30 }
                }),contract.StartDate.AddDays(contract.LendingRate.Days + 30) },
                new object[] { GetContractWithContractRenews(new ContractRenew()
                {
                    RenewContractId = 1,
                    StartDate = contract.StartDate.AddDays(contract.LendingRate.Days),
                    LendingRate = new LendingRate() { Days = 30 }
                }, new ContractRenew()
                {
                    RenewContractId = 2,
                    StartDate = contract.StartDate.AddDays(contract.LendingRate.Days).AddDays(30),
                    LendingRate = new LendingRate() { Days = 14}

                }), contract.StartDate.AddDays(contract.LendingRate.Days + 30 + 14)}
            };
        }

        public static IEnumerable<object[]> GetDelayAndExpectedRenewPrice()
        {
            return new List<object[]>
            {
                new object[]{new LendingRate() { Days = 10},500}
            };
        }

        public static IEnumerable<object[]> GetDelayAndExpectedBuyBackPrice()
        {
            return new List<object[]>
            {
                new object[]{new LendingRate() { Days = 10},1458}
            };
        }

        public static IEnumerable<object[]> GetNewLendingRateAndExpectedContractAmountAndMoqTimes()
        {
            return new List<object[]>
            {
                new object[]{null,0,Times.Never()},
                new object[]{new LendingRate(){Days = 14, Procent = 16},1258,Times.Once()}
            };
        }


        public static IEnumerable<object[]> GetContractGeneratorWithExpectedLateness()
        {
            var contract = GetContract();

            return new List<object[]>
            {
                new object[] { GetContract(), 0 },
                new object[] { GetContract(new DateTime(2021,11,25)), DateTime.Today.Subtract(new DateTime(2021,11,25).AddDays(MonthLendingRate.Days)).Days },
                new object[] { GetContract(new DateTime(2021,11,24)), DateTime.Today.Subtract(new DateTime(2021,11,24).AddDays(MonthLendingRate.Days)).Days },
                new object[] { GetContract(new DateTime(2021,11,14)), DateTime.Today.Subtract(new DateTime(2021,11,14).AddDays(MonthLendingRate.Days)).Days },
                new object[] { GetContract(new DateTime(2021,10,14)), DateTime.Today.Subtract(new DateTime(2021,10,14).AddDays(MonthLendingRate.Days)).Days },
                new object[] { GetContract(new DateTime(2021,7,5)), DateTime.Today.Subtract(new DateTime(2021,7,5).AddDays(MonthLendingRate.Days)).Days },
                new object[] { GetContractWithContractRenews(new DateTime(2021,11,25), new ContractRenew()
                {
                    RenewContractId = 1,
                    LendingRate = OneWeekLendingRate,
                    StartDate = new DateTime(2021,11,25).AddDays(30)
                }), DateTime.Today.Subtract(new DateTime(2021,11,25).AddDays(30).AddDays(OneWeekLendingRate.Days)).Days },
                new object[] { GetContractWithContractRenews(new DateTime(2021,10,25), new ContractRenew()
                {
                    RenewContractId = 1,
                    LendingRate = OneWeekLendingRate,
                    StartDate = new DateTime(2021,10,25).AddDays(30)
                }), DateTime.Today.Subtract(new DateTime(2021,10,25).AddDays(MonthLendingRate.Days).AddDays(OneWeekLendingRate.Days)).Days },
                new object[] { GetContractWithContractRenews(new DateTime(2021,10,25), new ContractRenew()
                {
                    RenewContractId = 1,
                    LendingRate = MonthLendingRate,
                    StartDate = new DateTime(2021,10,25).AddDays(30)
                }), DateTime.Today.Subtract(new DateTime(2021,10,25).AddDays(MonthLendingRate.Days).AddDays(MonthLendingRate.Days)).Days },
                new object[] { GetContractWithContractRenews(new DateTime(2021,10,25), new ContractRenew()
                {
                    RenewContractId = 1,
                    LendingRate = OneWeekLendingRate,
                    StartDate = new DateTime(2021,10,25).AddDays(30)
                }, new ContractRenew()
                {
                    RenewContractId = 2,
                    LendingRate = OneWeekLendingRate,
                    StartDate = new DateTime(2021,10,25).AddDays(37)
                }), DateTime.Today.Subtract(new DateTime(2021,10,25).AddDays(MonthLendingRate.Days).AddDays(OneWeekLendingRate.Days).AddDays(OneWeekLendingRate.Days)).Days }
            };
        }
    }
}