using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Helpers;
using SR5Builder.DataModels;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Attribute = SR5Builder.DataModels.Attribute;

namespace SR5Builder.ViewModels
{
    public struct NamePoints
    {
        private string mName;
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        private int mPoints;
        public int Points
        {
            get { return mPoints; }
            set { mPoints = value; }
        }

        public NamePoints(string name, int points)
        {
            mName = name;
            mPoints = points;
        }

        public override string ToString()
        {
            return Name.ToString() + " (" + Points.ToString() + ")";
        }
    }

    public class CharacterViewModel : ViewModelBase
    {
        #region Private Fields

        private SR5Character character;

        #endregion // Private fields

        #region Properties

            #region Misc

        public override string DisplayName
        {
            get { return character.Name; }
            set { character.Name = value; }
        }
		
		public CharGenMethod Method 
		{
			get { return character.Priorities.Method; }
		}

            #endregion Misc

            #region Display Properties

        public bool AttributesEnabled { get { return character.Priorities.Metatype != Priority.U; } }

        public bool SkillsEnabled { get { return character.Priorities.Skills != Priority.U; } }

        public bool PowersEnabled { get { return (SpecialChoice != null && SpecialChoice.Name.Contains("Adept")); } }

        public bool SpellsEnabled { get { return (SpecialChoice != null && (SpecialChoice.Name == "Magician" || SpecialChoice.Name == "Mystic Adept")); } }

        public int ActiveSpecialTab { get; set; }

        public bool PrioritiesHidden { get { return !(Method == CharGenMethod.Priority || Method == CharGenMethod.SumToTen); } }

            #endregion Display Propertoes

            #region Priorities

        public PrioritiesViewModel PrioritiesVM { get; private set; }

            #endregion // Priorities

            #region Metatype Properties

        public ObservableCollection<NamePoints> AvailableMetatypes { get; private set; }

        public string Metatype
        {
            get { return character.Metatype; }
            set
            {
                character.Metatype = value;
                OnPropertyChanged(nameof(MetatypeStats));
            }
        }

        public MetatypeStats MetatypeStats
        {
            get { return character.MetatypeStats; }
        }

            #endregion Metatype Properties
        
            #region Attributes

        public Attribute Body { get { return character.Body; } }
        public Attribute Agility { get { return character.Agility; } }
        public Attribute Reaction { get { return character.Reaction; } }
        public Attribute Strength { get { return character.Strength; } }
        public Attribute Willpower { get { return character.Willpower; } }
        public Attribute Logic { get { return character.Logic; } }
        public Attribute Intuition { get { return character.Intuition; } }
        public Attribute Charisma { get { return character.Charisma; } }
        public Attribute Edge { get { return character.Edge; } }

            #endregion Attributes

            #region OtherAttributeStuff

        /// <summary>
        /// How many points available for the selected priority
        /// </summary>
        public int AttributePoints
        {
            get
            {
                return GlobalData.PriorityLevels[character.Priorities.Attributes].AttributePoints;                
            }
        }

        /// <summary>
        /// How many points have been assigned to attributes
        /// </summary>
        public int AttributePointsSpent
        {
            get { return character.AttributePointsSpent; }
        }

        public int AttributePointsRemaining
        {
            get
            {
                return AttributePoints - AttributePointsSpent;
            }
        }

        public Essence Essence { get { return character.Essence; } }

        #endregion // OtherAttributeStuff

        public QualitiesViewModel QualitiesVM { get; set; }

            #region Initiatives

        public Initiative PhysicalInitiative { get { return character.PhysicalInitiative; } }
        public Initiative ARInitiative      { get { return character.ARInititative; } }
        public Initiative ColdSimInitiative { get { return character.ColdSimInitiative; } }
        public Initiative HotSimInitiative  { get { return character.HotSimInitiative; } }
        public Initiative AstralInitiative  { get { return character.AstralInitiative; } }

        public LeveledTrait PhysicalDice { get { return character.PhysicalInitiativeDice; } }
        public LeveledTrait ARDice { get { return character.ARInitiativeDice; } }
        public LeveledTrait ColdSimDice { get { return character.ColdSimInitiativeDice; } }
        public LeveledTrait HotSimDice { get { return character.HotSimInitiativeDice; } }
        public LeveledTrait AstralDice { get { return character.AstralInitiativeDice; } }

            #endregion // Initiatives

            #region Limits

        public LeveledTrait PhysicalLimit { get { return character.PhysicalLimit; } }
        public LeveledTrait MentalLimit { get { return character.MentalLimit; } }
        public LeveledTrait SocialLimit { get { return character.SocialLimit; } }
        public LeveledTrait AstralLimit { get { return character.AstralLimit; } }

            #endregion

            #region Specials

        /// <summary>
        /// <b>true</b> if the character can select a Magic (Adept, mMagician, mystic Adept)
        /// or Resonance trait.
        /// In other words, a priority of D or higher 
        /// </summary>
        public bool SpecialEnabled
        {
            get
            {
                return character.Priorities.Special != Priority.U
                    && character.Priorities.Special != Priority.E;
            }
        }

