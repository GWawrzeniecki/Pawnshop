using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using PawnShop.Core.Models.ApiModels;
using PawnShop.Services.DataService;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawnShop.Modules.Home.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        #region PrivateMembers

        private readonly IContainerProvider _containerProvider;
        private readonly IMessageBoxService _messageBoxService;
        private IList<ISeries> _renews;
        private IList<ISeries> _sales;
        private IList<ISeries> _exchangeRates;
        private IList<Axis> _dateChartsXAxes;
        private IList<ISeries> _goldRates;
        private bool _isBusy;

        #endregion

        #region Constructor

        public HomeViewModel(IContainerProvider containerProvider, IMessageBoxService messageBoxService)
        {
            _containerProvider = containerProvider;
            _messageBoxService = messageBoxService;
            Sales = new List<ISeries>();
            Renews = new List<ISeries>();
            ExchangeRates = new List<ISeries>();
            GoldRates = new List<ISeries>();
            InitializeExchangeRatesXAxes();
            LoadAllCharts();
        }

        #endregion

        #region PublicProperties

        public IList<ISeries> Sales
        {
            get => _sales;
            set => SetProperty(ref _sales, value);
        }

        public IList<ISeries> Renews
        {
            get => _renews;
            set => SetProperty(ref _renews, value);
        }

        public IList<ISeries> ExchangeRates
        {
            get => _exchangeRates;
            set => SetProperty(ref _exchangeRates, value);
        }

        public IList<ISeries> GoldRates
        {
            get => _goldRates;
            set => SetProperty(ref _goldRates, value);
        }

        public IList<Axis> DateChartsXAxes
        {
            get => _dateChartsXAxes;
            set => SetProperty(ref _dateChartsXAxes, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }


        #endregion

        #region PrivateMethods

        private async void LoadAllCharts()
        {
            IsBusy = true;
            await LoadSellChart();
            await LoadRenewChart();
            await LoadExchangeRatesChart();
            await LoadGoldRatesChart();
            IsBusy = false;
        }

        private async Task LoadSellChart()
        {
            try
            {
                await TryToLoadSellChart();
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError($"Wystąpił błąd podczas ładowania wykresu sprzedaży.{Environment.NewLine}Błąd: {e.Message}",
                    "Wykres sprzedaży - błąd");
            }
        }

        private async Task TryToLoadSellChart()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            var sales = await unitOfWork.SaleRepository.GetSaleInDays(new DateTime(2021, 11, 27), DateTime.Today);
            var tmp = new List<ISeries>();
            for (var index = 1; index < sales.Count + 1; index++)
            {
                var saleChartDto = sales[index - 1];
                tmp.Add(new PieSeries<decimal>()
                {
                    Values = new decimal[] { saleChartDto.SoldPriceSum },
                    Name = saleChartDto.SaleDate.ToShortDateString(),
                    InnerRadius = 150
                });
            }

            Sales = tmp;
        }

        private async Task LoadRenewChart()
        {
            try
            {
                await TryToLoadRenewChart();
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError($"Wystąpił błąd podczas ładowania wykresu przedłużeń.{Environment.NewLine}Błąd: {e.Message}",
                    "Wykres przedłużeń - błąd");
            }
        }

        private async Task TryToLoadRenewChart()
        {
            using var unitOfWork = _containerProvider.Resolve<IUnitOfWork>();
            var sales = await unitOfWork.DealDocumentRepository.GetRenewSumInDays(new DateTime(2021, 11, 27), DateTime.Today);
            var tmp = new List<ISeries>();
            for (var index = 1; index < sales.Count + 1; index++)
            {
                var dealDocumentRenewChartDto = sales[index - 1];
                tmp.Add(new PieSeries<decimal>()
                {
                    Values = new decimal[] { dealDocumentRenewChartDto.IncomeSum },
                    Name = dealDocumentRenewChartDto.RenewDate.ToShortDateString(),
                    InnerRadius = 150
                });
            }

            Renews = tmp;
        }

        private async Task LoadExchangeRatesChart()
        {
            try
            {
                await TryToLoadExchangeRatesChart();
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError($"Wystąpił błąd podczas ładowania wykresu walut.{Environment.NewLine}Błąd: {e.Message}",
                    "Wykres sprzedazy - blad");
            }
        }

        private async Task TryToLoadExchangeRatesChart()
        {
            var currencies = new List<string>
            {
                "USD",
                "AUD",
                "CAD",
                "TRY"
            };

            var currencyDic = new Dictionary<string, IList<DateTimePoint>>();

            currencies.ForEach(c => currencyDic.Add(c, new List<DateTimePoint>()));

            var apiService = _containerProvider.Resolve<IApiService>();
            var exchangeRates = await apiService.GetResponseInJson<List<ExchangeRate>>(BuildExchangeRateUrl(DateTime.Today.AddDays(-5), DateTime.Today));

            var containsOurCurrencies = currencies.All(c => exchangeRates.All(ex => ex.rates.ToList().Any(r => r.code.Equals(c))));

            if (!containsOurCurrencies)
                throw new Exception("Dane nie zawierały wszystkich walut.");

            foreach (var exchangeRate in exchangeRates)
            {
                currencies.ForEach(c => currencyDic[c].Add(new DateTimePoint(DateTime.Parse(exchangeRate.effectiveDate),
                    exchangeRate.rates.First(r => r.code.Equals(c)).mid)));
            }

            var tmp = new List<ISeries>();
            currencies.ForEach(c => tmp.Add(new ColumnSeries<DateTimePoint>()
            {
                Values = currencyDic[c],
                Name = c
            }));

            ExchangeRates = tmp;
        }

        private void InitializeExchangeRatesXAxes()
        {
            DateChartsXAxes = new List<Axis>
            {
                new Axis
                {
                    Labeler = value => new DateTime((long) value).ToString("MMMM dd"),
                    LabelsRotation = 15,

                    // in this case we want our columns with a width of 1 day, we can get that number
                    // using the following syntax
                    UnitWidth = TimeSpan.FromDays(1).Ticks,

                    // The MinStep property forces the separator to be greater than 1 day.
                    MinStep = TimeSpan.FromDays(1).Ticks

                    // if the difference between our points is in hours then we would:
                    // UnitWidth = TimeSpan.FromHours(1).Ticks,

                    // since all the months and years have a different number of days
                    // we can use the average, it would not cause any visible error in the user interface
                    // Months: TimeSpan.FromDays(30.4375).Ticks
                    // Years: TimeSpan.FromDays(365.25).Ticks
                }
            };
        }

        private string BuildExchangeRateUrl(DateTime dateFrom, DateTime dateTo)
        {
            return $"http://api.nbp.pl/api/exchangerates/tables/a/{dateFrom:yyyy-MM-dd}/{dateTo:yyyy-MM-dd}/?format=json";
        }

        private async Task LoadGoldRatesChart()
        {
            try
            {
                await TryToLoadGoldRatesChart();
            }
            catch (Exception e)
            {
                _messageBoxService.ShowError($"Wystąpił błąd podczas ładowania wykresu złota.{Environment.NewLine}Błąd: {e.Message}",
                    "Wykres złota - blad");
            }
        }

        private async Task TryToLoadGoldRatesChart()
        {
            var goldRatesDateTimePoints = new List<DateTimePoint>();
            var apiService = _containerProvider.Resolve<IApiService>();
            var goldRates = await apiService.GetResponseInJson<List<GoldRate>>(BuildGoldRateUrl(DateTime.Today.AddDays(-5), DateTime.Today));

            goldRates.ForEach(gr => goldRatesDateTimePoints.Add(new DateTimePoint(DateTime.Parse(gr.data), gr.cena)));

            GoldRates = new List<ISeries>()
            {
                new ColumnSeries<DateTimePoint>()
                {
                    Values = goldRatesDateTimePoints,
                    Name = "Złoto próba: 1000"
                }
            };
        }

        private string BuildGoldRateUrl(DateTime dateFrom, DateTime dateTo)
        {
            return $"http://api.nbp.pl/api/cenyzlota/{dateFrom:yyyy-MM-dd}/{dateTo:yyyy-MM-dd}/?format=json";
        }

        #endregion

    }


}