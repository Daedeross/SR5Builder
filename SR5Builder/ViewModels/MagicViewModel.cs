using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using SR5Builder.DataModels;
using System.Windows.Input;

using DrWPF.Windows.Data;

using SR5Builder.Helpers;
using SR5Builder.Dialogs;
using SR5Builder.Loaders;

namespace SR5Builder.ViewModels
{
    public class MagicViewModel : ViewModelBase
    {
        #region Private Fields

        SR5Character character;

        #endregion // Private fields

        #region Properties

            #region Spells

        public List<string> SpellCategories { get; set; }

        private string mSelectedCategory;

        public string SelectedCategory
        {
            get { return mSelectedCategory; }
            set
            {
                if (mSelectedCategory != value)
                {
                    mSelectedCategory = value;
                    AvailableSpells = new ObservableCollection<SpellLoader>(GlobalData.PreLoadedSpells[mSelectedCategory]);
                    OnPropertyChanged("AvailableSpells");
                    OnPropertyChanged("SelectedCategory");
                }
            }
        }
        
        public ObservableCollection<SpellLoader> AvailableSpells { get; set; }

        public SpellLoader SelectedNewSpell { get; set; }

        public ObservableDictionary<string, Spell> SpellList
        {
            get { return character.SpellList; }
            set
            {
                character.SpellList = value;
            }
        }

        public Spell SelectedSpell { get; set; }

            #endregion Spells

            #region Powers

        public ObservableCollection<AdeptPowerLoader> AvailablePowers { get; set; }

        public AdeptPowerLoader SelectedNewPower { get; set; }

        public ObservableDictionary<string, AdeptPower> PowerList
        {
            get { return character.PowerList; }
            set
            {
                character.PowerList = value;
            }
        }

        public AdeptPower SelectedPower { get; set; }

        public decimal PowerPointsSpent { get { return character.PowerPointsSpent; } }

        public decimal PowerPoints { get { return 5; } }

        #endregion // Powers

        #endregion // Properties

        #region Constructors

        public MagicViewModel(SR5Character c)
        {
            character = c;
            SpellCategories = GlobalData.PreLoadedSpells.Keys.ToList();
            AvailablePowers = new ObservableCollection<AdeptPowerLoader>(GlobalData.PreLoadedPowers);
            character.PropertyChanged += this.BubblePropertyChanged;
        } 

        #endregion // Constructors

        #region Commands

            #region AddSpell

        private RelayCommand mAddSpellCommand;

        public ICommand AddSpellCommand
        {
            get
            {
                if (mAddSpellCommand == null)
                {
                    mAddSpellCommand = new RelayCommand(p => this.AddSpellExecute(), p => this.AddSpellCanExecute());
                }
                return mAddSpellCommand;
            }
        }

        private void AddSpellExecute()
        {
            if (SelectedNewSpell.ExtKind == null || SelectedNewSpell.ExtKind.Length == 0)
            {
                Spell sp = SelectedNewSpell.ToSpell(character);
                SpellList.Add(sp.Name, sp);
                SelectedSpell = SpellList[sp.Name];
            }
            else
            {
                // Open dialog
                SelectionViewModel vm = SelectionViewModel.CreateSelection(character, SelectedNewSpell);
                SelectionDialog dlg = new SelectionDialog();
                dlg.DataContext = vm;

                bool? result = dlg.ShowDialog();
                //vm.Selection = "foo";
                System.Diagnostics.Debug.WriteLine(vm.Selection);
                if (result == true)
                {
                    Spell sp = SelectedNewSpell.ToSpell(character, vm.Selection);
                    SpellList.Add(sp.Name, sp);
                    SelectedSpell = SpellList[sp.Name];
                }
            }
        }

        private bool AddSpellCanExecute()
        {
            return (SelectedNewSpell != null && !SpellList.ContainsKey(SelectedNewSpell.Name));
        }

            #endregion // AddSpell

            #region RemoveSpell

        private RelayCommand mRemoveSpellCommand;

        public ICommand RemoveSpellCommand
        {
            get
            {
                if (mRemoveSpellCommand == null)
                {
                    mRemoveSpellCommand = new RelayCommand(p => this.RemoveSpellExecute(), p => this.RemoveSpellCanExecute());
                }
                return mRemoveSpellCommand;
            }
        }

        private void RemoveSpellExecute()
        {
            SpellList.Remove(SelectedSpell.Name);
            SelectedSpell = SpellList.Count > 0 ? SpellList.Last().Value : null;
        }