        public ObservableCollection<SpecialChoice> SpecialChoices { get; private set; }

        public SpecialChoice SpecialChoice
        {
            get { return character.SpecialChoice; }
            set
            {
                if (value == null)
                {
                    character.SpecialChoice = SpecialChoice.None(character.Priorities.Special);
                }
                else
                {
                    character.SpecialChoice = value;
                }
            }
        }

        public SpecialKind SpecialKind
        {
            get
            {
                return character.SpecialKind;
            }
        }

        public DataModels.Attribute SpecialAttribute
        {
            get { return character.SpecialAttribute; }
            set {  }
        }

        public int SpecialAttributePoints
        {
            get { return character.SpecialAttributePoints; }
        }

        public int SpecialAttributePointsSpent
        {
            get { return character.SpecialAttributePointsSpent; }
        }

        public int SpecialAttributePointsRemaining
        {
            get { return character.SpecialAttributePoints - character.SpecialAttributePointsSpent; }
        }

        public bool HasPowerPoints {  get { return character.PowerPoints.AugmentedRating > 0; } }

            #endregion // Specials

            #region Skills

        public SkillsViewModel SkillsVM { get; private set; }

            #endregion // Skills

            #region Magic

        public MagicViewModel MagicVM { get; private set; }

            #endregion // Magic

            #region Gear

        public GearViewModel GearVM { get; set; }

        public WeaponsViewModel WeaponsVM { get; set; }

        public string MoneyRemaining
        {
            get
            {
                //return string.Format(GlobalData.CostFormat, "{0C}", character.MoneyRemaining);
                return character.MoneyRemaining.ToString("C", GlobalData.CostFormat);
            }
        }

            #endregion

            #region Validation

        public int IsDone
        {
            get
            {
                if (!PrioritiesValid)
                    return -1;

                bool isDone;

                isDone = AttributePointsDone == 0;
                if (AttributePointsDone < 0)
                    return -1;

                isDone &= SpecialPointsDone == 0;
                if (SpecialPointsDone < 0)
                    return -1;

                isDone &= SkillPointsDone == 0;
                if (SkillPointsDone < 0)
                    return -1;

                isDone &= SkillGroupPointsDone == 0;
                if (SkillGroupPointsDone < 0)
                    return -1;

                isDone &= PowerPointsDone == 0;
                if (PowerPointsDone < 0)
                    return -1;

                if (isDone)
                    return 0;
                else return 1;
            }
        }

        public bool PrioritiesValid
        {
            get
            {
                return character.Priorities.Verify();
            }
        }

        public int AttributePointsDone
        {
            get { return AttributePointsRemaining.CompareTo(0); }
        }

        public int SpecialPointsDone
        {
            get
            {
                return SpecialAttributePointsRemaining.CompareTo(0);
            }
        }

        public int SkillPointsDone
        {
            get { return character.SkillPointsRemaining.CompareTo(0); }
        }

        public int SkillGroupPointsDone
        {
            get { return character.SkillGroupPointsRemaining.CompareTo(0); }
        }

        public int PowerPointsDone
        {
            get { return ((decimal)character.PowerPoints.AugmentedRating - character.PowerPointsSpent).CompareTo(0); }
        }

        public int KarmaSpent
        {
            get { return character.KarmaSpent; }
        }

        #endregion

        #endregion // Properties

        #region Constructors

        public CharacterViewModel()
        {
            character = new SR5Character();
            Initialize();
        }

        public CharacterViewModel(SR5Character character)
        {
            this.character = character;
            Initialize();
        }

        private void Initialize()
        {
            SkillsVM = new SkillsViewModel(character);
            PrioritiesVM = new PrioritiesViewModel(character.Priorities);
            MagicVM = new MagicViewModel(character);
            GearVM = new GearViewModel(character);
            WeaponsVM = new WeaponsViewModel(character);
            QualitiesVM = new QualitiesViewModel(character);

            character.Priorities.PropertyChanged += this.OnPrioritiesChanged;
            character.PropertyChanged += this.OnCharacterChanged;

            LinkAttributeChanged();
        }

        private void LinkAttributeChanged()
        {
            foreach (SR5Builder.DataModels.Attribute a in character.Attributes.Values)
            {
                a.PropertyChanged += this.OnAttributeChanged;
            }
        }

        #endregion // Constructors

        #region Commands

        private RelayCommand mExamineCommand;

        public ICommand ExamineCommand
        {
            get
            {
                if (mExamineCommand == null)
                {
                    mExamineCommand = new RelayCommand(p => this.ExamineExecute(), p => true);
                }
                return mExamineCommand;
            }
        }

        private void ExamineExecute()
        {
            character.Examine();
        }

        #endregion // Commands

        #region Private Methods

