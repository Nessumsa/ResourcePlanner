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
        private ImageHandler? _imageHandler;

        public ICommand MakeNewCMD { get; }
        public ICommand DeleteCMD { get; }
        public RelayCommand SaveCMD { get; private set; }
        public ICommand ChooseCMD { get; }
        public ICommand UploadCMD { get; }

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



        public ResourcesViewModel()
        {
            this.MakeNewCMD = new RelayCommand(ResetForm, IsResourceSelected);
            this.DeleteCMD = new RelayCommand(Delete, IsResourceSelected);
            this.SaveCMD = new RelayCommand(Create, CanCreate);
            this.ChooseCMD = new RelayCommand(ChooseImage, CanChooseImage);
            this.UploadCMD = new RelayCommand(UploadImage, CanUploadImage);

            this._resourceList = new ObservableCollection<Resource>();

            this._name = string.Empty;
            this._description = string.Empty;
            this._selectedImagePath = string.Empty;

            LogOnScreenViewModel.UserLoggedIn += InitView;
        }

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
        private bool CanChooseImage()
        {
            return _imageHandler != null;
        }
        private async void UploadImage()
        {
            if (_imageHandler == null || _resourceHandler == null || _selectedResource == null)
                return;

            // Delete the previous image if it exists.
            if (!string.IsNullOrEmpty(_selectedResource.ImgPath))
                await _imageHandler.DeleteImage(_selectedResource.ImgPath);

            // Upload the new image and update the resource's data.
            var url = await _imageHandler.UploadImage(SelectedImagePath);
            if (url != null)
            {
                _selectedResource.ImgPath = url;
                SelectedImagePath = string.Empty;

                await _resourceHandler.UpdateResource(_selectedResource);
            }
        }
        private bool CanUploadImage()
        {
            return !string.IsNullOrEmpty(SelectedImagePath) &&
                   _imageHandler != null &&
                   _resourceHandler != null;
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

            Resource resource = new Resource(Name, Description, SelectedImagePath, UserManager.Instance.InstitutionId);
            bool resourceCreated = await _resourceHandler.CreateResource(resource);
            if (resourceCreated)
            {
                
                SelectedResource = null;
                await PopulateResourceList();
                ResetForm();
            }
        }
        private bool CanCreate()
        {
            return !string.IsNullOrEmpty(Name) &&
                   !string.IsNullOrEmpty(Description) &&
                   !string.IsNullOrEmpty(SelectedImagePath);
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
            SelectedResource.ImgPath = SelectedImagePath;


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
            ImageHttpAdapter imageHttpAdapter = new ImageHttpAdapter(RestApiClient.Instance.Client);
            this._imageHandler = new ImageHandler(imageHttpAdapter);

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
            SelectedImagePath = SelectedResource.ImgPath ?? string.Empty;
        }

        private void ResetFields()
        {
            Name = Description = SelectedImagePath = string.Empty;
        }
        
    }
}

