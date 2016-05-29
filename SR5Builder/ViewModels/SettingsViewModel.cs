using DrWPF.Windows.Data;
using SR5Builder.Prototypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SR5Builder.ViewModels
{
    public class Setting//: IEditableObject
    {
        private string oldKey;
        public string Key { get; private set; }
        private object oldValue;
        public object Value {
            get;
            set;
        }

        public Setting(string key, object value)
        {
            Key = key;
            Value = value;
        }

        //public void BeginEdit()
        //{
        //    oldKey = Key;
        //    oldValue = Value;
        //}

        //public void EndEdit()
        //{
            
        //}

        //public void CancelEdit()
        //{
        //    Key = oldKey;
        //    Value = oldValue;
        //}
    }

    public class SettingsViewModel: ViewModelBase
    {
        public ObservableDictionary<string, SettingsPrototype> SavedSettings{ get; set; }

        private string mSelectedSettings;
        public string SelectedSettings
        {
            get { return mSelectedSettings; }
            set
            {
                if (mSelectedSettings != value)
                {
                    mSelectedSettings = value;
                    OnSelectionChanged();
                    OnPropertyChanged(nameof(CurrentSettings));
                }
            }
        }

        public ObservableCollection<Setting> CurrentSettings { get; set; }

        public SettingsViewModel()
        {
            SavedSettings = new ObservableDictionary<string, SettingsPrototype>(GlobalData.GenSettingsList);
            mSelectedSettings = GlobalData.GenSettingsList.First().Key;
            OnSelectionChanged();
        }

        private void OnSelectionChanged()
        {
            if (mSelectedSettings != null)
            {
                CurrentSettings = new ObservableCollection<Setting>
                    (from kvp in SavedSettings[mSelectedSettings].Properties
                     select new Setting(kvp.Key, kvp.Value));
            }
        }
    }
}
