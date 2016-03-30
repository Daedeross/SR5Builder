﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Helpers;
using SR5Builder.DataModels;

namespace SR5Builder.ViewModels
{
    public class Workspace : ViewModelBase
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        public ObservableCollection<CharacterViewModel> CharacterVMs { get; set; }

        private CharacterViewModel mSelectedCharacter;
        public CharacterViewModel SelectedCharacter
        {
            get { return mSelectedCharacter; }
            set
            {
                if (value != mSelectedCharacter)
                {
                    mSelectedCharacter = value;
                    OnPropertyChanged("SelectedCharacter");
                }
            }
        }

        #endregion // Properties

        #region Constructors

        public Workspace()
        {
            Initialize();
            CharacterVMs = new ObservableCollection<CharacterViewModel>();
        }

        #endregion // Constructors

        #region Commands

            #region AddNewCharacter

        private RelayCommand mAddNewCharacter;
        public ICommand AddNewCharacter
        {
            get
            {
                if (mAddNewCharacter == null)
                {
                    mAddNewCharacter = new RelayCommand(
                        parap => this.AddNewCharacterExecute(),
                        param => this.AddNewCharacterCanExecute()
                        );
                }
                return mAddNewCharacter;
            }
        }

        private void AddNewCharacterExecute()
        {
            CharacterViewModel vm = new CharacterViewModel();
            vm.DisplayName = "Character " + (CharacterVMs.Count + 1).ToString();
            CharacterVMs.Add(vm);
            SelectedCharacter = CharacterVMs.Last();
        }

        private bool AddNewCharacterCanExecute()
        {
            return true;
        }

            #endregion // AddNewCharacter

        #region ExamineCharacter

        private RelayCommand mExamineCharacterCommand;
        public ICommand ExamineCharacterCommand
        {
            get
            {
                if (mExamineCharacterCommand == null)
                {
                    mExamineCharacterCommand = new RelayCommand(
                        parap => this.ExamineCharacterExecute(),
                        param => this.ExamineCharacterCanExecute()
                        );
                }
                return mExamineCharacterCommand;
            }
        }

        private void ExamineCharacterExecute()
        {
            SelectedCharacter.Examine();
        }

        private bool ExamineCharacterCanExecute()
        {
            return SelectedCharacter != null;
        }

        #endregion // ExamineCharacter

        #endregion // Commands

        #region Private Methods

        private void Initialize()
        {
            //SpecialChoice.CreateFile();
            GlobalData.Initialize();
        }

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}