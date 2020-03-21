﻿using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IDialogService _dialogService;
        private ViewModelBase _detailsPage;
        private ICommand _cmdSelectedItem;

        public ViewModelBase DetailsPage
        {
            get
            {
                return _detailsPage;
            }
            set
            {
                _detailsPage = value;
                NotifyPropertChanged("DetailsPage");
            }
        }
        public ICommand CmdSelectedItem
        {
            get { return _cmdSelectedItem; }
        }

        public ObservableCollection<TreeItemViewModel> ProjectStructure { get; } = new ObservableCollection<TreeItemViewModel>();

        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            _cmdSelectedItem = new DelegateCommand(x => GetSelectedItem(x));

            DetailsPage = new DetailsViewModel("No item selected");

            LoadLatestConfig();
        }

        private void LoadLatestConfig()
        {
            var root = new TreeItemViewModel("Project", ItemTypeISA88.ProcessCell, _dialogService);
            
            var subsys1 = new TreeItemViewModel("Subsystem 1", ItemTypeISA88.EquipmentModule, _dialogService);
            var subsys2 = new TreeItemViewModel("Subsystem 2", ItemTypeISA88.EquipmentModule, _dialogService);
            var subsys3 = new TreeItemViewModel("Subsystem 3", ItemTypeISA88.EquipmentModule, _dialogService);

            subsys2.AddSubsystem(subsys3);

            root.AddSubsystem(subsys1);
            root.AddSubsystem(subsys2);

            ProjectStructure.Add(root);
        }

        private void GetSelectedItem(object x)
        {
            var selectedItem = ProjectStructure[0].GetSelectedItem(new TreeItemViewModel());

            DetailsPage = new DetailsViewModel(selectedItem.ItemName);
        }
    }
}
