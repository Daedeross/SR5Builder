using DrWPF.Windows.Data;
using SR5Builder.Loaders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SR5Builder.ViewModels
{
    public class SettingsViewModel: ViewModelBase
    {
        public ObservableDictionary<string, SettingsLoader> SavedSettings{ get; set; }

        private SettingsLoader mCurrentSettings;
        public SettingsLoader CurrentSettings
        {
            get { return mCurrentSettings; }
            set
            {
                if (mCurrentSettings != value)
                {
                    mCurrentSettings = value;
                    OnPropertyChanged("CurrentSettings");
                }
            }
        }

        public SettingsViewModel()
        {
            SavedSettings = new ObservableDictionary<string, SettingsLoader>(GlobalData.GenSettingsList);
            mCurrentSettings = GlobalData.GenSettingsList.First().Value;
            object obj = mCurrentSettings.Properties.First().Value;
        }
    }
}
