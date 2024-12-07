using ResourcePlanner.Domain;
using ResourcePlanner.Infrastructure.Adapters;
using ResourcePlanner.Infrastructure;
using ResourcePlanner.Models;
using ResourcePlanner.UseCases;
using ResourcePlanner.Utilities;
using ResourcePlanner.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Printing;

namespace ResourcePlanner.Viewmodels
{
    class ResourcesViewModel : Bindable
    {
        private ResourceHandler? _resourceHandler;

        public ICommand MakeNewCMD { get; }
        public ICommand DeleteCMD { get; }
        public ICommand SaveCMD { get; private set; }

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
                _selectedResource = value;
                OnPropertyChanged();
                PopulateResource();
                SaveCMD = new CommandRelay(Update, IsResourceSelected);
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
            this.MakeNewCMD = new CommandRelay(ResetForm, IsResourceSelected);
            this.DeleteCMD = new CommandRelay(Delete, IsResourceSelected);
            this.SaveCMD = new CommandRelay(Create, CanCreate);

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
            SaveCMD = new CommandRelay(Create, CanCreate);
        }

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

        private async void Update()
        {
            if (SelectedResource == null || _resourceHandler == null)
                return;

            SelectedResource.Name = Name;
            SelectedResource.Description = Description;
            SelectedResource.ImgPath = ImgPath;

            await _resourceHandler.UpdateResource(SelectedResource);
        }

        private async void Delete()
        {
            if (SelectedResource == null || SelectedResource.Id == null || _resourceHandler == null)
                return;

            bool resourceDeleted = await _resourceHandler.DeleteResource(SelectedResource.Id);
            if (resourceDeleted)
            {
                ResetFields();
                SelectedResource = null;
                await PopulateResourceList();
            }
        }
        private bool IsResourceSelected() => SelectedResource != null;

        private async void InitView()
        {
            ResourceHttpAdapter resourceHttpAdapter = new ResourceHttpAdapter(RestApiClient.Instance.Client);
            this._resourceHandler = new ResourceHandler(resourceHttpAdapter);

            await PopulateResourceList();
        }

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
        private void PopulateResource()
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
