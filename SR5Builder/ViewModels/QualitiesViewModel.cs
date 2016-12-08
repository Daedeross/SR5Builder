using System;
using System.Collections.Generic;
using System.Linq;
using SR5Builder.DataModels;
using SR5Builder.Helpers;
using System.Windows.Input;
using DrWPF.Windows.Data;
using System.Collections.ObjectModel;
using SR5Builder.Prototypes;
using SR5Builder.Dialogs;
using System.ComponentModel;

namespace SR5Builder.ViewModels
{
    public class QualitiesViewModel: ViewModelBase
    {
        #region Private Fields

        SR5Character character;

        #endregion // Private fields

        #region Properties

        public ObservableCollection<string> Categories { get; set; }

        private string mSelectedCategory;

        public string SelectedCategory
        {
            get { return mSelectedCategory; }
            set
            {
                if (mSelectedCategory != value)
                {
                    mSelectedCategory = value;
                    AvailableQualities = new ObservableCollection<QualityPrototype>(GlobalData.Qualities[mSelectedCategory]);
                    OnPropertyChanged(nameof(AvailableQualities));
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        public ObservableCollection<QualityPrototype> AvailableQualities { get; set; }

        public ObservableDictionary<string, Quality> Qualities
        {
            get { return character.Qualities; }
        }
        
        private QualityPrototype mSelectedNewQuality;
        public QualityPrototype SelectedNewQuality
        {
            get { return mSelectedNewQuality; }
            set
            {
                if (value != mSelectedNewQuality)
                {
                    mSelectedNewQuality = value;
                    OnPropertyChanged(nameof(SelectedNewQuality));
                }
            }
        }

        public Quality SelectedQuality { get; set; }

        public int KarmaSpent { get { return character.KarmaSpent; } }

        public int TotalKarma { get { return character.TotalKarma; } }

        #endregion // Properties

        #region Constructors

        public QualitiesViewModel(SR5Character c)
        {
            character = c;
            character.PropertyChanged += this.OnCharacterChanged;
            Categories = new ObservableCollection<string>(GlobalData.Qualities.Keys);
        }

        #endregion // Constructors

        #region Commands

        #region AddQuality

        private RelayCommand mAddQualityCommand;

        public ICommand AddQualityCommand
        {
            get
            {
                if (mAddQualityCommand == null)
                {
                    mAddQualityCommand = new RelayCommand(p => this.AddQualityExecute(), p => this.AddQualityCanExecute());
                }
                return mAddQualityCommand;
            }
        }

        private void AddQualityExecute()
        {
            string ext = null;
            if (SelectedNewQuality.ExtKind != null && SelectedNewQuality.ExtKind.Length != 0)
            {
                // Open dialog
                SelectionViewModel vm = SelectionViewModel.CreateSelection(character, SelectedNewQuality);
                SelectionDialog dlg = new SelectionDialog();
                dlg.DataContext = vm;

                bool? result = dlg.ShowDialog();
                ext = vm.Selection;
            }

            Quality sp = SelectedNewQuality.ToQuality(character, ext);
            Qualities.Add(sp.Name, sp);
            SelectedQuality = Qualities[sp.Name];
        }

        private bool AddQualityCanExecute()
        {
            return (SelectedNewQuality != null && !Qualities.ContainsKey(SelectedNewQuality.Name));
        }

        #endregion // AddQuality

        #region RemoveQuality

        private RelayCommand mRemoveQualityCommand;

        public ICommand RemoveQualityCommand
        {
            get
            {
                if (mRemoveQualityCommand == null)
                {
                    mRemoveQualityCommand = new RelayCommand(p => this.RemoveQualityExecute(), p => this.RemoveQualityCanExecute());
                }
                return mRemoveQualityCommand;
            }
        }

        private void RemoveQualityExecute()
        {
            Qualities.Remove(SelectedQuality.Name);
            SelectedQuality = Qualities.Count > 0 ? Qualities.Last().Value : null;
        }

        private bool RemoveQualityCanExecute()
        {
            return (SelectedQuality != null);
        }

        #endregion // RemoveQuality

        #region OpenPage

        private RelayCommand<Quality> mOpenPageCommand;

        public ICommand OpenPageCommand
        {
            get
            {
                if (mOpenPageCommand == null)
                {
                    mOpenPageCommand = new RelayCommand<Quality>(p => this.OpenPageExecute(p));
                }
                return mOpenPageCommand;
            }
        }

        private void OpenPageExecute(Quality s)
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

        #region IncreaseQuality

        private RelayCommand mIncreaseQualityCommand;

        public ICommand IncreaseQualityCommand
        {
            get
            {
                if (mIncreaseQualityCommand == null)
                {
                    mIncreaseQualityCommand = new RelayCommand(p => this.IncreaseQualityExecute(), p => this.IncreaseQualityCanExecute());
                }
                return mIncreaseQualityCommand;
            }
        }

        private bool IncreaseQualityCanExecute()
        {
            return (SelectedQuality != null && SelectedQuality.BaseRating < SelectedQuality.Max);
        }

        private void IncreaseQualityExecute()
        {
            SelectedQuality.BaseRating++;
        }

        #endregion // IncreaseQuality

        #region DecreaseQuality

        private RelayCommand mDecreaseQualityCommand;

        public ICommand DecreaseQualityCommand
        {
            get
            {
                if (mDecreaseQualityCommand == null)
                {
                    mDecreaseQualityCommand = new RelayCommand(p => this.DecreaseQualityExecute(), p => this.DecreaseQualityCanExecute());
                }
                return mDecreaseQualityCommand;
            }
        }

        private bool DecreaseQualityCanExecute()
        {
            return (SelectedQuality != null && SelectedQuality.BaseRating > SelectedQuality.Min);
        }

        private void DecreaseQualityExecute()
        {
            SelectedQuality.BaseRating--;
        }

        #endregion // DecreaseQuality

        #endregion // Commands

        #region Private Methods
        
        private void OnCharacterChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Contains("Karma"))
            {
                OnPropertyChanged(e.PropertyName);
            }
        }

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}
