using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Helpers;
using SR5Builder.DataModels;
using SR5Builder.Dialogs;
using Microsoft.Win32;
using SR5Builder.Prototypes.Loaders;

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
                    OnPropertyChanged(nameof(SelectedCharacter));
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
            //CharacterViewModel vm = new CharacterViewModel();
            //vm.DisplayName = "Character " + (CharacterVMs.Count + 1).ToString();
            //CharacterVMs.Add(vm);
            //SelectedCharacter = CharacterVMs.Last();
            NewCharacterViewModel vm = new NewCharacterViewModel();
            NewCharacterDialog dlg = new NewCharacterDialog();
            dlg.DataContext = vm;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                SR5Character c = new SR5Character(vm.Settings.CurrentSettings.AsQueryable())
                {
                    Name = "Character " + (CharacterVMs.Count + 1).ToString()
                };
                CharacterViewModel cvm = new CharacterViewModel(c);
                //cvm.DisplayName = 
                CharacterVMs.Add(cvm);
                SelectedCharacter = CharacterVMs.Last();
            }
        }

        private bool AddNewCharacterCanExecute()
        {
            return true;
        }

            #endregion // AddNewCharacter
            #region OpenCharacter

        private RelayCommand mOpenCharacter;
        public ICommand OpenCharacter
        {
            get
            {
                if (mOpenCharacter == null)
                {
                    mOpenCharacter = new RelayCommand(
                        parap => this.OpenCharacterExecute(),
                        param => this.OpenCharacterCanExecute()
                        );
                }
                return mOpenCharacter;
            }
        }

        private void OpenCharacterExecute()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "SR5 Character (*.sr5)|*.sr5";
            if (dlg.ShowDialog() == true)
            {
                CharacterLoader loader = CharacterLoader.LoadFromFile(dlg.FileName);
                SR5Character c = new SR5Character(loader);
                CharacterViewModel cvm = new CharacterViewModel(c);
                CharacterVMs.Add(cvm);
                SelectedCharacter = CharacterVMs.Last();
            }
        }

        private bool OpenCharacterCanExecute()
        {
            return true;
        }

            #endregion // OpenCharacter
            #region SaveCharacter

        private RelayCommand mSaveCharacter;
        public ICommand SaveCharacter
        {
            get
            {
                if (mSaveCharacter == null)
                {
                    mSaveCharacter = new RelayCommand(
                        parap => this.SaveCharacterExecute(),
                        param => this.SaveCharacterCanExecute()
                        );
                }
                return mSaveCharacter;
            }
        }

        private void SaveCharacterExecute()
        {
            SelectedCharacter.Save();
        }

        private bool SaveCharacterCanExecute()
        {
            return SelectedCharacter != null;
        }
            #endregion // SaveCharacter
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
            #region PrintCharacter

        private RelayCommand mPrintCharacterCommand;
        public ICommand PrintCharacterCommand
        {
            get
            {
                if (mPrintCharacterCommand == null)
                {
                    mPrintCharacterCommand = new RelayCommand(
                        parap => this.PrintCharacterExecute(),
                        param => this.PrintCharacterCanExecute()
                        );
                }
                return mPrintCharacterCommand;
            }
        }

        private void PrintCharacterExecute()
        {
            SelectedCharacter.Print();
        }

        private bool PrintCharacterCanExecute()
        {
            return SelectedCharacter != null;
        }

            #endregion // PrintCharacter

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