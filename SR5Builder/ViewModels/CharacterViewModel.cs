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
            get
            {
                return character.Name;
            }
            set
            {
                character.Name = value;
            }
        }
		
		public CharGenMethod Method 
		{
			get { return character.Priorities.Method; }
		}

        public bool IsValid
        {
            get
            {
                bool valid = true;
                valid &= (AttributePointsRemaining >= 0);
                valid &= (SpecialAttributePointsRemaining >= 0);

                return valid;
            }
        }

        public bool IsDone
        {
            get
            {
                bool done = true;
                done &= (AttributePointsRemaining == 0);
                done &= (SpecialAttributePointsRemaining == 0);

                return done;
            }
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
                OnPropertyChanged("MetatypeStats");
            }
        }

        public MetatypeStats MetatypeStats
        {
            get { return character.MetatypeStats; }
        }

            #endregion Metatype Properties

            #region Attribute Text

        public string BodyText
        {
            get
            {
                return character.Body.ToString();
            }
        }

        public string AgilityText
        {
            get
            {
                return character.Agility.ToString();
            }
        }

        public string ReactionText
        {
            get
            {
                return character.Reaction.ToString();
            }
        }

        public string StrengthText
        {
            get
            {
                return character.Strength.ToString();
            }
        }

        public string WillpowerText
        {
            get
            {
                return character.Willpower.ToString();
            }
        }

        public string LogicText
        {
            get
            {
                return character.Logic.ToString();
            }
        }

        public string IntuitionText
        {
            get
            {
                return character.Intuition.ToString();
            }
        }

        public string CharismaText
        {
            get { return character.Charisma.ToString(); }
        }

        public string EdgeText
        {
            get { return character.Edge.ToString(); }
        }

            #endregion // Attribute Text

            #region Base Attributes

        public int BaseBody
        {
            get { return character.Body.BaseRating; }
            set
            {
                character.Body.BaseRating = value;
            }
        }

        public int BaseAgility
        {
            get { return character.Agility.BaseRating; }
            set
            {
                character.Agility.BaseRating = value;
            }
        }

        public int BaseReaction
        {
            get { return character.Reaction.BaseRating; }
            set
            {
                character.Reaction.BaseRating = value;
            }
        }

        public int BaseStrength
        {
            get { return character.Strength.BaseRating; }
            set
            {
                character.Strength.BaseRating = value;
            }
        }

        public int BaseWillpower
        {
            get { return character.Willpower.BaseRating; }
            set
            {
                character.Willpower.BaseRating = value;
            }
        }

        public int BaseLogic
        {
            get { return character.Logic.BaseRating; }
            set
            {
                character.Logic.BaseRating = value;
            }
        }

        public int BaseIntuition
        {
            get { return character.Intuition.BaseRating; }
            set
            {
                character.Intuition.BaseRating = value;
            }
        }

        public int BaseCharisma
        {
            get { return character.Charisma.BaseRating; }
            set
            {
                character.Charisma.BaseRating = value;
            }
        }

        public int BaseEdge
        {
            get { return character.Edge.BaseRating; }
            set
            {
                character.Edge.BaseRating = value;
            }
        }

            #endregion // Base Attributes

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

        #endregion // OtherAttributeStuff
        
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

        public int SpecialAttribute
        {
            get { return character.SpecialAttribute; }
            set { character.SpecialAttribute = value; }
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

        #endregion

            #region Validation

        public bool PrioritiesValid
        {
            get
            {
                return character.Priorities.Verify();
            }
        }

        public bool AttributesPointsValid
        {
            get { return AttributePointsRemaining == 0; }
        }

        public bool SpecialPointsValid
        {
            get { return SpecialAttributePointsRemaining == 0; }
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
                    OnPropertyChanged("AttributePoints");
                    OnPropertyChanged("AttributePointsRemaining");
                    OnPropertyChanged("PrioritiesValid");
                    break;
                case "Special":
                    SpecialChoices = new ObservableCollection<SpecialChoice>(
                                     from s in GlobalData.Specials
                                     where s.Priority == character.Priorities.Special
                                     select s
                                     );
                    HandleSpecialChanged();
                    OnPropertyChanged("SpecialChoices");
                    OnPropertyChanged("SpecialEnabled");
                    OnPropertyChanged("SpecialKind");
                    OnPropertyChanged("SpecialAttribute");
                    OnPropertyChanged("PrioritiesValid");
                    break;
                case "Metatype":
                    AvailableMetatypes = new ObservableCollection<NamePoints>(
                                         from s in GlobalData.Metatypes.Values
                                         where s.SpecialPoints.ContainsKey(character.Priorities.Metatype)
                                         select new NamePoints(s.Name, s.SpecialPoints[character.Priorities.Metatype]));
                    OnPropertyChanged("AvailableMetatypes");
                    OnPropertyChanged("AttributesEnabled");
                    OnPropertyChanged("Metatype");
                    OnPropertyChanged("PrioritiesValid");
                    //character.Metatype = character.Metatype;
                    break;
                case "Skills":
                    OnPropertyChanged("SkillsEnabled");
                    OnPropertyChanged("PrioritiesValid");
                    break;
                default:
                    OnPropertyChanged("PrioritiesValid");
                    break;
            }      
        }

        private void OnCharacterChanged(object sender, PropertyChangedEventArgs e)
        {
            string p = e.PropertyName;
            //if (p.Contains("Augmented"))
            //{
            //    OnPropertyChanged(p.Replace("Augmented", "") + "Text");
            //    OnPropertyChanged("AttributePointsSpent");
            //    OnPropertyChanged("AttributePointsRemaining");
            //}
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
                OnPropertyChanged("ActiveSpecialTab");
            }
            OnPropertyChanged(p);
        }

        private void OnAttributeChanged(object sender, PropertyChangedEventArgs e)
        {
            SR5Builder.DataModels.Attribute a = sender as SR5Builder.DataModels.Attribute;

            if (a != null)
            {
                string name = e.PropertyName.Replace("Rating", "");
                OnPropertyChanged(name + a.Name);
                OnPropertyChanged(a.Name + "Text");
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