using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using DrWPF.Windows.Data;
using SR5Builder.Prototypes;

namespace SR5Builder.DataModels
{
    /// <summary>
    /// This class holds all the in-memory data, including its own instance of Settings.
    /// (De)Seri1alization is handled by CharacterLoader.
    /// </summary>
    public class SR5Character : DataModelBase
    {
        #region Private Fields

        #endregion // Private Fields

        #region Properties

        /// <summary>
        /// Holds all the traits on the character which are augmentable
        /// (i.e. implement the IAugemntable interface).
        /// </summary>
        public ObservableDictionary<string, IAugmentable> Augmentables { get; set; }

        public ObservableCollection<IAugmentContainer> AugmentContainers { get; set; }

        public ObservableCollection<IKarmaCost> KarmaCosts { get; set; }
            = new ObservableCollection<IKarmaCost>();

        public GenSettings Settings { get; set; }

            #region Metatype

        private string mMetatype;
        public string Metatype
        {
            get { return mMetatype; }
            set
            {
                //if (!GlobalData.PriorityLevels[Priorities.Metatype].Metatypes.Contains(value))
                //    value = "Human";

                if (value != null && mMetatype != value)
                {
                    if (GlobalData.Metatypes.Keys.Contains(value))
                    {
                        MetatypeStats = GlobalData.Metatypes[value];
                        mMetatype = value;
                        OnPropertyChanged(nameof(Metatype));
                        OnPropertyChanged(nameof(MetatypeStats));
                        //OnPropertyChanged(nameof(SpecialAttributePointsRemaining));
                        //RefreshAttributes();
                    }
                }
                OnPropertyChanged(nameof(SpecialAttributePoints));
            }
        }

        [XmlIgnore]
        public MetatypeStats MetatypeStats { get; private set; }

            #endregion // Metatype

            #region Attributes

        public Dictionary<string, Attribute> Attributes { get; set; }

        private Attribute mBody;
        public Attribute Body
        {
            get { return mBody; }
            set
            {
                if (value != mBody)
                {
                    mBody = value;
                    OnPropertyChanged(nameof(Body));
                }
            }
        }

        private Attribute mAgility;
        public Attribute Agility
        {
            get { return mAgility; }
            set
            {
                if (value != mAgility)
                {
                    mAgility = value;
                    OnPropertyChanged(nameof(Agility));
                }
            }
        }

        private Attribute mReaction;
        public Attribute Reaction
        {
            get { return mReaction; }
            set
            {
                if (value != mReaction)
                {
                    mReaction = value;
                    OnPropertyChanged(nameof(Reaction));
                }
            }
        }

        private Attribute mStrength;
        public Attribute Strength
        {
            get { return mStrength; }
            set
            {
                if (value != mStrength)
                {
                    mStrength = value;
                    OnPropertyChanged(nameof(Strength));
                }
            }
        }

        private Attribute mWillpower;
        public Attribute Willpower
        {
            get { return mWillpower; }
            set
            {
                if (value != mWillpower)
                {
                    mWillpower = value;
                    OnPropertyChanged(nameof(Willpower));
                }
            }
        }

        private Attribute mLogic;
        public Attribute Logic
        {
            get { return mLogic; }
            set
            {
                if (value != mLogic)
                {
                    mLogic = value;
                    OnPropertyChanged(nameof(Logic));
                }
            }
        }

        private Attribute mIntuition;
        public Attribute Intuition
        {
            get { return mIntuition; }
            set
            {
                if (value != mIntuition)
                {
                    mIntuition = value;
                    OnPropertyChanged(nameof(Intuition));
                }
            }
        }

        private Attribute mCharisma;
        public Attribute Charisma
        {
            get { return mCharisma; }
            set
            {
                if (value != mCharisma)
                {
                    mCharisma = value;
                    OnPropertyChanged(nameof(Charisma));
                }
            }
        }

        private Attribute mEdge;
        public Attribute Edge
        {
            get { return mEdge; }
            set
            {
                if (value != mEdge)
                {
                    mEdge = value;
                    OnPropertyChanged(nameof(Edge));
                }
            }
        }

        private Essence mEssence;
        public Essence Essence
        {
            get { return mEssence; }
            protected set
            {
                if (value != mEssence)
                {
                    mEssence = value;
                    OnPropertyChanged(nameof(Essence));
                }
            }
        }

