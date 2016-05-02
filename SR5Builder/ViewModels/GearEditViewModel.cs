using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.DataModels;
using SR5Builder.Loaders;
using DrWPF.Windows.Data;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace SR5Builder.ViewModels
{
    public delegate void EditingDoneEventHandler(object sender, EventArgs e);

    public class GearEditViewModel : ViewModelBase
    {
        #region Private Fields

        private Gear gear;

        #endregion // Private fields

        #region Properties

        public string Name
        {
            get { return gear.Name; }
        }

        public string Capacity
        {
            get
            {
                if (gear.Capacity == 0)
                {
                    return "—";
                }
                else return gear.CapacityUsed + "/" + gear.Capacity;
            }
        }

        public decimal Cost
        {
            get
            {
                return gear.Cost;
            }
        }

        public string DisplayCost
        {
            get { return Cost.ToString("C", GlobalData.CostFormat); }
        }

        public ObservableCollection<GearModLoader> AvailableMods { get; set; }

        public ObservableDictionary<string, GearModLoader> ModList { get; set; }

        public GearModLoader SelectedNewMod { get; set; }

        public GearModLoader SelectedMod { get; set; }

        #endregion // Properties

        #region Events

        public event EditingDoneEventHandler EditingDone;

        #endregion // Events

        #region Constructors

        public GearEditViewModel(Gear gear)
        {
            this.gear = gear;
            gear.PropertyChanged += this.BubblePropertyChanged;
            CreateModList();
        }

        ~GearEditViewModel()
        {
            Detatch();
        }

        #endregion // Constructors

        #region Commands

            #region AddMod

        private RelayCommand mAddModCommand;

        public ICommand AddModCommand
        {
            get
            {
                if (mAddModCommand == null)
                {
                    mAddModCommand = new RelayCommand(p => this.AddModExecute(), p => this.AddModCanExecute());
                }
                return mAddModCommand;
            }
        }

        private void AddModExecute()
        {
            ModList.Add(SelectedNewMod.Name, SelectedNewMod);
            SelectedMod = SelectedNewMod;
        }

        private bool AddModCanExecute()
        {
            if (SelectedNewMod == null)
                return false;
            if(ModList.ContainsKey(SelectedNewMod.Name))
                return false;

            foreach (string taboo in SelectedNewMod.Taboo)
                if (ModList.ContainsKey(taboo))
                    return false;

            return true;
        }

            #endregion // AddMod

            #region RemoveMod

        private RelayCommand mRemoveModCommand;

        public ICommand RemoveModCommand
        {
            get
            {
                if (mRemoveModCommand == null)
                {
                    mRemoveModCommand = new RelayCommand(p => this.RemoveModExecute(), p => this.RemoveModCanExecute());
                }
                return mRemoveModCommand;
            }
        }

        private void RemoveModExecute()
        {
            ModList.Remove(SelectedMod.Name);
            if (ModList.Count == 0)
                SelectedMod = null;
            else
                SelectedMod = ModList.Last().Value;
        }

        private bool RemoveModCanExecute()
        {
            return (SelectedMod != null);
        }

            #endregion // RemoveMod

            #region Done

        private RelayCommand mDoneCommand;

        public ICommand DoneCommand
        {
            get
            {
                if (mDoneCommand == null)
                {
                    mDoneCommand = new RelayCommand(p => this.DoneExecute(p), p => this.DoneCanExecute());
                }
                return mDoneCommand;
            }
        }

        private void DoneExecute(object p)
        {
            if (p.ToString() == "submit")
            {
                var oldMods = from mod in gear.Mods.Keys
                              where !ModList.ContainsKey(mod)
                              select mod;
                var newMods = from mod in ModList.Keys
                              where !gear.Mods.ContainsKey(mod)
                              select mod;
                foreach (var oldMod in oldMods)
                {
                    gear.Mods.Remove(oldMod);
                }
                foreach (var newMod in newMods)
                {
                    gear.Mods.Add(newMod, new GearMod(gear, ModList[newMod]));
                }
            }
            gear = null;
            EditingDone?.Invoke(this, EventArgs.Empty);
        }

        private bool DoneCanExecute()
        {
            return true;
        }

            #endregion // Done

        #endregion // Commands

        #region Private Methods

        private void CreateModList()
        {
            // add existing mods to tmp collection
            ModList = new ObservableDictionary<string, GearModLoader>(
                                    (from mod in gear.Mods.Keys
                                     select mod).ToDictionary(x => x, x => GlobalData.GearMods[x]));

            // add available mods
            AvailableMods = new ObservableCollection<GearModLoader>();
            // add specific allowed mods
            foreach(string key in gear.AvailableMods)
            {
                if (!ModList.ContainsKey(key))
                {
                    GearModLoader gml;
                    if (GlobalData.GearMods.TryGetValue(key, out gml))
                    {
                        AvailableMods.Add(gml);
                    }
                    else
                        SR5Builder.Helpers.Log.LogMessage("Failed to add GearMod: '" + key + ".' item not found in resources.");
                }
            }
            // add mods by sub-category
            foreach (string cat in gear.ModCategories)
            {
                Dictionary<string, GearModLoader> modCat;
                if (GlobalData.GearModCategories.TryGetValue(cat, out modCat))
                {
                    foreach (var key in modCat.Keys)
                    {
                        if (!ModList.ContainsKey(key))
                        {
                            AvailableMods.Add(modCat[key]);
                        }
                    }
                }
            }
        }

        #endregion // Private Methods

        #region Public Methods

        public void Detatch()
        {
            if (gear != null)
            {
                gear.PropertyChanged -= this.BubblePropertyChanged;
                gear = null;
            }
        }

        #endregion // Public Methods
    }
}