        private void OnPrioritiesChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Attributes":
                    OnPropertyChanged(nameof(AttributePoints));
                    OnPropertyChanged(nameof(AttributePointsRemaining));
                    OnPropertyChanged(nameof(PrioritiesValid));
                    OnPropertyChanged(nameof(AttributePointsDone));
                    break;
                case "Special":
                    SpecialChoices = new ObservableCollection<SpecialChoice>(
                                     from s in GlobalData.Specials
                                     where s.Priority == character.Priorities.Special
                                     select s
                                     );
                    HandleSpecialChanged();
                    OnPropertyChanged(nameof(SpecialChoices));
                    OnPropertyChanged(nameof(SpecialEnabled));
                    OnPropertyChanged(nameof(SpecialKind));
                    OnPropertyChanged(nameof(SpecialAttribute));
                    OnPropertyChanged(nameof(PrioritiesValid));
                    OnPropertyChanged(nameof(SpecialPointsDone));
                    OnPropertyChanged(nameof(SpecialAttributePointsRemaining));
                    OnPropertyChanged(nameof(HasPowerPoints));
                    break;
                case "Metatype":
                    AvailableMetatypes = new ObservableCollection<NamePoints>(
                                         from s in GlobalData.Metatypes.Values
                                         where s.SpecialPoints.ContainsKey(character.Priorities.Metatype)
                                         select new NamePoints(s.Name, s.SpecialPoints[character.Priorities.Metatype]));
                    OnPropertyChanged(nameof(AvailableMetatypes));
                    OnPropertyChanged(nameof(AttributesEnabled));
                    OnPropertyChanged(nameof(Metatype));
                    OnPropertyChanged(nameof(PrioritiesValid));
                    OnPropertyChanged(nameof(AttributePointsDone));
                    OnPropertyChanged(nameof(SpecialPointsDone));
                    OnPropertyChanged(nameof(SpecialAttributePoints));
                    OnPropertyChanged(nameof(SpecialAttributePointsRemaining));
                    //character.Metatype = character.Metatype;
                    break;
                case "Skills":
                    OnPropertyChanged(nameof(SkillsEnabled));
                    OnPropertyChanged(nameof(PrioritiesValid));
                    OnPropertyChanged(nameof(SkillPointsDone));
                    break;
                default:
                    OnPropertyChanged(nameof(PrioritiesValid));
                    break;
            }
            OnPropertyChanged(nameof(IsDone));
        }

        private void OnCharacterChanged(object sender, PropertyChangedEventArgs e)
        {
            string p = e.PropertyName;
            if (e.PropertyName == "SpecialChoice")
            {
                switch (SpecialChoice.Name)
                {
                    case "Adept":
                        ActiveSpecialTab = 1;
                        break;
                    case "Technomancer":
                        ActiveSpecialTab = 2;
                        break;
                    default:
                        ActiveSpecialTab = 0;
                        break;
                }
                OnPropertyChanged(nameof(ActiveSpecialTab));
                OnPropertyChanged(nameof(HasPowerPoints));
            }
            else if (e.PropertyName.Contains("AttributePoint"))
            {
                OnPropertyChanged(nameof(AttributePointsDone));
                OnPropertyChanged(nameof(IsDone));
            }
            else if (e.PropertyName.Contains("Skill") && e.PropertyName.Contains("Point"))
            {
                if (e.PropertyName.Contains("Group"))
                {
                    OnPropertyChanged(nameof(SkillGroupPointsDone));
                }
                else
                {
                    OnPropertyChanged(nameof(SkillPointsDone));
                }
                OnPropertyChanged(nameof(IsDone));
            }
            else if (e.PropertyName == "PowerPointsSpent")
            {
                OnPropertyChanged(nameof(IsDone));
            }

            OnPropertyChanged(p);
        }

        private void OnAttributeChanged(object sender, PropertyChangedEventArgs e)
        {
            DataModels.Attribute a = sender as DataModels.Attribute;

            if (a != null)
            {
                string name = e.PropertyName.Replace("Rating", "");
                OnPropertyChanged(name + a.Name);
                OnPropertyChanged(a.Name + "Text");

                if (a == character.PowerPoints &&
                    e.PropertyName == nameof(DataModels.Attribute.AugmentedRating))
                {
                    OnPropertyChanged(nameof(HasPowerPoints));
                }
            }
        }

        private void HandleSpecialChanged()
        {
            string s_name = SpecialChoice.Name;
            bool found = false;
            foreach (SpecialChoice sc in SpecialChoices)
            {
                if (s_name == sc.Name)
                {
                    SpecialChoice = sc;
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                SpecialChoice = SpecialChoice.None(PrioritiesVM.Priorities.Special);
            }
        }

        #endregion // Private Methods

        #region Public Methods

        public override void Dispose()
        {
            character.Priorities.PropertyChanged -= this.OnPrioritiesChanged;
            character.PropertyChanged -= this.OnCharacterChanged;
            base.Dispose();
        }

        public void Examine()
        {
            character.Examine();
        }

        #endregion // Public Methods
    }
}