        public int AttributePointsSpent
        {
            get
            {
                if (MetatypeStats == null)
                {
                    return 0;
                }
                int points = 0;
                points += mBody.Points;
                points += mAgility.Points;
                points += mReaction.Points;
                points += mStrength.Points;
                points += mWillpower.Points;
                points += mLogic.Points;
                points += mIntuition.Points;
                points += mCharisma.Points;
                return points;
            }
        }
   
            #endregion // Attributes

            #region Calculated Traits

                #region Initiatives

        public Initiative PhysicalInitiative { get; set; }
        public Initiative ARInititative { get; set; }
        public Initiative ColdSimInitiative { get; set; }
        public Initiative HotSimInitiative { get; set; }
        public Initiative AstralInitiative { get; set; }

        public InitiativeDice PhysicalInitiativeDice { get; set; }
        public InitiativeDice ARInitiativeDice { get; set; }
        public InitiativeDice ColdSimInitiativeDice { get; set; }
        public InitiativeDice HotSimInitiativeDice { get; set; }
        public InitiativeDice AstralInitiativeDice { get; set; }

            #endregion // Initiatives

            #region Limits

        public Limit MentalLimit { get; private set; }

        public Limit PhysicalLimit { get; private set; }

        public Limit SocialLimit { get; private set; }

        public Limit AstralLimit
        {
            get
            {
                if (MentalLimit.AugmentedRating > SocialLimit.AugmentedRating)
                    return MentalLimit;
                else return SocialLimit;
            }
        }

                #endregion // Limits

                #region Persona

        public int Attack
        {
            get { return mCharisma.AugmentedRating; }
        }

        public int Sleeze
        {
            get { return mIntuition.AugmentedRating; }
        }

        public int DataProcessing
        {
            get { return mLogic.AugmentedRating; }
        }

        public int FireWall
        {
            get { return mWillpower.AugmentedRating; }
        }

        public int DeviceRating
        {
            get { return mSpecialAttribute.AugmentedRating; }
        }

                #endregion // Living Persona

                #region Condition

        public int PhysicalBoxes
        {
            get { return (int)Math.Ceiling((double)mBody.AugmentedRating / 2 + 8); }
        }

        public int StunBoxes
        {
            get { return (int)Math.Ceiling((double)mWillpower.AugmentedRating / 2 + 8); }
        }
                #endregion // Condition

            #endregion // Calculated Traits

            #region Specials

        private SpecialChoice mSpecialChoice;
        public SpecialChoice SpecialChoice
        {
            get { return mSpecialChoice; }
            set
            {
                if (mSpecialChoice != value)
                {
                    mSpecialChoice = value;
                    ResetSpecialProperties();
                    mSpecialAttribute.Name = SpecialKind.ToString();
                    mSpecialAttribute.BaseRating = mSpecialAttribute.BaseRating;
                    OnPropertyChanged(nameof(SpecialChoice));
                    OnPropertyChanged(nameof(SpecialKind));
                    OnPropertyChanged(nameof(SpecialAttribute));
                }
            }
        }

        public SpecialKind SpecialKind
        {
            get
            {
                if (mSpecialChoice == null)
                    return SpecialKind.None;
                return mSpecialChoice.Kind;
            }
        }

        private Attribute mSpecialAttribute;
        public Attribute SpecialAttribute
        {
            get
            {
                return mSpecialAttribute;
            }
            set
            {
                mSpecialAttribute = value;
                OnPropertyChanged(nameof(SpecialAttribute));
                OnPropertyChanged(nameof(SpecialAttributePointsSpent));
            }
        }

        public int SpecialAttributePoints
        {
            get
            {
                //Debug.WriteLine("Mon");
                if (this.MetatypeStats != null && Priorities.Metatype != Priority.U)
                {
                    //Debug.WriteLine("Tue");
                    return MetatypeStats.SpecialPoints[this.Priorities.Metatype];
                }
                return 0;
            }
        }

        public int SpecialAttributePointsSpent
        {
            get
            {
                return mSpecialAttribute.Points + mEdge.Points;
            }
        }

            #endregion // Specials

            #region Priorities

        public Priorities Priorities { get; private set; }

            #endregion // Priorities

