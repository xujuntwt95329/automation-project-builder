﻿using AutomationProjectBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutomationProjectBuilder.Misc
{
    public interface IDataService
    {
        public ObservableCollection<ModuleFunction> GetFunctions(Guid ItemId);
        public void AddFunction(ModuleFunction function);
        public void DeleteListItem(IListItem function);

        public ProjectModule GetProjectRoot();
        public ProjectModule ResetProjectRoot();
        public void UpdateModule(ProjectModule item);

        public void SetParameters(Guid ModuleId, List<ModuleParameter> parameters);
        public List<ModuleParameter> GetParameters(Guid ItemId);
        public List<ParameterGroup> GetParameterGroups();

        public bool Save();
        public void SaveAs(string filePath);

        public void Load();
        public void Open(string filePath);
    }
}
