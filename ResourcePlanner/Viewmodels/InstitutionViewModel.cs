using ResourcePlanner.Domain;
using ResourcePlanner.Infrastructure;
using ResourcePlanner.Infrastructure.Adapters;
using ResourcePlanner.UseCases;
using ResourcePlanner.Utilities;
using ResourcePlanner.Utilities.Regexes;
using System.Diagnostics;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    public class InstitutionViewModel : Bindable
    {
        private InstitutionHandler? _institutionHandler;
        private ImageHandler? _imageHandler;
        private Institution? _instituttion;
        public ICommand ChooseCMD { get; }
        public ICommand UploadCMD { get; }
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

        private string _selectedImagePath;

        public string SelectedImagePath
        {
            get { return _selectedImagePath; }
            set
            {
                _selectedImagePath = value;
                OnPropertyChanged();
            }
        }


        public InstitutionViewModel()
        {
            this.ChooseCMD = new CommandRelay(ChooseImage, CanChooseImage);
            this.UploadCMD = new CommandRelay(UploadImage, CanUploadImage);
            this.SaveCMD = new CommandRelay(Update, CanUpdate);

            this._startTime = string.Empty;
            this._endTime = string.Empty;
            this._interval = 0;
            this._selectedImagePath = string.Empty;

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
            if (_institutionHandler == null)
                return false;

            if (string.IsNullOrEmpty(StartTime) || !TimeValidator.IsValid(StartTime) ||
                string.IsNullOrEmpty(EndTime) || !TimeValidator.IsValid(EndTime))
                return false;

            DateTime startDateTime = DateTime.ParseExact(StartTime, "HH:mm", null);
            DateTime endDateTime = DateTime.ParseExact(EndTime, "HH:mm", null);
            if (startDateTime >= endDateTime)
                return false;

            return Interval >= 5 && Interval <= 60;
        }

        private void ChooseImage()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp"
            };

            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                SelectedImagePath = openFileDialog.FileName;
            }
        }

        private bool CanChooseImage()
        {
            return _imageHandler != null;
        }

        private async void UploadImage() 
        {
            if (_imageHandler == null || _instituttion == null)
                return;

            if (!string.IsNullOrEmpty(_instituttion.InstitutionImage))
                await _imageHandler.DeleteImage(_instituttion.InstitutionImage);

            var url = await _imageHandler.UploadImage(SelectedImagePath);
            if (url != null)
            {
                _instituttion.InstitutionImage = url;
                SelectedImagePath = string.Empty;
                Debug.WriteLine(_instituttion.InstitutionImage);
            }
        }
        private bool CanUploadImage()
        {
            return !string.IsNullOrEmpty(SelectedImagePath) &&
                   _imageHandler != null;
        }

        private async void InitView()
        {
            Debug.WriteLine("Calling init inst view");
            InstitutionHttpAdapter institutionHttpAdapter = new InstitutionHttpAdapter(RestApiClient.Instance.Client);
            ImageHttpAdapter imageHttpAdapter = new ImageHttpAdapter(RestApiClient.Instance.Client);
            this._institutionHandler = new InstitutionHandler(institutionHttpAdapter);
            this._imageHandler = new ImageHandler(imageHttpAdapter);

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