        public ObservableDictionary<string, Quality> Qualities { get; set; }

            #region Skills

        public ObservableDictionary<string, Skill> SkillList { get; set; }

        public ObservableDictionary<string, SkillGroup> SkillGroupsList { get; set; }

        public int SkillPointsSpent
        {
            get
            {
                int points = 0;
                foreach (Skill skill in SkillList.Values)
                {
                    points += skill.Points;
                }
                return points;
            }
        }

        public int SkillPointsRemaining
        {
            get { return GlobalData.PriorityLevels[Priorities.Skills].SkillPoints - SkillPointsSpent; }
        }

        public int SkillGroupPointsSpent
        {
            get
            {
                int points = 0;
                foreach (SkillGroup skillGroup in SkillGroupsList.Values)
                {
                    points += skillGroup.Points;
                }
                return points;
            }
        }

        public int SkillGroupPointsRemaining
        {
            get { return GlobalData.PriorityLevels[Priorities.Skills].SkillGroupPoints - SkillGroupPointsSpent; }
        }

            #endregion // Skills

            #region Spells / Powers

        public ObservableDictionary<string, Spell> SpellList { get; set; }

        public int SpellKarma
        {
            get
            {
                if (SpecialChoice.Name == "Magician" || SpecialChoice.Name == "Mystic Adept")
                {
                    int count = SpellList.Count - SpecialChoice.Spells;
                    return count > 0 ? count * Settings.SpellKarma : 0;
                }
                else
                    return 0;
            }
        }

        public ObservableDictionary<string, AdeptPower> PowerList { get; set; }

        public PowerPoints PowerPoints { get; set; }
        
        public decimal PowerPointsSpent
        {
            get
            {
                decimal pp = 0;
                foreach (var power in PowerList.Values)
                {
                    pp += power.PowerPoints;
                }
                return pp;
            }
        }

        public int PowerKarma
        {
            get
            {
                if (SpecialChoice.Name == "Mystic Adept")
                {
                    decimal pp = 0;
                    foreach (AdeptPower power in PowerList)
                    {
                        pp += power.PowerPoints;
                    }
                    return (int)Math.Ceiling( pp * Settings.PowerPointKarma);
                }
                return 0;
            }
        }

        private int mAdvancedGrade;
        public int AdvancedGrade
        {
            get
            {
                return mAdvancedGrade;
            }
            set
            {
                if (mAdvancedGrade != value)
                {
                    mAdvancedGrade = value;
                    OnPropertyChanged(nameof(AdvancedGrade));
                    OnPropertyChanged(nameof(AdvancedGradeKarma));
                }
            }
        }

        public int AdvancedGradeKarma
        {
            get
            {
                if (SpecialKind == SpecialKind.Magic)
                {
                    return Settings.InitiationKarma(mAdvancedGrade);
                }
                else if (SpecialKind == SpecialKind.Resonance)
                {
                    return Settings.SubmersionKarma(mAdvancedGrade);
                }
                return 0;
            }
        }

        #endregion // Spells / Powers
        
        private int mKarmaEarned;
        public int KarmaEarned
        {
            get { return mKarmaEarned; }
            set
            {
                if (value != mKarmaEarned)
                {
                    mKarmaEarned = value;
                    OnPropertyChanged("KarmaEarned");
                }
            }
        }

        public int TotalKarma
        {
            get { return KarmaEarned + Settings.StartingKarma; }
        }

        private int mKarmaSpent;
        public int KarmaSpent
        {
            get { return mKarmaSpent; }
        }
    
        public int KarmaAvailable
        {
            get { return mKarmaEarned - mKarmaSpent; }
        }

            #region Gear

        public ObservableDictionary<string, Gear> GearList { get; set; }

        public ObservableDictionary<string, Implant> ImplantList { get; set; }

            #endregion // Gear

            #region Weapons

        public ObservableDictionary<string, MeleeWeapon> MeleeWeapons { get; set; }

        public ObservableDictionary<string, RangedWeapon> RangedWeapons { get; set; }

        //public ObservableDictionary<string, RangedWeapon> ProjectileWeapons { get; set; }

        #endregion // Weapons


        private decimal mMoneySpent;
        public decimal MoneySpent
        {
            get { return mMoneySpent; }
        }

        public decimal StartingMoney
        {
            get { return GlobalData.PriorityLevels[Priorities.Resources].Resources; }
        }

