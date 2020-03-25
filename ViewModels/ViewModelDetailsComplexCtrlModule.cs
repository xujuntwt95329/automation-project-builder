﻿using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelDetailsComplexCtrlModule : ViewModelDetailsBase
    {
        private IDialogService _dialogService;
        private IDataService _dataService;

        private ICommand _cmdAddFunction;
        private ICommand _cmdDeleteFunction;

        public ICommand CmdAddFunction
        {
            get { return _cmdAddFunction; }
        }

        public ICommand CmdDeleteFunction
        {
            get { return _cmdDeleteFunction; }
        }

        public ObservableCollection<ModuleFunction> ModuleFunctions { get; set; } = new ObservableCollection<ModuleFunction>();
                       
        public ViewModelDetailsComplexCtrlModule(Guid moduleId, IDialogService dialogService,IDataService dataService)
        {
            _dialogService = dialogService;
            _dataService = dataService;

            ViewModuleId = moduleId;
            ViewModuleType = ModuleType.ComplexCtrlModule;

            ModuleFunctions = dataService.GetFunctions(moduleId);

            _cmdAddFunction = new DelegateCommand(x => AddFunction());
            _cmdDeleteFunction = new DelegateCommand(x => DeleteFunction());
        }

        private void AddFunction()
        {
            var dialog = new ViewModelDialogTextInput("");

            var result = _dialogService.ShowDialog(dialog);

            if (result.Value)
            {
                var moduleFunction = new ModuleFunction(ViewModuleId, dialog.TextInput);

                _dataService.AddFunction(moduleFunction);

                ModuleFunctions.Add(moduleFunction);
            }
        }

        private void DeleteFunction()
        {

        }
    }
}
