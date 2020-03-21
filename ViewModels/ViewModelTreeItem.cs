﻿using AutomationProjectBuilder.Misc;
using AutomationProjectBuilder.Model;
using AutomationProjectBuilder.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace AutomationProjectBuilder.ViewModels
{
    public class ViewModelTreeItem : ViewModelBase
    {
        private string _itemName;
        private ItemTypeISA88 _itemType;

        public Guid ItemId { get; set; }
        public ViewModelTreeItem Parent { get; set; }
        public bool IsSelected { get; set; }
        public ObservableCollection<ViewModelTreeItem> Subsystems { get; } = new ObservableCollection<ViewModelTreeItem>();

        public string ItemName
        {
            get
            {
                return _itemName;
            }
            set
            {
                _itemName = value;
                NotifyPropertChanged("ItemName");
            }
        }

        public ItemTypeISA88 ItemType
        {
            get
            {
                return _itemType;
            }
            set
            {
                _itemType = value;
                NotifyPropertChanged("ItemType");
            }
        }

        private ICommand _cmdAddSubsystem;
        private ICommand _cmdDeleteSubsystem;
        private ICommand _cmdEditSubsystem;

        private IDialogService _dialogService;

        public ICommand CmdAddSubsystem
        {
            get { return _cmdAddSubsystem; }
        }

        public ICommand CmdDeleteSubsystem
        {
            get { return _cmdDeleteSubsystem; }
        }

        public ICommand CmdEditSubsystem
        {
            get { return _cmdEditSubsystem; }
        }

        public ViewModelTreeItem()
        {
            // empty constructor
        }

        public ViewModelTreeItem(string name, ItemTypeISA88 itemType, IDialogService dialogService)
        {
            ItemId = Guid.NewGuid();
            ItemName = name;
            ItemType = itemType;

            _dialogService = dialogService;

            _cmdAddSubsystem = new DelegateCommand(x => CreateSubsystem());
            _cmdDeleteSubsystem = new DelegateCommand(x => DeleteSubsystem());
            _cmdEditSubsystem = new DelegateCommand(x => EditSubsystem());
        }

        public void CreateSubsystem()
        {
            var dialog = new ViewModelDialogTreeItem(new ViewModelTreeItem());

            var result = _dialogService.ShowDialog(dialog);         

            if(result.Value)
            {
                var subsystem = new ViewModelTreeItem(
                    dialog.ItemName, 
                    dialog.ItemTypeSelection,
                    _dialogService);
                AddSubsystem(subsystem);
            }       
        }

        public void AddSubsystem(ViewModelTreeItem subsystem)
        {
            subsystem.Parent = this;
            Subsystems.Add(subsystem);
        }

        public void DeleteSubsystem()
        {
            Parent.Subsystems.Remove(this);
        }

        public void EditSubsystem()
        {
            var dialog = new ViewModelDialogTreeItem(this);

            var result = _dialogService.ShowDialog(dialog);

            if (result.Value)
            {
                ItemName = dialog.ItemName;
                ItemType = dialog.ItemTypeSelection;
            }
        }

        public ViewModelTreeItem GetSelectedItem(ViewModelTreeItem treeitem)
        {          
            if(IsSelected)
            {
                treeitem = this;
            }
            else
            {
                foreach(ViewModelTreeItem item in Subsystems)
                {
                    treeitem = item.GetSelectedItem(treeitem);
                }
            }
            return treeitem;
        }
    }
}