        public decimal MoneyRemaining
        {
            get { return StartingMoney - MoneySpent; }
        }

        #endregion // Properties

        #region Constructors

        public SR5Character()
        {
            Initialize(GlobalData.GenSettingsList["Default"]);
        }

        public SR5Character(SettingsPrototype settings)
        {
            Initialize(settings);
        }

        public SR5Character(IQueryable<ViewModels.Setting> settings)
        {
            SettingsPrototype loader = new SettingsPrototype();
            loader.Properties = (from set in settings
                                 select set).ToDictionary(s => s.Key, s => s.Value);
            Initialize(loader);
        }

        private void Initialize(SettingsPrototype settings)
        {
            Settings = new GenSettings(settings.Properties);

            Priorities = new Priorities();
            Priorities.ChangeMethod(Settings.Method);
            MetatypeStats = new MetatypeStats();
            Metatype = "Human";

            KarmaCosts.CollectionChanged += OnKarmaCostsCollectionChanged;
            Augmentables = new ObservableDictionary<string, IAugmentable>();

            InitializeAttributes();
            InitializeCollections();

            SpecialChoice = SpecialChoice.None(Priority.U);
        }

        private void InitializeCollections()
        {
            // Priorities
            Priorities.PropertyChanged += this.OnPrioritiesChanged;

            // create Skill and SkillGroup dicts and add them to Augmentable listener
            SkillList = new ObservableDictionary<string, Skill>();
            SkillList.CollectionChanged += this.OnSkillListChanged;
            SkillList.CollectionChanged += this.OnAugmentablesChanged;

            SkillGroupsList = new ObservableDictionary<string, SkillGroup>();
            SkillGroupsList.CollectionChanged += this.OnSkillGroupListChanged;
            SkillGroupsList.CollectionChanged += this.OnAugmentablesChanged;

            SpellList = new ObservableDictionary<string, Spell>();
            SpellList.CollectionChanged += OnSpellsChanged;

            PowerList = new ObservableDictionary<string, AdeptPower>();
            PowerList.CollectionChanged += this.OnAugmentablesChanged;
                // Even though Adept powers never recieve Augments,
                // this is needed to remove augments when powers are removed

            GearList = new ObservableDictionary<string, Gear>();
            GearList.CollectionChanged += this.OnAugmentablesChanged;
            GearList.CollectionChanged += this.OnGearCollectionChanged;

            ImplantList = new ObservableDictionary<string, Implant>();
            ImplantList.CollectionChanged += this.OnAugmentablesChanged;
            ImplantList.CollectionChanged += this.OnImplantsCollectionChanged;

            Qualities = new ObservableDictionary<string, Quality>();
            Qualities.CollectionChanged += this.OnAugmentablesChanged;

            mEssence.Subscribe();
        }