        private bool RemoveSpellCanExecute()
        {
            return (SelectedSpell != null);
        }

            #endregion // RemoveSpell

            #region OpenPage

        private RelayCommand<Spell> mOpenPageCommand;

        public ICommand OpenPageCommand
        {
            get
            {
                if (mOpenPageCommand == null)
                {
                    mOpenPageCommand = new RelayCommand<Spell>(p => this.OpenPageExecute(p));
                }
                return mOpenPageCommand;
            }
        }

        private void OpenPageExecute(Spell s)
        {
            if (s != null)
            {
                string source = "";
                int page = s.Page;
                switch (s.Book)
                {
                    case "SR5":
                        source = Properties.Settings.Default.CoreBookPDF;
                        page += Properties.Settings.Default.CoreBookPageOffset;
                        break;
                    default:
                        break;
                }

                Commands.OpenSourcePage(source, page);
            }
        }

        private bool OpenPageCanExecute()
        {
            return true;
        }

            #endregion // OpenPage

            #region AddPower

        private RelayCommand mAddPowerCommand;

        public ICommand AddPowerCommand
        {
            get
            {
                if (mAddPowerCommand == null)
                {
                    mAddPowerCommand = new RelayCommand(p => this.AddPowerExecute(), p => this.AddPowerCanExecute());
                }
                return mAddPowerCommand;
            }
        }

        private void AddPowerExecute()
        {
            //PowerList.Add(SelectedNewPower.Clone(character));
            if (SelectedNewPower.ExtKind == null || SelectedNewPower.ExtKind.Length == 0)
            {
                AdeptPower p = SelectedNewPower.ToPower(character, "");
                PowerList.Add(p.Name, p);
                SelectedPower = PowerList[p.Name];
            }
            else
            {
                // Open dialog
                SelectionViewModel vm = SelectionViewModel.CreateSelection(character, SelectedNewPower);
                SelectionDialog dlg = new SelectionDialog();
                dlg.DataContext = vm;

                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    AdeptPower p = SelectedNewPower.ToPower(character, vm.Selection);
                    PowerList.Add(p.Name, p);

                    SelectedPower = PowerList[p.Name];
                }
            }
        }

        private bool AddPowerCanExecute()
        {
            return (SelectedNewPower != null && !PowerList.ContainsKey(SelectedNewPower.Name));
        }
        
            #endregion // AddPower

            #region RemovePower

        private RelayCommand mRemovePowerCommand;

        public ICommand RemovePowerCommand
        {
            get
            {
                if (mRemovePowerCommand == null)
                {
                    mRemovePowerCommand = new RelayCommand(p => this.RemovePowerExecute(), p => this.RemovePowerCanExecute());
                }
                return mRemovePowerCommand;
            }
        }

        private void RemovePowerExecute()
        {
            PowerList.Remove(SelectedPower.Name);
            if (PowerList.Count > 0)
            {
                SelectedPower = PowerList.Last().Value;
            }
            else SelectedPower = null;
        }

        private bool RemovePowerCanExecute()
        {
            return (SelectedPower != null);
        }

            #endregion // RemovePower

            #region IncreasePower

        private RelayCommand mIncreasePowerCommand;

        public ICommand IncreasePowerCommand
        {
            get
            {
                if (mIncreasePowerCommand == null)
                {
                    mIncreasePowerCommand = new RelayCommand(p => this.IncreasePowerExecute(), p => this.IncreasePowerCanExecute());
                }
                return mIncreasePowerCommand;
            }
        }

        private bool IncreasePowerCanExecute()
        {
            return (SelectedPower != null && SelectedPower.BaseRating < SelectedPower.Max);
        }

        private void IncreasePowerExecute()
        {
            SelectedPower.BaseRating++;
        }

            #endregion // IncreasePower

            #region DecreasePower

        private RelayCommand mDecreasePowerCommand;

        public ICommand DecreasePowerCommand
        {
            get
            {
                if (mDecreasePowerCommand == null)
                {
                    mDecreasePowerCommand = new RelayCommand(p => this.DecreasePowerExecute(), p => this.DecreasePowerCanExecute());
                }
                return mDecreasePowerCommand;
            }
        }

        private bool DecreasePowerCanExecute()
        {
            return (SelectedPower != null && SelectedPower.BaseRating > SelectedPower.Min);
        }

        private void DecreasePowerExecute()
        {
            SelectedPower.BaseRating--;
        }

        #endregion // DecreasePower

        #endregion // Commands

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}