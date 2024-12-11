using ResourcePlanner.Domain;
using ResourcePlanner.Infrastructure.Adapters;
using ResourcePlanner.UseCases;
using ResourcePlanner.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ResourcePlanner.Infrastructure.Managers;
using ResourcePlanner.Utilities.MVVM;

namespace ResourcePlanner.Viewmodels
{
    class ResourcesViewModel : Bindable
    {
        private ResourceHandler? _resourceHandler;

        public ICommand MakeNewCMD { get; }
        public ICommand DeleteCMD { get; }
        public RelayCommand SaveCMD { get; private set; }

        private ObservableCollection<Resource> _resourceList;
        public ObservableCollection<Resource> Resourcelist
        {
            get { return _resourceList; }
            set
            {
                _resourceList = value;
                OnPropertyChanged();
            }
        }

        private Resource? _selectedResource;
        public Resource? SelectedResource
        {
            get { return _selectedResource; }
            set
            {
                System.Diagnostics.Debug.WriteLine(UserManager.Instance.InstitutionId);
                _selectedResource = value;
                OnPropertyChanged();
                PopulateResourceProfile();
                SaveCMD.UpdateCommand(Update, IsResourceSelected);
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        private string _imgPath;
        public string ImgPath
        {
            get { return _imgPath; }
            set
            {
                _imgPath = value;
                OnPropertyChanged();
            }
        }



        public ResourcesViewModel()
        {
            this.MakeNewCMD = new RelayCommand(ResetForm, IsResourceSelected);
            this.DeleteCMD = new RelayCommand(Delete, IsResourceSelected);
            this.SaveCMD = new RelayCommand(Create, CanCreate);

            this._resourceList = new ObservableCollection<Resource>();

            this._name = string.Empty;
            this._description = string.Empty;
            this._imgPath = string.Empty;

            LogOnScreenViewModel.UserLoggedIn += InitView;
        }

        private void ResetForm()
        {
            ResetFields();
            SelectedResource = null;
            SaveCMD.UpdateCommand(Create, CanCreate);
        }
        /// <summary>
        /// Function to create a resource and send it to the backend via the resource handler and adapter
        /// </summary>
        private async void Create()
        {
            if (UserManager.Instance.InstitutionId == null || _resourceHandler == null)
                return;

            Resource resource = new Resource(Name, Description, ImgPath, UserManager.Instance.InstitutionId);
            bool resourceCreated = await _resourceHandler.CreateResource(resource);
            if (resourceCreated)
            {
                ResetFields();
                SelectedResource = null;
                await PopulateResourceList();
            }
        }
        private bool CanCreate()
        {
            return !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(Description) &&
                   !string.IsNullOrEmpty(ImgPath);
        }
        /// <summary>
        /// Function to update the variables of an already existing resource using the handler and adapter
        /// </summary>
        private async void Update()
        {
            if (SelectedResource == null || _resourceHandler == null)
                return;

            SelectedResource.Name = Name;
            SelectedResource.Description = Description;
            SelectedResource.ImgPath = ImgPath;


            bool resourceUpdated = await _resourceHandler.UpdateResource(SelectedResource);
            if (resourceUpdated)
            {
                ResetForm();
                await PopulateResourceList();
            }

        }
        /// <summary>
        /// Function to delete a resource from the database using the handler and adapter
        /// </summary>
        private async void Delete()
        {
            if (SelectedResource == null || SelectedResource.Id == null || _resourceHandler == null)
                return;

            bool resourceDeleted = await _resourceHandler.DeleteResource(SelectedResource.Id);
            if (resourceDeleted)
            {
                ResetForm();
                await PopulateResourceList();
            }
        }
        private bool IsResourceSelected() => SelectedResource != null;
        /// <summary>
        /// This method is called when the user logs on and calls the populate resource list method.
        /// </summary>
        private async void InitView()
        {
            ResourceHttpAdapter resourceHttpAdapter = new ResourceHttpAdapter(RestApiClient.Instance.Client);
            this._resourceHandler = new ResourceHandler(resourceHttpAdapter);

            await PopulateResourceList();
        }
        /// <summary>
        /// Method to retrieve all the resources associated with the institutionID of the user logging on.
        /// </summary>
        /// <returns></returns>
        private async Task PopulateResourceList()
        {
            if (UserManager.Instance.InstitutionId == null || _resourceHandler == null)
                return;

            this.Resourcelist.Clear();
            var resources = await _resourceHandler.ReadAll(UserManager.Instance.InstitutionId);
            if (resources != null)
            {
                foreach (var resource in resources)
                    Resourcelist.Add(resource);
            }
        }
        /// <summary>
        /// Method used for loading the variables of a selected resource from the listview.
        /// </summary>
        private void PopulateResourceProfile()
        {
            if (SelectedResource == null)
                return;

            Name = SelectedResource.Name ?? string.Empty;
            Description = SelectedResource.Description ?? string.Empty;
            ImgPath = SelectedResource.ImgPath ?? string.Empty;
        }

        private void ResetFields()
        {
            Name = Description = ImgPath = string.Empty;
        }
        
    }
}

