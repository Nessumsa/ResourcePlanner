using ResourcePlanner.Models;
using ResourcePlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    public class InstitutionViewModel : Bindable
    {
        private InstitutionHandler? _institutionHandler;
        private Institution? _instituttion;
        public ICommand SaveCMD { get; }

        private string _startTime;
        public string StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }

        private string _endTime;
        public string EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnPropertyChanged();
            }
        }

        private int _interval;
        public int Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                OnPropertyChanged();
            }
        }

        public InstitutionViewModel()
        {
            this.SaveCMD = new CommandRelay(Update, CanUpdate);

            this._startTime = string.Empty;
            this._endTime = string.Empty;
            this._interval = 0;

            LogOnScreenViewModel.UserLoggedIn += InitView;
        }

        private async void Update() 
        {
            if (_institutionHandler == null || _instituttion == null)
                return;

            _instituttion.OpenTime = StartTime;
            _instituttion.CloseTime = EndTime;
            _instituttion.BookingInterval = Interval;

            await _institutionHandler.UpdateInstitution(_instituttion);
        }
        private bool CanUpdate()
        {
            if (string.IsNullOrEmpty(StartTime) || !TimeValidator.IsValid(StartTime) ||
                string.IsNullOrEmpty(EndTime) || !TimeValidator.IsValid(EndTime))
                return false;

            DateTime startDateTime = DateTime.ParseExact(StartTime, "HH:mm", null);
            DateTime endDateTime = DateTime.ParseExact(EndTime, "HH:mm", null);
            if (startDateTime >= endDateTime)
                return false; 

            return Interval >= 5 && Interval <= 60;
        }

        private async void InitView() 
        {
            InstitutionHttpAdapter institutionHttpAdapter = new InstitutionHttpAdapter(RestApiClient.Instance.Client);
            this._institutionHandler = new InstitutionHandler(institutionHttpAdapter);

            await PopulateFields();
        }
        private async Task PopulateFields() 
        {
            if (UserManager.Instance.InstitutionId == null || _institutionHandler == null)
                return;

            _instituttion = await _institutionHandler.GetInstitution(UserManager.Instance.InstitutionId);
            if (_instituttion != null)
            {
                StartTime = _instituttion.OpenTime ?? string.Empty;
                EndTime = _instituttion.CloseTime ?? string.Empty;
                Interval = _instituttion.BookingInterval;
            }
        }
    }
}
