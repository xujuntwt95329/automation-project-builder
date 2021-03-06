﻿using AutomationProjectBuilder.Data.Interfaces;
using AutomationProjectBuilder.Data.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AutomationProjectBuilder.Gui.ViewModels
{
    public class ViewModelDetailsEqCtrlModule : ViewModelDetailsBase
    {
        private IDialogService _dialogService;
        private IDataService _dataService;

        private ICommand _cmdAddFunction;
        private ICommand _cmdEdit;
        
        public ProjectModule Module { get; set; }
        
        public ICommand CmdAddFunction
        {
            get => _cmdAddFunction;
        }

        public ICommand CmdEdit
        {
            get => _cmdEdit;
        }

        public string NameParamSet
        {
            get => Module.NameParamSet;
            set { Module.NameParamSet = value; NotifyPropertChanged("NameParamSet"); }
        }

        public ObservableCollection<ViewModelListItem> ParameterList { get; set; } = new ObservableCollection<ViewModelListItem>();
        public ObservableCollection<ViewModelListItem> FunctionsList { get; set; } = new ObservableCollection<ViewModelListItem>();

        public ViewModelDetailsEqCtrlModule(ProjectModule module, IDialogService dialogService, IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            Module = module;

            ModuleId = module.Id;
            ModuleType = module.Type;

            if (module.NameParamSet == "") module.NameParamSet = "No parameters set";

            _cmdAddFunction = new DelegateCommand(x => 
            { 
                AddFunction(); 
            });
            _cmdEdit = new DelegateCommand(x => 
            { 
                LoadParameterSet(); 
            });
           
            foreach(ModuleParameter parameter in module.Parameters)
            {
                ParameterList.Add(new ViewModelListItem(parameter, this.ParameterList, module.Parameters));
            }

            foreach(ModuleFunction function in module.Functions)
            {
                FunctionsList.Add(new ViewModelListItem(function, this.FunctionsList, module.Functions));
            }
        }
        private async void LoadParameterSet() 
        {
            var dialog = new ViewModelDialogConfig(_dataService);

            var result = await _dialogService.ShowDialog(dialog);
            
            if(result.Value && dialog.SelectedParameterGroup != null && dialog.SelectedParameterSet != null)
            {
                NameParamSet = dialog.SelectedParameterSet.Name;

                Module.Parameters.Clear();
                
                ParameterList.Clear();

                var newParameterSet = dialog.SelectedParameterSet.Parameters;

                foreach (ModuleParameter parameter in newParameterSet)
                {
                    parameter.ModuleId = ModuleId;

                    Module.Parameters.Add(parameter);

                    ParameterList.Add(new ViewModelListItem(parameter, this.ParameterList, Module.Parameters));                  
                }
            }
        }

        private void AddFunction()
        {
            var function = new ModuleFunction(ModuleId, "New function");

            Module.Functions.Add(function);

            FunctionsList.Add(new ViewModelListItem(function, this.FunctionsList, Module.Functions));
        }
    }
}
