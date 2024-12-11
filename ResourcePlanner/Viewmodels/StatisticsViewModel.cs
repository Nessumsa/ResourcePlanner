using ResourcePlanner.Domain;
using ResourcePlanner.Infrastructure;
using ResourcePlanner.Infrastructure.Adapters;
using ResourcePlanner.UseCases;
using ResourcePlanner.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace ResourcePlanner.Viewmodels
{
    class StatisticsViewModel : Bindable
    {
        private StatisticsHandler? _statisticsHandler;

        public ICommand GenerateCMD { get; }

        private DateTime _startDate = DateTime.Today;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime _endTime = DateTime.Today;
        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnPropertyChanged();
            }
        }

        public StatisticsViewModel()
        {
            
            this.GenerateCMD = new RelayCommand(Generate, CanGenerate);
            LogOnScreenViewModel.UserLoggedIn += InitView;
        }
        private void InitView()
        {
            StatisticsHttpAdapter statisticsHttpAdapter = new StatisticsHttpAdapter(RestApiClient.Instance.Client);
            this._statisticsHandler = new StatisticsHandler(statisticsHttpAdapter);
        }
        private async void Generate()
        {
            if (UserManager.Instance.InstitutionId == null || _statisticsHandler == null)
                return;

            Statistics statistics = new Statistics(StartDate, EndTime, UserManager.Instance.InstitutionId);
            bool rapportCreated = await _statisticsHandler.CreateStatistics(statistics);
            if (rapportCreated)
            {
                ResetFields();
                //await PopulateResourceList();
            }
        }
        private bool CanGenerate()
        {
            return StartDate != null &&
                   EndTime != null;
        }
        private void ResetFields()
        {
            StartDate = DateTime.Today;
            EndTime = DateTime.Today;
        }
    }
}