        private void InitializeAttributes()
        {
            // Create main and special Attributes
            mBody = new Attribute(this, "Body");
            mAgility = new Attribute(this, "Agility");
            mReaction = new Attribute(this, "Reaction");
            mStrength = new Attribute(this, "Strength");

            mWillpower = new Attribute(this, "Willpower");
            mLogic = new Attribute(this, "Logic");
            mIntuition = new Attribute(this, "Intuition");
            mCharisma = new Attribute(this, "Charisma");

            mEdge = new Attribute(this, "Edge");

            mEssence = new Essence(this, "Essence");
            mEssence.BaseRating = 6;    // may get pulled from settings later

            mSpecialAttribute = new SpecialAttribute(this);

            // Add attributes to containing Dictionary
            // for easy refference by other traits
            Attributes = new Dictionary<string, Attribute>(13);
            Attributes.Add("Body", mBody);
            Attributes.Add("Agility", mAgility);
            Attributes.Add("Reaction", mReaction);
            Attributes.Add("Strength", mStrength);

            Attributes.Add("Willpower", mWillpower);
            Attributes.Add("Logic", mLogic);
            Attributes.Add("Intuition", mIntuition);
            Attributes.Add("Charisma", mCharisma);

            Attributes.Add("Edge", mEdge);

            Attributes.Add("Special", mSpecialAttribute);

            // Power Points
            PowerPoints = new PowerPoints(this, "PowerPoints");
            Attributes.Add(PowerPoints.Name, PowerPoints);

            // Add to Augmentables and PropertyChanged listener
            foreach (Attribute a in Attributes.Values)
            {
                a.PropertyChanged += this.OnAttributeChanged;
                Augmentables.Add(a.Name, a);
            }
            
            // Initiatives
            PhysicalInitiative = new Initiative(mReaction, mIntuition);
            PhysicalInitiative.Name = "Initiative";
            ARInititative = PhysicalInitiative;
            ColdSimInitiative = new Initiative(mIntuition, mLogic);
            ColdSimInitiative.Name = "Cold Sim Initiative";
            HotSimInitiative = new Initiative(mIntuition, mLogic);
            HotSimInitiative.Name = "Hot Sim Initiative";
            AstralInitiative = new Initiative(mIntuition, mIntuition);
            AstralInitiative.Name = "Astral Initiative";

            Augmentables.Add(PhysicalInitiative.Name, PhysicalInitiative);
            Augmentables.Add(ColdSimInitiative.Name, ColdSimInitiative);
            Augmentables.Add(HotSimInitiative.Name, HotSimInitiative);
            Augmentables.Add(AstralInitiative.Name, AstralInitiative);

            // Initiative Dice
            PhysicalInitiativeDice = new InitiativeDice(this, "Physical Initiative Dice", 1);
            ARInitiativeDice = PhysicalInitiativeDice;// new InitiativeDice(this, "AR Initiative Dice", 1);
            ColdSimInitiativeDice = new InitiativeDice(this, "ColdSim Initiative Dice", 3);
            HotSimInitiativeDice = new InitiativeDice(this, "HotSim Initiative Dice", 4);
            AstralInitiativeDice = new InitiativeDice(this, "Astral Initiative Dice", 2);

            Augmentables.Add(PhysicalInitiativeDice.Name, PhysicalInitiativeDice);
            //Augmentables.Add(ARInitiativeDice.Name, ARInitiativeDice);
            Augmentables.Add(ColdSimInitiativeDice.Name, ColdSimInitiativeDice);
            Augmentables.Add(HotSimInitiativeDice.Name, HotSimInitiativeDice);
            Augmentables.Add(AstralInitiativeDice.Name, AstralInitiativeDice);

            // Limits
            MentalLimit = new Limit(mLogic, mIntuition, mWillpower);
            MentalLimit.Name = "Mental Limit";
            PhysicalLimit = new Limit(mStrength, mBody, mReaction);
            PhysicalLimit.Name = "Physical Limit";
            SocialLimit = new Limit(mCharisma, mWillpower, mEssence);
            SocialLimit.Name = "Social Limit";

            // Add limits to Augmentables
            Augmentables.Add(MentalLimit.Name, MentalLimit);
            Augmentables.Add(PhysicalLimit.Name, PhysicalLimit);
            Augmentables.Add(SocialLimit.Name, SocialLimit);
        }

        #endregion // Constructors

        #region Public Methods

