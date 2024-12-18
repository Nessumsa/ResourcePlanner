using ResourcePlanner.Domain;
using ResourcePlanner.Infrastructure.Adapters;
using ResourcePlanner.Infrastructure.Managers;
using ResourcePlanner.Interfaces.Adapters.CRUD;
using ResourcePlanner.UseCases;
using ResourcePlanner.Utilities.MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ResourcePlanner.Viewmodels
{
    /// <summary>
    /// ViewModel for managing and displaying error reports in the application.
    /// </summary>
    public class ErrorReportsViewModel : Bindable
    {
        // Private field for handling error report use cases.
        private ErrorReportHandler? _errorReportHandler;

        /// Command to resolve the selected error report.
        public ICommand ResolveCMD { get; }

        private ObservableCollection<ErrorReport> _errorReportList;

        /// Collection of active error reports.
        public ObservableCollection<ErrorReport> ErrorReportList
        {
            get { return _errorReportList; }
            set
            {
                _errorReportList = value;
                OnPropertyChanged();
            }
        }

        private ErrorReport? _selectedErrorReport;

        /// The currently selected error report.
        public ErrorReport? SelectedErrorReport
        {
            get { return _selectedErrorReport; }
            set
            {
                _selectedErrorReport = value;
                OnPropertyChanged();
                PopulateDetails();
            }
        }

        // Fields for error report details.
        private string _dateCreated;
        private string _user;
        private string _resource;
        private string _description;

        /// Date the selected error report was created.
        public string DateCreated
        {
            get { return _dateCreated; }
            set
            {
                _dateCreated = value;
                OnPropertyChanged();
            }
        }

        /// User the selected error report was created by.
        public string User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        /// Resource related to the selected error report.
        public string Resource
        {
            get { return _resource; }
            set
            {
                _resource = value;
                OnPropertyChanged();
            }
        }

        /// Description of the selected error report.
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        /// Initializes a new instance of the ErrorReportsViewModel class.
        public ErrorReportsViewModel()
        {
            this.ResolveCMD = new RelayCommand(Resolve, CanResolve);

            this._errorReportList = new ObservableCollection<ErrorReport>();
            this._dateCreated = string.Empty;
            this._user = string.Empty;
            this._resource = string.Empty;
            this._description = string.Empty;

            // Subscribes to user login event to initialize the view.
            LogOnScreenViewModel.UserLoggedIn += InitView;

            //Subscribers to resource deleted event to reinitialize the view.
            ResourcesViewModel.ResourceDeleted += ResetFields;
            ResourcesViewModel.ResourceDeleted += PopulateList;
        }

        /// <summary>
        /// Resolves the currently selected error report by marking it as resolved.
        /// </summary>
        private async void Resolve()
        {
            if (SelectedErrorReport == null || _errorReportHandler == null)
                return;

            SelectedErrorReport.Resolved = true;
            bool reportDeleted = await _errorReportHandler.ResolveErrorReport(SelectedErrorReport);
            if (reportDeleted)
            {
                SelectedErrorReport = null;
                ResetFields();
                PopulateList();
            }
        }

        /// <summary>
        /// Determines whether the resolve command can execute.
        /// </summary>
        /// <returns>True if a handler and selected error report are available; otherwise, false.</returns>
        private bool CanResolve()
        {
            return _errorReportHandler != null &&
                   SelectedErrorReport != null;
        }

        /// <summary>
        /// Initializes the view by setting up the error report handler and populating the error reports list.
        /// </summary>
        private void InitView()
        {
            IReadAdapter<Resource, string> resourceAdapter = new ResourceHttpAdapter(RestApiClient.Instance.Client);
            IReadAdapter<User, string> userAdapter = new UserHttpAdapter(RestApiClient.Instance.Client);
            ErrorReportHttpAdapter errorReportHttpAdapter = new ErrorReportHttpAdapter(RestApiClient.Instance.Client,
                                                                                       resourceAdapter, 
                                                                                       userAdapter);
            this._errorReportHandler = new ErrorReportHandler(errorReportHttpAdapter);

            PopulateList();
        }

        /// <summary>
        /// Populates the list of active error reports for the current institution.
        /// </summary>
        private async void PopulateList()
        {
            if (UserManager.Instance.InstitutionId == null || _errorReportHandler == null)
                return;

            this.ErrorReportList.Clear();
            var errorReports = await _errorReportHandler.GetAllActiveErrorReports(UserManager.Instance.InstitutionId);
            if (errorReports != null)
            {
                foreach (var errorReport in errorReports)
                    ErrorReportList.Add(errorReport);
            }
        }

        /// <summary>
        /// Populates the details fields for the currently selected error report.
        /// </summary>
        private void PopulateDetails()
        {
            if (SelectedErrorReport == null)
                return;

            DateCreated = SelectedErrorReport.CreatedDate.ToShortDateString();
            User = SelectedErrorReport.AssociatedUser?.Name ?? string.Empty;
            Resource = SelectedErrorReport.AssociatedResource?.Name ?? string.Empty;
            Description = SelectedErrorReport.Description ?? string.Empty;
        }

        /// <summary>
        /// Resets the details fields to their default values.
        /// </summary>
        private void ResetFields()
        {
            DateCreated = User = Resource = Description = string.Empty;
        }
    }
}