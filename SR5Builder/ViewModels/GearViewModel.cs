﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using DrWPF.Windows.Data;
using SR5Builder.DataModels;
using SR5Builder.Prototypes;

namespace SR5Builder.ViewModels
{
    public class GearViewModel: ViewModelBase
    {
        #region Private Fields

        SR5Character character;

        #endregion // Private fields

        #region Properties

            #region Gear

        public ObservableCollection<string> GearCategories { get; set; }

        protected string mSelectedGearCategory;
        public string SelectedGearCategory
        {
            get { return mSelectedGearCategory; }
            set
            {
                if (value != mSelectedGearCategory)
                {
                    Dictionary<string, GearPrototype> dict;

                    if (GlobalData.Gear.TryGetValue(value, out dict))
                    {
                        mSelectedGearCategory = value;
                        AvailableGear = new ObservableDictionary<string, GearPrototype>(dict);
                    }
                    OnPropertyChanged(nameof(SelectedGearCategory));
                    OnPropertyChanged(nameof(AvailableGear));
                }
            }
        }

        public ObservableDictionary<string, GearPrototype> AvailableGear { get; set; }

        public GearPrototype SelectedNewGear { get; set; }

        public ObservableDictionary<string, Gear> GearList
        {
            get { return character.GearList; }
            set { character.GearList = value; }
        }

        public Gear SelectedGear { get; set; }

            #endregion // Gear

            #region Implants

        public ObservableCollection<string> ImplantCategories { get; set; }

        protected string mSelectedImplantCategory;
        public string SelectedImplantCategory
        {
            get { return mSelectedImplantCategory; }
            set
            {
                if (value != mSelectedImplantCategory)
                {
                    Dictionary<string, ImplantPrototype> dict;

                    if (GlobalData.Implants.TryGetValue(value, out dict))
                    {
                        mSelectedImplantCategory = value;
                        AvailableImplants = new ObservableDictionary<string, ImplantPrototype>(dict);
                    }
                    OnPropertyChanged(nameof(SelectedImplantCategory));
                    OnPropertyChanged(nameof(AvailableImplants));
                }
            }
        }

        public ObservableDictionary<string, ImplantPrototype> AvailableImplants { get; set; }

        public ImplantPrototype SelectedNewImplant { get; set; }

        public ObservableDictionary<string, Implant> ImplantList
        {
            get { return character.ImplantList; }
            set { character.ImplantList = value; }
        }

        public Implant SelectedImplant { get; set; }

            #endregion // Implants

            #region ViewModels

        public string SelectedTab { get; set; }

        public GearEditViewModel GearEditVM { get; set; }

        protected bool mEditing = false;
        public bool Editing
        {
            get { return mEditing; }
            set
            {
                if (value != mEditing)
                {
                    if (GearEditVM != null)
                        GearEditVM.EditingDone -= this.EditingDone;

                    GearEditVM = null;

                    mEditing = value;
                    if (mEditing)
                    {
                        if (SelectedTab == "Gear" && SelectedGear != null)
                        {
                            GearEditVM = new GearEditViewModel(SelectedGear);
                            GearEditVM.EditingDone += this.EditingDone;
                        }
                        else if(SelectedTab == "Augmentations" && SelectedImplant != null)
                        {
                            GearEditVM = new GearEditViewModel(SelectedImplant);
                            GearEditVM.EditingDone += this.EditingDone;
                        }
                        else
                            mEditing = false;
                    }

                    OnPropertyChanged(nameof(GearEditVM));
                    OnPropertyChanged(nameof(Editing));
                    OnPropertyChanged(nameof(EditVisible));
                }
            }
        }

