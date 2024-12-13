using ResourcePlanner.Domain;
using ResourcePlanner.Infrastructure.Adapters;
using ResourcePlanner.Infrastructure.Managers;
using ResourcePlanner.UseCases;
using ResourcePlanner.Utilities.MVVM;
using ResourcePlanner.Utilities.Regexes;
using System.Windows;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    /// <summary>
    /// ViewModel for managing institution settings such as working hours, booking intervals, and institution image.
    /// </summary>
    public class InstitutionViewModel : Bindable
    {
        // Handlers for institution and image operations.
        private InstitutionHandler? _institutionHandler;
        private ImageHandler? _imageHandler;

        // Represents the institution currently being managed.
        private Institution? _instituttion;

        /// Command to choose an image for the institution.
        public ICommand ChooseCMD { get; }

        /// Command to upload the selected image for the institution.
        public ICommand UploadCMD { get; }

        /// Command to save updated institution settings.
        public ICommand SaveCMD { get; }

        // Backing fields for institution settings.
        private string _startTime;
        private string _endTime;
        private int _interval;
        private string _selectedImagePath;

        /// The start time of the institution's opening hours.
        public string StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }

        /// The end time of the institution's closing hours.
        public string EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnPropertyChanged();
            }
        }

        /// The booking interval for the institution, in minutes.
        public int Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                OnPropertyChanged();
            }
        }

        /// The file path of the selected image for upload.
        public string SelectedImagePath
        {
            get { return _selectedImagePath; }
            set
            {
                _selectedImagePath = value;
                OnPropertyChanged();
            }
        }

        /// Initializes a new instance of the InstitutionViewModel class.
        public InstitutionViewModel()
        {
            this.ChooseCMD = new RelayCommand(ChooseImage, CanChooseImage);
            this.UploadCMD = new RelayCommand(UploadImage, CanUploadImage);
            this.SaveCMD = new RelayCommand(Update, CanUpdate);

            this._startTime = string.Empty;
            this._endTime = string.Empty;
            this._interval = 0;
            this._selectedImagePath = string.Empty;

            // Subscribe to user login event to initialize the view.
            LogOnScreenViewModel.UserLoggedIn += InitView;
        }

        /// <summary>
        /// Updates the institution's settings with the current values.
        /// </summary>
        private async void Update()
        {
            if (_institutionHandler == null || _instituttion == null)
                return;

            _instituttion.OpenTime = StartTime;
            _instituttion.CloseTime = EndTime;
            _instituttion.BookingInterval = Interval;

            bool updateSuccess = await _institutionHandler.UpdateInstitution(_instituttion);
            if (updateSuccess)
                MessageBox.Show("Institution has been updated successfully", 
                                "Success", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.None);
        }

        /// <summary>
        /// Determines if the SaveCMD can execute based on validation of settings.
        /// </summary>
        /// <returns>True if settings are valid; otherwise, false.</returns>
        private bool CanUpdate()
        {
            if (_institutionHandler == null)
                return false;

            if (string.IsNullOrEmpty(StartTime) || !TimeValidator.IsValid(StartTime) ||
                string.IsNullOrEmpty(EndTime) || !TimeValidator.IsValid(EndTime))
                return false;

            // Ensure that start time is before end time.
            DateTime startDateTime = DateTime.ParseExact(StartTime, "HH:mm", null);
            DateTime endDateTime = DateTime.ParseExact(EndTime, "HH:mm", null);
            if (startDateTime >= endDateTime)
                return false;

            // Ensure the interval is within the valid range.
            return Interval >= 5 && Interval <= 60;
        }

        /// <summary>
        /// Opens a file dialog to select an image for the institution.
        /// </summary>
        private void ChooseImage()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp"
            };

            var result = openFileDialog.ShowDialog();
            if (result == true)
                SelectedImagePath = openFileDialog.FileName;
        }

        /// <summary>
        /// Determines if the ChooseCMD can execute.
        /// </summary>
        /// <returns>True if an image handler is available; otherwise, false.</returns>
        private bool CanChooseImage()
        {
            return _imageHandler != null;
        }

        /// <summary>
        /// Uploads the selected image and updates the institution's image URL.
        /// </summary>
        private async void UploadImage()
        {
            if (_imageHandler == null || _institutionHandler == null || _instituttion == null)
                return;

            // Delete the previous image if it exists.
            if (!string.IsNullOrEmpty(_instituttion.imageUrl))
                await _imageHandler.DeleteImage(_instituttion.imageUrl);

            // Upload the new image and update the institution's data.
            var url = await _imageHandler.UploadImage(SelectedImagePath);
            if (url != null)
            {
                _instituttion.imageUrl = url;
                SelectedImagePath = string.Empty;

                await _institutionHandler.UpdateInstitution(_instituttion);
                MessageBox.Show("Image has been uploaded successfully",
                                "Success",
                                MessageBoxButton.OK,
                                MessageBoxImage.None);
            }
        }

        /// <summary>
        /// Determines if the UploadCMD can execute.
        /// </summary>
        /// <returns>True if a valid image path and handlers are available; otherwise, false.</returns>
        private bool CanUploadImage()
        {
            return !string.IsNullOrEmpty(SelectedImagePath) &&
                   _imageHandler != null &&
                   _institutionHandler != null;
        }

        /// <summary>
        /// Initializes the view by setting up handlers and populating fields with institution data.
        /// </summary>
        private async void InitView()
        {
            InstitutionHttpAdapter institutionHttpAdapter = new InstitutionHttpAdapter(RestApiClient.Instance.Client);
            ImageHttpAdapter imageHttpAdapter = new ImageHttpAdapter(RestApiClient.Instance.Client);
            this._institutionHandler = new InstitutionHandler(institutionHttpAdapter);
            this._imageHandler = new ImageHandler(imageHttpAdapter);

            await PopulateFields();
        }

        /// <summary>
        /// Populates the fields with data from the institution.
        /// </summary>
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