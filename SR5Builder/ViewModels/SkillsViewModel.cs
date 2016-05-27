﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

using SR5Builder.DataModels;
using Helpers;
using System.Windows.Input;
using System.Diagnostics;
using SR5Builder.Dialogs;
using DrWPF.Windows.Data;
using SR5Builder.Prototypes;

namespace SR5Builder.ViewModels
{
    public class SkillsViewModel : ViewModelBase
    {
        #region Private Fields

        SR5Character character;

        #endregion // Private fields

        #region Properties

        private List<string> mCategoryList;
        public List<string> CategoryList
        {
            get
            {
                if (mCategoryList == null)
                {
                    mCategoryList = GlobalData.PreLoadedSkills.Keys.ToList();
                    if (character.SpecialKind != SpecialKind.Magic)
                        mCategoryList.Remove("Magical");
                    if (character.SpecialKind != SpecialKind.Resonance)
                        mCategoryList.Remove("Resonance");
                }
                return mCategoryList;
            }
        }

        private string mSelectedCategory;
        public string SelectedCategory
        {
            get { return mSelectedCategory; }
            set
            {
                if (value != mSelectedCategory)
                {
                    mSelectedCategory = value;
                    CreateAvailableSkills();
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        public ObservableCollection<SkillPrototype> AvailableSkills { get; private set; }

        public SkillPrototype SelectedNewSkill { get; set; }

        public ObservableDictionary<string, Skill> SkillsList
        {
            get { return character.SkillList; }
            set { character.SkillList = value; }
        }

        public Skill SelectedSkill { get; set; }

        public ObservableCollection<SkillGroupPrototype> AvailableSkillGroups { get; private set; }

        public SkillGroupPrototype SelectedNewSkillGroup { get; set; }

        public ObservableDictionary<string, SkillGroup> SkillGroupsList
        {
            get { return character.SkillGroupsList; }
            set { character.SkillGroupsList = value; }
        }

        public SkillGroup SelectedSkillGroup { get; set; }

        public int SkillPoints
        {
            get
            {
                return GlobalData.PriorityLevels[character.Priorities.Skills].SkillPoints;
            }
        }

        public int SkillPointsSpent { get { return character.SkillPointsSpent; } }

        public int SkillGroupPoints
        {
            get
            {
                return GlobalData.PriorityLevels[character.Priorities.Skills].SkillGroupPoints;
            }
        }

        public int SkillGroupPointsSpent { get { return character.SkillGroupPointsSpent; } }

        public bool PointsVisible
        {
            get
            {
                return character.Settings.Method == CharGenMethod.Priority
                    || character.Settings.Method == CharGenMethod.SumToTen;
            }
        }

        #endregion // Properties

        #region Constructors

        public SkillsViewModel(SR5Character c)
        {
            character = c;
            c.PropertyChanged += this.OnCharacterChanged;
            AvailableSkills = new ObservableCollection<SkillPrototype>();
            CreateAvailableGroups();
        }

        #endregion // Constructors

        #region Commands

            #region AddSkill

        private RelayCommand mAddSkillCommand;
        public ICommand AddSkillCommand
        {
            get
            {
                if (mAddSkillCommand == null)
                {
                    mAddSkillCommand = new RelayCommand(p => this.AddSkillExecute(), p => this.AddSkillCanExecute());
                }
                return mAddSkillCommand;
            }
        }

        private void AddSkillExecute()
        {
            SkillGroup grp = null;

            foreach (SkillGroup sg in SkillGroupsList.Values)
            {
                if (sg.Skills.ContainsKey(SelectedNewSkill.Name))
                {
                    grp = sg;
                    break;
                }
            }

            if (grp != null)
            {
                OkCancelDialog dlg = new OkCancelDialog();
                dlg.DataContext = new SkillAddViewModel(grp.Name, SelectedNewSkill.Name);
                bool? result = dlg.ShowDialog();

                if (result == true)
                {
                    SkillGroupsList.Remove(grp.Name);
                    SkillsList.Add(SelectedNewSkill.Name, SelectedNewSkill.NewSkill(character));
                }
            }
            else
                SkillsList.Add(SelectedNewSkill.Name, SelectedNewSkill.NewSkill(character));
        }

        private bool AddSkillCanExecute()
        {
            return (SelectedNewSkill != null && !SkillsList.ContainsKey(SelectedNewSkill.Name));
        }

            #endregion

            #region RemoveSkill

        private RelayCommand mRemoveSkillCommand;
        public ICommand RemoveSkillCommand
        {
            get
            {
                if (mRemoveSkillCommand == null)
                {
                    mRemoveSkillCommand = new RelayCommand(p => this.RemoveSkillExecute(), p => this.RemoveSkillCanExecute());
                }
                return mRemoveSkillCommand;
            }
        }

        private void RemoveSkillExecute()
        {
            SkillsList.Remove(SelectedSkill.Name);
        }

        private bool RemoveSkillCanExecute()
        {
            return (SelectedSkill != null);
        }

            #endregion // Remove Skill

            #region IncreaseSkill

        private RelayCommand mIncreaseSkillCommand;
        public ICommand IncreaseSkillCommand
        {
            get
            {
                if(mIncreaseSkillCommand == null)
                {
                    mIncreaseSkillCommand = new RelayCommand(
                                                       p => this.IncreaseSkillExecute(),
                                                       p => this.IncreaseSkillCanExecute());
                }
                return mIncreaseSkillCommand;
            }
        }

        private void IncreaseSkillExecute()
        {
            SelectedSkill.BaseRating++;
        }

        private bool IncreaseSkillCanExecute()
        {
            return (SkillsList != null &&
                    SkillsList.Count > 0 &&
                    SelectedSkill != null &&
                    SelectedSkill.BaseRating < 6);
        }

            #endregion // IncreaseSkill

            #region DecreaseSkill

        private RelayCommand mDecreaseSkillCommand;
        public ICommand DecreaseSkillCommand
        {
            get
            {
                if (mDecreaseSkillCommand == null)
                {
                    mDecreaseSkillCommand = new RelayCommand(
                                                       p => this.DecreaseSkillExecute(),
                                                       p => this.DecreaseSkillCanExecute());
                }
                return mDecreaseSkillCommand;
            }
        }

        private void DecreaseSkillExecute()
        {
            SelectedSkill.BaseRating--;
        }

        private bool DecreaseSkillCanExecute()
        {
            return (SkillsList != null &&
                    SkillsList.Count > 0 &&
                    SelectedSkill != null &&
                    SelectedSkill.BaseRating > 0);
        }

            #endregion // DecreaseSkill

            #region AddSkillGroup

        private RelayCommand mAddSkillGroupCommand;
        public ICommand AddSkillGroupCommand
        {
            get
            {
                if (mAddSkillGroupCommand == null)
                {
                    mAddSkillGroupCommand = new RelayCommand(p => this.AddSkillGroupExecute(), p => this.AddSkillGroupCanExecute());
                }
                return mAddSkillGroupCommand;
            }
        }

        private void AddSkillGroupExecute()
        {
            Skill[] skills = (from sk in character.SkillList.Values
                              where SelectedNewSkillGroup.SkillNames.Contains(sk.Name)
                              select sk).ToArray();
            

            if (skills.Length != 0)
            {

                string[] skNames = (from sk in skills
                                    select sk.Name).ToArray();

                OkCancelDialog dlg = new OkCancelDialog();
                dlg.DataContext = new SkillGroupAddViewModel(SelectedNewSkillGroup.Name, skNames);
                var result = dlg.ShowDialog();

                if (result == true)
                {
                    foreach (string name in skNames)
                    {
                        SkillsList.Remove(name);
                    }
                    SkillGroupsList.Add(SelectedNewSkillGroup.Name ,SelectedNewSkillGroup.NewSkillGroup(character));
                }
            }
            else
                SkillGroupsList.Add(SelectedNewSkillGroup.Name, SelectedNewSkillGroup.NewSkillGroup(character));
        }

        private bool AddSkillGroupCanExecute()
        {
            return (SelectedNewSkillGroup != null);// &&
                    //!(from sg in SkillGroupsList
                      //select sg.Name).Contains(SelectedNewSkillGroup.Name));
        }

            #endregion // AddSkillGroup

            #region RemoveSkillGroup

        private RelayCommand mRemoveSkillGroupCommand;
        public ICommand RemoveSkillGroupCommand
        {
            get
            {
                if (mRemoveSkillGroupCommand == null)
                {
                    mRemoveSkillGroupCommand = new RelayCommand(p => this.RemoveSkillGroupExecute(), p => this.RemoveSkillGroupCanExecute());
                }
                return mRemoveSkillGroupCommand;
            }
        }

        private void RemoveSkillGroupExecute()
        {
            SkillGroupsList.Remove(SelectedSkillGroup.Name);
        }

        private bool RemoveSkillGroupCanExecute()
        {
            return (SelectedSkillGroup != null);
        }

            #endregion // Remove SkillGroup

            #region IncreaseSkillGroup

        private RelayCommand mIncreaseSkillGroupCommand;
        public ICommand IncreaseSkillGroupCommand
        {
            get
            {
                if (mIncreaseSkillGroupCommand == null)
                {
                    mIncreaseSkillGroupCommand = new RelayCommand(
                                                       p => this.IncreaseSkillGroupExecute(),
                                                       p => this.IncreaseSkillGroupCanExecute());
                }
                return mIncreaseSkillGroupCommand;
            }
        }

        private void IncreaseSkillGroupExecute()
        {
            SelectedSkillGroup.BaseRating++;
        }

        private bool IncreaseSkillGroupCanExecute()
        {
            return (SkillGroupsList != null &&
                    SkillGroupsList.Count > 0 &&
                    SelectedSkillGroup != null &&
                    SelectedSkillGroup.BaseRating < 6);
        }

            #endregion // IncreaseSkillGroup

            #region DecreaseSkillGroup

        private RelayCommand mDecreaseSkillGroupCommand;
        public ICommand DecreaseSkillGroupCommand
        {
            get
            {
                if (mDecreaseSkillGroupCommand == null)
                {
                    mDecreaseSkillGroupCommand = new RelayCommand(
                                                       p => this.DecreaseSkillGroupExecute(),
                                                       p => this.DecreaseSkillGroupCanExecute());
                }
                return mDecreaseSkillGroupCommand;
            }
        }

        private void DecreaseSkillGroupExecute()
        {
            SelectedSkillGroup.BaseRating--;
        }

        private bool DecreaseSkillGroupCanExecute()
        {
            return (SkillGroupsList != null &&
                    SkillGroupsList.Count > 0 &&
                    SelectedSkillGroup != null &&
                    SelectedSkillGroup.BaseRating > 0);
        }

            #endregion // DecreaseSkillGroup

        #endregion // Commands

        #region Private Methods

        private void OnCharacterChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SpecialKind":
                    mCategoryList = GlobalData.PreLoadedSkills.Keys.ToList();
                    if (character.SpecialKind != SpecialKind.Magic)
                        mCategoryList.Remove("Magical");
                    if (character.SpecialKind != SpecialKind.Resonance)
                        mCategoryList.Remove("Resonance");
                    CreateAvailableGroups();
                    OnPropertyChanged(nameof(CategoryList));
                    break;
                case "Priorities":
                    OnPropertyChanged(nameof(SkillPoints));
                    OnPropertyChanged(nameof(SkillGroupPoints));
                    break;
                case "SkillPoints":
                case "SkillPointsSpent":
                case "SkillGroupPoints":
                case "SkillGroupPointsSpent":
                    OnPropertyChanged(e.PropertyName);
                    break;
                default:
                    break;
            }
        }

        private void CreateAvailableSkills()
        {
            if (mSelectedCategory == null || mSelectedCategory == "")
                AvailableSkills.Clear();
            else if (mSelectedCategory == "All")
            {
                List<SkillPrototype> tmpList = new List<SkillPrototype>();
                foreach (string cat in CategoryList)
                {
                    if (cat != "All")
                    {
                        tmpList = tmpList.Concat(GlobalData.PreLoadedSkills[cat]).ToList();
                    }
                }
                AvailableSkills = new ObservableCollection<SkillPrototype>(tmpList);
            }
            else
                AvailableSkills = new ObservableCollection<SkillPrototype>(
                    GlobalData.PreLoadedSkills[SelectedCategory]);
            OnPropertyChanged(nameof(AvailableSkills));
        }

        private void CreateAvailableGroups()
        {
            List<SkillGroupPrototype> list = GlobalData.PreLoadedSkillGroups;
            if (character.SpecialKind != SpecialKind.Magic)
            {
                list = (from sg in list
                       where (sg.Name != "Conjuring" && sg.Name != "Sorcery" && sg.Name != "Enchanting")
                       select sg).ToList();
            }
            if (character.SpecialKind != SpecialKind.Resonance)
            {
                list = (from sg in list
                        where sg.Name != "Tasking"
                        select sg).ToList();
            }
            AvailableSkillGroups = new ObservableCollection<SkillGroupPrototype>(list);
            OnPropertyChanged(nameof(AvailableSkillGroups));
        }

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}