        public Visibility EditVisible
        {
            get
            {
                if (mEditing)
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

            #endregion //ViewModels

        #endregion // Properties

        #region Constructors

        public GearViewModel(SR5Character owner)
        {
            character = owner;
            Initialize();
        }

        private void Initialize()
        {
            GearCategories = new ObservableCollection<string>(GlobalData.Gear.Keys);
            AvailableGear = new ObservableDictionary<string, GearPrototype>();
            ImplantCategories = new ObservableCollection<string>(GlobalData.Implants.Keys);
            AvailableImplants = new ObservableDictionary<string, ImplantPrototype>();
            Editing = false;
        }

        #endregion // Constructors

        #region Commands

            #region AddGear

        private RelayCommand mAddGearCommand;

        public ICommand AddGearCommand
        {
            get
            {
                if (mAddGearCommand == null)
                {
                    mAddGearCommand = new RelayCommand(p => this.AddGearExecute(), p => this.AddGearCanExecute());
                }
                return mAddGearCommand;
            }
        }

        private void AddGearExecute()
        {
            GearList.Add(SelectedNewGear.Name, SelectedNewGear.ToGear(character));
            SelectedGear = GearList[SelectedNewGear.Name];
            OnPropertyChanged(nameof(SelectedGear));
        }

        private bool AddGearCanExecute()
        {
            return (SelectedNewGear != null && !GearList.ContainsKey(SelectedNewGear.Name));
        }

            #endregion // AddGear

            #region RemoveGear

        private RelayCommand mRemoveGearCommand;

        public ICommand RemoveGearCommand
        {
            get
            {
                if (mRemoveGearCommand == null)
                {
                    mRemoveGearCommand = new RelayCommand(p => this.RemoveGearExecute(), p => this.RemoveGearCanExecute());
                }
                return mRemoveGearCommand;
            }
        }

        private void RemoveGearExecute()
        {
            GearList.Remove(SelectedGear.Name);
            if (GearList.Count == 0)
                SelectedGear = null;
            else
                SelectedGear = GearList.Last().Value;

            OnPropertyChanged(nameof(SelectedGear));
        }

        private bool RemoveGearCanExecute()
        {
            return (SelectedGear != null);
        }

            #endregion // RemoveGear

            #region IncreaseGear

        private RelayCommand mIncreaseGearCommand;
        public ICommand IncreaseGearCommand
        {
            get
            {
                if (mIncreaseGearCommand == null)
                {
                    mIncreaseGearCommand = new RelayCommand(
                                                       p => this.IncreaseGearExecute(),
                                                       p => this.IncreaseGearCanExecute());
                }
                return mIncreaseGearCommand;
            }
        }

        private void IncreaseGearExecute()
        {
            SelectedGear.BaseRating++;
        }

        private bool IncreaseGearCanExecute()
        {
            return (GearList != null &&
                    GearList.Count > 0 &&
                    SelectedGear != null &&
                    SelectedGear.BaseRating < SelectedGear.Max);
        }

            #endregion // IncreaseGear

            #region DecreaseGear

        private RelayCommand mDecreaseGearCommand;
        public ICommand DecreaseGearCommand
        {
            get
            {
                if (mDecreaseGearCommand == null)
                {
                    mDecreaseGearCommand = new RelayCommand(
                                                       p => this.DecreaseGearExecute(),
                                                       p => this.DecreaseGearCanExecute());
                }
                return mDecreaseGearCommand;
            }
        }

        private void DecreaseGearExecute()
        {
            SelectedGear.BaseRating--;
        }

        private bool DecreaseGearCanExecute()
        {
            return (GearList != null &&
                    GearList.Count > 0 &&
                    SelectedGear != null &&
                    SelectedGear.BaseRating > SelectedGear.Min);
        }

            #endregion // DecreaseGear

            #region AddImplant

        private RelayCommand mAddImplantCommand;

        public ICommand AddImplantCommand
        {
            get
            {
                if (mAddImplantCommand == null)
                {
                    mAddImplantCommand = new RelayCommand(p => this.AddImplantExecute(), p => this.AddImplantCanExecute());
                }
                return mAddImplantCommand;
            }
        }

        private void AddImplantExecute()
        {
            ImplantList.Add(SelectedNewImplant.Name, new Implant(character, SelectedNewImplant));
            SelectedImplant = ImplantList[SelectedNewImplant.Name];
            OnPropertyChanged(nameof(SelectedImplant));
        }

        private bool AddImplantCanExecute()
        {
            return (SelectedNewImplant != null && !ImplantList.ContainsKey(SelectedNewImplant.Name));
        }

            #endregion // AddImplant

            #region RemoveImplant

        private RelayCommand mRemoveImplantCommand;

        public ICommand RemoveImplantCommand
        {
            get
            {
                if (mRemoveImplantCommand == null)
                {
                    mRemoveImplantCommand = new RelayCommand(p => this.RemoveImplantExecute(), p => this.RemoveImplantCanExecute());
                }
                return mRemoveImplantCommand;
            }
        }

        private void RemoveImplantExecute()
        {
            ImplantList.Remove(SelectedImplant.Name);
            if (ImplantList.Count == 0)
                SelectedImplant = null;
            else
                SelectedImplant = ImplantList.Last().Value;

            OnPropertyChanged(nameof(SelectedImplant));
        }

        private bool RemoveImplantCanExecute()
        {
            return (SelectedImplant != null);
        }

            #endregion // RemoveImplant

            #region IncreaseImplant

        private RelayCommand mIncreaseImplantCommand;
        public ICommand IncreaseImplantCommand
        {
            get
            {
                if (mIncreaseImplantCommand == null)
                {
                    mIncreaseImplantCommand = new RelayCommand(
                                                       p => this.IncreaseImplantExecute(),
                                                       p => this.IncreaseImplantCanExecute());
                }
                return mIncreaseImplantCommand;
            }
        }

        private void IncreaseImplantExecute()
        {
            SelectedImplant.BaseRating++;
        }

        private bool IncreaseImplantCanExecute()
        {
            return (ImplantList != null &&
                    ImplantList.Count > 0 &&
                    SelectedImplant != null &&
                    SelectedImplant.BaseRating < SelectedImplant.Max);
        }

            #endregion // IncreaseImplant

            #region DecreaseImplant

        private RelayCommand mDecreaseImplantCommand;
        public ICommand DecreaseImplantCommand
        {
            get
            {
                if (mDecreaseImplantCommand == null)
                {
                    mDecreaseImplantCommand = new RelayCommand(
                                                       p => this.DecreaseImplantExecute(),
                                                       p => this.DecreaseImplantCanExecute());
                }
                return mDecreaseImplantCommand;
            }
        }

        private void DecreaseImplantExecute()
        {
            SelectedImplant.BaseRating--;
        }

        private bool DecreaseImplantCanExecute()
        {
            return (ImplantList != null &&
                    ImplantList.Count > 0 &&
                    SelectedImplant != null &&
                    SelectedImplant.BaseRating > SelectedImplant.Min);
        }

            #endregion // DecreaseImplant

            #region LaunchEdit

        private RelayCommand mLaunchEditCommand;

        public ICommand LaunchEditCommand
        {
            get
            {
                if (mLaunchEditCommand == null)
                {
                    mLaunchEditCommand = new RelayCommand(p => this.LaunchEditExecute(), p => this.LaunchEditCanExecute());
                }
                return mLaunchEditCommand;
            }
        }

        private void LaunchEditExecute()
        {
            if (SelectedTab == "Gear")
                GearEditVM = new GearEditViewModel(this.SelectedGear);
            else // if (SelectedTab == "Augmentations")
                GearEditVM = new GearEditViewModel(this.SelectedImplant);

            Editing = true;
        }

        private bool LaunchEditCanExecute()
        {
            if (SelectedTab == "Gear" && SelectedGear != null && (SelectedGear.AvailableMods.Count != 0
                                                                  || SelectedGear.ModCategories.Count != 0))
                return true;

            if (SelectedTab == "Augmentations" && SelectedImplant != null && (SelectedImplant.AvailableMods.Count != 0
                                                                              || SelectedImplant.ModCategories.Count != 0))
                return true;

            return false;
        }

            #endregion // LaunchEdit

        #endregion // Commands

        #region Private Methods

        private void EditingDone(object sender, EventArgs e)
        {
            Editing = false;
            GearEditVM = null;
        }

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}