        public void Examine()
        {
            Debug.WriteLine("Examine" + Name);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnPrioritiesChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Priorities.Metatype):
                    if (GlobalData.PriorityLevels[Priorities.Metatype].Metatypes.Contains(this.mMetatype))
                        Metatype = mMetatype;
                    else
                        Metatype = "Human";
                    break;
                case nameof(Priorities.Special):
                    if(mSpecialChoice != null)
                    {
                        SpecialChoice[] newSpecials = (from s in GlobalData.Specials
                                                       where s.Priority == Priorities.Special && s.Name == mSpecialChoice.Name
                                                       select s).ToArray();
                        if (newSpecials.Count() == 0)
                            this.SpecialChoice = SpecialChoice.None(Priorities.Special);
                        else this.SpecialChoice = newSpecials[0];
                    }
                    else this.SpecialChoice = SpecialChoice.None(Priorities.Special);
                    OnPropertyChanged(nameof(SpecialAttribute));
                    RecalcFreeSpells();
                    mSpecialAttribute.Name = SpecialKind.ToString();
                    break;
                case nameof(Priorities.Skills):
                    OnPropertyChanged(nameof(SkillPointsRemaining));
                    OnPropertyChanged(nameof(SkillGroupPointsRemaining));
                    break;
                case nameof(Priorities.Resources):
                    OnPropertyChanged(nameof(StartingMoney));
                    OnPropertyChanged(nameof(MoneyRemaining));
                    break;
                default:
                    break;
            }
        }

        private void OnSkillListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (KeyValuePair<string, Skill> skill in e.OldItems)
                    skill.Value.PropertyChanged -= this.OnSkillChanged;

            if (e.NewItems != null)
                foreach (KeyValuePair<string, Skill> skill in e.NewItems)
                    skill.Value.PropertyChanged += this.OnSkillChanged;

            OnPropertyChanged(nameof(SkillPointsSpent));
        }

        private void OnSkillChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SkillPointsSpent));
            OnPropertyChanged(nameof(SkillPointsRemaining));
        }

        private void OnSkillGroupListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (KeyValuePair<string, SkillGroup> kvp in e.OldItems)
                    kvp.Value.PropertyChanged -= this.OnSkillGroupChanged;
            if (e.NewItems != null)
                foreach (KeyValuePair<string, SkillGroup> kvp in e.NewItems)
                    kvp.Value.PropertyChanged += this.OnSkillGroupChanged;

            OnPropertyChanged(nameof(SkillGroupPointsSpent));
        }

        private void OnSkillGroupChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SkillGroupPointsSpent));
            OnPropertyChanged(nameof(SkillGroupPointsRemaining));
        }

        private void OnAttributeChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Points")
            {
                if (sender == mSpecialAttribute || sender == mEdge)
                {
                    OnPropertyChanged(nameof(SpecialAttributePointsSpent));
                    //OnPropertyChanged(nameof(SpecialAttributePointsRemaining));
                }
                else
                {
                    OnPropertyChanged(nameof(AttributePointsSpent));
                    //OnPropertyChanged(nameof(AttributePointsRemaining));
                }
            }
            else if (e.PropertyName == "AugmentedRating")
            {
                if (sender == MentalLimit || sender == SocialLimit)
                {
                    OnPropertyChanged(nameof(AstralLimit));
                }
            }
        }

        /// <summary>
        /// Keeps the dictionary containing all Augmentables up to date as Traits are
        /// added and removed from other lists/dictionarys on the character
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAugmentablesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            HashSet<string> propNames = new HashSet<string>();  // used to store property names that are changed via augmentables update
            if (e.OldItems != null)
            {
                List<object> oldItems = e.OldItems.OfType<object>().ToList();

                foreach (object item in oldItems)
                {
                    Type valueType = item.GetType();
                    if (valueType.IsGenericType)
                    {
                        Type baseType = valueType.GetGenericTypeDefinition();
                        if (baseType == typeof(KeyValuePair<,>))
                        {
                            object a = valueType.GetProperty("Value").GetValue(item, null);

                            if (a is IAugmentable)
                                Augmentables.Remove((a as IAugmentable).Name);

                            if (a is IAugmentContainer)
                                (a as IAugmentContainer).ClearAugments();

                            // If item is SkillGroup, remove its skills from Augmentables
                            if (a is SkillGroup)
                                foreach(KeyValuePair<string, Skill> kvp in (a as SkillGroup).Skills)
                                    Augmentables.Remove(kvp.Key);

                            if (a is AdeptPower)
                            {
                                (a as INotifyPropertyChanged).PropertyChanged -= this.OnPowerChanged;
                                propNames.Add(nameof(PowerPointsSpent));
                            }

                            if (a is IKarmaCost)
                            {
                                KarmaCosts.Remove(a as IKarmaCost);
                            }
                        }
                    }
                }
            }

            if (e.NewItems != null)
            {
                List<object> newItems = e.NewItems.OfType<object>().ToList();
                
                foreach (object item in newItems)
                {
                    Type valueType = item.GetType();
                    if (valueType.IsGenericType)
                    {
                        Type baseType = valueType.GetGenericTypeDefinition();
                        if (baseType == typeof(KeyValuePair<,>))
                        {
                            object a = valueType.GetProperty("Value").GetValue(item, null);

                            if (a is IAugmentable)
                                Augmentables.Add((a as IAugmentable).Name, (a as IAugmentable));

                            // If item is SkillGroup, add its skills to Augmentables
                            if (a is SkillGroup)
                                foreach(KeyValuePair<string, Skill> kvp in (a as SkillGroup).Skills)
                                    Augmentables.Add(kvp.Key, kvp.Value);

                            if (a is AdeptPower)
                            {
                                (a as AdeptPower).PropertyChanged += this.OnPowerChanged;
                                propNames.Add(nameof(PowerPointsSpent));
                            }

                            if (a is IKarmaCost)
                            {
                                KarmaCosts.Add(a as IKarmaCost);
                            }
                        }
                    }
                }
            }

            // Call Prop Changed
            foreach (string name in propNames)
            {
                OnPropertyChanged(name);
            }
        }

        private void OnPowerChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AdeptPower.PowerPoints))
            {
                OnPropertyChanged(nameof(PowerPointsSpent));
            }
        }

        private void ResetSpecialProperties()
        {
            // if the
            if (mSpecialChoice == null)
            {
                //PowerList.Clear();
                //SpellList.Clear();
            }
            else
            {
                if (!mSpecialChoice.Name.Contains("Magician") && mSpecialChoice.Name != "Mystic Adept")
                {
                    if (SpellList != null)
                    {
                        SpellList.Clear();
                    }
                }
                if (!mSpecialChoice.Name.Contains("Adept"))
                {
                    if (PowerList != null)
                    {
                        PowerList.Clear();
                    }
                }

                if (mSpecialChoice.Name != "Technomancer")
                {

                }
            }
        }

        private void OnKarmaCostsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (object item in e.OldItems)
                    if (item is INotifyPropertyChanged)
                        (item as INotifyPropertyChanged).PropertyChanged -= OnKarmaCostChanged;

            if (e.NewItems != null)
                foreach (object item in e.NewItems)
                    if (item is INotifyPropertyChanged)
                        (item as INotifyPropertyChanged).PropertyChanged += OnKarmaCostChanged;

            RecalcKarmaCost();
        }

        private void OnKarmaCostChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IKarmaCost.Karma))
            {
                RecalcKarmaCost();
            }
        }

        private void RecalcKarmaCost()
        {
            mKarmaSpent = 0;
            //mKarmaEarned = 0;
            foreach (IKarmaCost item in KarmaCosts)
            {
                mKarmaSpent += item.Karma;
            }
            OnPropertyChanged(nameof(KarmaSpent));
            OnPropertyChanged(nameof(KarmaAvailable));
        }

        private void OnSpellsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // keep KarmaCosts up to date
            if (e.OldItems != null)
                foreach (KeyValuePair<String, Spell> kvp in e.OldItems)
                    KarmaCosts.Remove(kvp.Value);

            if (e.NewItems != null)
                foreach (KeyValuePair<String, Spell> kvp in e.NewItems)
                    KarmaCosts.Add(kvp.Value);
        }

        private void RecalcFreeSpells()
        {
            int freeCount = SpecialChoice.Spells;
            foreach (var spell in SpellList.Values)
            {
                if (freeCount > 0)
                {
                    spell.Free = true;
                }
                else
                {
                    spell.Free = false;
                }
            }
            RecalcKarmaCost();
        }

        private void OnImplantsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (KeyValuePair<string, Implant> kvp in e.OldItems)
                    kvp.Value.PropertyChanged -= OnGearChanged;

            if (e.NewItems != null)
                foreach (KeyValuePair<String, Implant> kvp in e.NewItems)
                    kvp.Value.PropertyChanged += OnGearChanged;

            RecalcMoney();
        }

        private void OnGearCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (KeyValuePair<string, Gear> kvp in e.OldItems)
                    kvp.Value.PropertyChanged -= OnGearChanged;

            if (e.NewItems != null)
                foreach (KeyValuePair<String, Gear> kvp in e.NewItems)
                    kvp.Value.PropertyChanged += OnGearChanged;

            RecalcMoney();
        }

        private void OnGearChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Gear.Cost))
            {
                RecalcMoney();
            }
        }

        private void RecalcMoney()
        {
            mMoneySpent = 0;
            foreach (Gear item in GearList.Values)
            {
                mMoneySpent += item.Cost;
            }
            foreach (Gear item in ImplantList)
            {
                mMoneySpent += item.Cost;
            }
            OnPropertyChanged(nameof(MoneySpent));
            OnPropertyChanged(nameof(MoneyRemaining));
        }

        #endregion // Private Methods
    }
}
