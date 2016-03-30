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

namespace SR5Builder.DataModels
{
    public class SR5Character : DataModelBase
    {
        public static readonly int MaxAugment = 4;

        #region Private Fields

        #endregion // Private Fields

        #region Properties

        /// <summary>
        /// Holds all the traits on the character which are augmentable
        /// (i.e. implement the IAugemntable interface).
        /// </summary>
        public ObservableDictionary<string, IAugmentable> Augmentables { get; set; }

        public ObservableCollection<IAugmentContainer> AugmentContainers { get; set; }

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
                        OnPropertyChanged("Metatype");
                        OnPropertyChanged("MetatypeStats");
                        //OnPropertyChanged("SpecialAttributePointsRemaining");
                        //RefreshAttributes();
                    }
                }
                OnPropertyChanged("SpecialAttributePoints");
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
                    OnPropertyChanged("Body");
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
                    OnPropertyChanged("Agility");
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
                    OnPropertyChanged("Reaction");
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
                    OnPropertyChanged("Strength");
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
                    OnPropertyChanged("Willpower");
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
                    OnPropertyChanged("Logic");
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
                    OnPropertyChanged("Intuition");
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
                    OnPropertyChanged("Charisma");
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
                    OnPropertyChanged("Edge");
                }
            }
        }

        private Attribute mEssence;
        public Attribute Essence
        {
            get { return mEssence; }
            set
            {
                if (value != mEssence)
                {
                    mEssence = value;
                    OnPropertyChanged("Essence");
                }
            }
        }

            #region BaseAttributes



        //private LeveledTrait mBody;
        //public int BaseBody
        //{
        //    get { return mBody.BaseRating + MetatypeStats.BodyMin; }
        //    set
        //    {
        //        if (   value >= MetatypeStats.BodyMin
        //            && value <= MetatypeStats.BodyMax)
        //        {
        //            mBody.BaseRating = value - MetatypeStats.BodyMin;
        //            OnPropertyChanged("BaseBody");
        //            OnPropertyChanged("AugmentedBody");
        //        }
        //    }
        //}

        //private LeveledTrait mAgility;
        //public int BaseAgility
        //{
        //    get { return mAgility.BaseRating + MetatypeStats.AgilityMin; }
        //    set
        //    {
        //        if (value >= MetatypeStats.AgilityMin
        //            && value <= MetatypeStats.AgilityMax)
        //        {
        //            mAgility.BaseRating = value - MetatypeStats.AgilityMin;
        //            OnPropertyChanged("BaseAgility");
        //            OnPropertyChanged("AugmentedAgility");
        //        }
        //    }
        //}

        //private LeveledTrait mReaction;
        //public int BaseReaction
        //{
        //    get { return mReaction.BaseRating + MetatypeStats.ReactionMin; }
        //    set
        //    {
        //        if (value >= MetatypeStats.ReactionMin
        //            && value <= MetatypeStats.ReactionMax)
        //        {
        //            mReaction.BaseRating = value - MetatypeStats.ReactionMin;
        //            OnPropertyChanged("BaseReaction");
        //            OnPropertyChanged("AugmentedReaction");
        //        }
        //    }
        //}

        //private LeveledTrait mStrength;
        //public int BaseStrength
        //{
        //    get { return mStrength.BaseRating + MetatypeStats.StrengthMin; }
        //    set
        //    {
        //        if (value >= MetatypeStats.StrengthMin
        //            && value <= MetatypeStats.StrengthMax)
        //        {
        //            mStrength.BaseRating = value - MetatypeStats.StrengthMin;
        //            OnPropertyChanged("BaseStrength");
        //            OnPropertyChanged("AugmentedStrength");
        //        }
        //    }
        //}

        //private LeveledTrait mWillpower;
        //public int BaseWillpower
        //{
        //    get { return mWillpower.BaseRating + MetatypeStats.WillpowerMin; }
        //    set
        //    {
        //        if (value >= MetatypeStats.WillpowerMin
        //            && value <= MetatypeStats.WillpowerMax)
        //        {
        //            mWillpower.BaseRating = value - MetatypeStats.WillpowerMin;
        //            OnPropertyChanged("BaseWillpower");
        //            OnPropertyChanged("AugmentedWillpower");
        //        }
        //    }
        //}

        //private LeveledTrait mLogic;
        //public int BaseLogic
        //{
        //    get { return mLogic.BaseRating + MetatypeStats.LogicMin; }
        //    set
        //    {
        //        if (value >= MetatypeStats.LogicMin
        //            && value <= MetatypeStats.LogicMax)
        //        {
        //            mLogic.BaseRating = value - MetatypeStats.LogicMin;
        //            OnPropertyChanged("BaseLogic");
        //            OnPropertyChanged("AugmentedLogic");
        //        }
        //    }
        //}

        //private LeveledTrait mIntuition;
        //public int BaseIntuition
        //{
        //    get { return mIntuition.BaseRating + MetatypeStats.IntuitionMin; }
        //    set
        //    {
        //        if (value >= MetatypeStats.IntuitionMin
        //            && value <= MetatypeStats.IntuitionMax)
        //        {
        //            mIntuition.BaseRating = value - MetatypeStats.IntuitionMin;
        //            OnPropertyChanged("BaseIntuition");
        //            OnPropertyChanged("AugmentedIntuition");
        //        }
        //    }
        //}

        //private LeveledTrait mCharisma;
        //public int BaseCharisma
        //{
        //    get { return mCharisma.BaseRating + MetatypeStats.CharismaMin; }
        //    set
        //    {
        //        if (value >= MetatypeStats.CharismaMin
        //            && value <= MetatypeStats.CharismaMax)
        //        {
        //            mCharisma.BaseRating = value - MetatypeStats.CharismaMin;
        //            OnPropertyChanged("BaseCharisma");
        //            OnPropertyChanged("AugmentedCharisma");
        //        }
        //    }
        //}

        //private LeveledTrait mEdge;
        //public int BaseEdge
        //{
        //    get { return mEdge.BaseRating + MetatypeStats.EdgeMin; }
        //    set
        //    {
        //        if (value >= MetatypeStats.EdgeMin
        //            && value <= MetatypeStats.EdgeMax)
        //        {
        //            mEdge.BaseRating = value - MetatypeStats.EdgeMin;
        //            OnPropertyChanged("BaseEdge");
        //            OnPropertyChanged("SpecialAttributePointsSpent");
        //        }
        //    }
        //}

        //private double mEssence = 0;
        //public double BaseEssence
        //{
        //    get { return mEssence; }
        //    set
        //    {
        //        if (value <= 0)
        //        {
        //            mEssence = value;
        //            OnPropertyChanged("BaseEssence");
        //        }
        //    }
        //}
        //    #endregion // BaseAttributes

        //    #region BonusAttributes

        //private int mBonusBody;
        //public int BonusBody
        //{
        //    get { return mBonusBody; }
        //    set
        //    {
        //        if (mBonusBody != value &&
        //            value <= MaxAugment &&
        //            value > -1)
        //        {
        //            mBonusBody = value;
        //            OnPropertyChanged("BonusBody");
        //            OnPropertyChanged("AugmentedBody");
        //        }
        //    }
        //}

        //private int mBonusAgility;
        //public int BonusAgility
        //{
        //    get { return mBonusAgility; }
        //    set
        //    {
        //        if (mBonusAgility != value &&
        //            value <= MaxAugment &&
        //            value > -1)
        //        {
        //            mBonusAgility = value;
        //            OnPropertyChanged("BonusAgility");
        //            OnPropertyChanged("AugmentedAgility");
        //        }
        //    }
        //}
        
        //private int mBonusReaction;
        //public int BonusReaction
        //{
        //    get { return mBonusReaction; }
        //    set
        //    {
        //        if (mBonusReaction != value &&
        //            value <= MaxAugment &&
        //            value > -1)
        //        {
        //            mBonusReaction = value;
        //            OnPropertyChanged("BonusReaction");
        //            OnPropertyChanged("AugmentedReaction");
        //        }
        //    }
        //}

        //private int mBonusStrength;
        //public int BonusStrength
        //{
        //    get { return mBonusStrength; }
        //    set
        //    {
        //        if (mBonusStrength != value &&
        //            value <= MaxAugment &&
        //            value > -1)
        //        {
        //            mBonusStrength = value;
        //            OnPropertyChanged("BonusStrength");
        //            OnPropertyChanged("AugmentedStrength");
        //        }
        //    }
        //}

        //private int mBonusWillpower;
        //public int BonusWillpower
        //{
        //    get { return mBonusWillpower; }
        //    set
        //    {
        //        if (mBonusWillpower != value &&
        //            value <= MaxAugment &&
        //            value > -1)
        //        {
        //            mBonusWillpower = value;
        //            OnPropertyChanged("BonusWillpower");
        //            OnPropertyChanged("AugmentedWillpower");
        //        }
        //    }
        //}

        //private int mBonusLogic;
        //public int BonusLogic
        //{
        //    get { return mBonusLogic; }
        //    set
        //    {
        //        if (mBonusLogic != value &&
        //            value <= MaxAugment &&
        //            value > -1)
        //        {
        //            mBonusLogic = value;
        //            OnPropertyChanged("BonusLogic");
        //            OnPropertyChanged("AugmentedLogic");
        //        }
        //    }
        //}

        //private int mBonusIntuition;
        //public int BonusIntuition
        //{
        //    get { return mBonusIntuition; }
        //    set
        //    {
        //        if (mBonusIntuition != value &&
        //            value <= MaxAugment &&
        //            value > -1)
        //        {
        //            mBonusIntuition = value;
        //            OnPropertyChanged("BonusIntuition");
        //            OnPropertyChanged("AugmentedIntuition");
        //        }
        //    }
        //}

        //private int mBonusCharisma;
        //public int BonusCharisma
        //{
        //    get { return mBonusCharisma; }
        //    set
        //    {
        //        if (mBonusCharisma != value &&
        //            value <= MaxAugment &&
        //            value > -1)
        //        {
        //            mBonusCharisma = value;
        //            OnPropertyChanged("BonusCharisma");
        //            OnPropertyChanged("AugmentedCharisma");
        //        }
        //    }
        //}

        //    #endregion // BonusAttributes

        //    #region AugmentedAttributes

        //public int AugmentedBody
        //{
        //    get { return BaseBody + mBonusBody; }
        //}

        //public int AugmentedAgility
        //{
        //    get { return BaseAgility + mBonusAgility; }
        //}

        //public int AugmentedReaction
        //{
        //    get { return BaseReaction + mBonusReaction; }
        //}

        //public int AugmentedStrength
        //{
        //    get { return BaseStrength + mBonusStrength; }
        //}

        //public int AugmentedWillpower
        //{
        //    get { return BaseWillpower + mBonusWillpower; }
        //}

        //public int AugmentedLogic
        //{
        //    get { return BaseLogic + mBonusLogic; }
        //}

        //public int AugmentedIntuition
        //{
        //    get { return BaseIntuition + mBonusIntuition; }
        //}

        //public int AugmentedCharisma
        //{
        //    get { return BaseCharisma + mBonusCharisma; }
        //}


            #endregion // AugmentedAttributes

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

        public Initiative ColdSimInititative { get; set; }

        public Initiative HotSimInititative { get; set; }

        public Initiative AstralInititative { get; set; }

                #endregion // Initiatives

                #region Limits

        public Limit MentalLimit { get; private set; }

        public Limit PhysicalLimit { get; private set; }

        public Limit SocialLimit { get; private set; }

        public Limit AstraLimit
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
                    mSpecialAttribute.BaseRating = SpecialAttribute;
                    OnPropertyChanged("SpecialChoice");
                    OnPropertyChanged("SpecialKind");
                    OnPropertyChanged("SpecialAttribute");
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
        public int SpecialAttribute
        {
            get
            {
                if (mSpecialChoice != null && mSpecialChoice.Attribute > 0)
                {
                    //Debug.WriteLine(mSpecialChoice.Attribute);
                    return mSpecialAttribute.BaseRating;
                }
                else return 0;

            }
            set
            {
                if (SpecialChoice == null || SpecialChoice.Attribute == 0)
                {
                    mSpecialAttribute.BaseRating = 0;
                }
                else if (mSpecialAttribute.BaseRating != value)
                {
                    mSpecialAttribute.BaseRating = value;
                    OnPropertyChanged("SpecialAttribute");
                    OnPropertyChanged("SpecialAttributePointsSpent");
                }
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

            #region Skills

        public ObservableDictionary<string, Skill> SkillList { get; set; }

        public ObservableDictionary<string, SkillGroup> SkillGroupsList { get; set; }

        public int SkillPointsSpent
        {
            get
            {
                int points = 0;
                foreach (Skill skill in SkillList)
                {
                    points += skill.Points;
                }
                return points;
            }
        }

        public int SkillGroupPointsSpent
        {
            get
            {
                int points = 0;
                foreach (SkillGroup skillGroup in SkillGroupsList)
                {
                    points += skillGroup.Points;
                }
                return points;
            }
        }

            #endregion // Skills

            #region Spells / Powers

        public ObservableDictionary<string, Spell> SpellList { get; set; }

        public ObservableDictionary<string, AdeptPower> PowerList { get; set; }

            #endregion // Spells / Powers

            #region Gear

        public ObservableDictionary<string, Gear> GearList { get; set; }

        public ObservableDictionary<string, Implant> ImplantList { get; set; }

            #endregion // Gear

            #region Weapons

        public ObservableDictionary<string, MeleeWeapon> MeleeWeapons { get; set; }

        public ObservableDictionary<string, RangedWeapon> RangedWeapons { get; set; }

        //public ObservableDictionary<string, RangedWeapon> ProjectileWeapons { get; set; }

            #endregion // Weapons

        #endregion // Properties

        #region Constructors

        public SR5Character()
        {
            Priorities = new Priorities();
            MetatypeStats = new MetatypeStats();
            Metatype = "Human";

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
            PowerList = new ObservableDictionary<string, AdeptPower>();
            PowerList.CollectionChanged += this.OnAugmentablesChanged;
                // Even though Adept powers never recieve Augments,
                // this is needed to remove augments when powers are removed

            GearList = new ObservableDictionary<string, Gear>();
            GearList.CollectionChanged += this.OnAugmentablesChanged;

            ImplantList = new ObservableDictionary<string, Implant>();
            ImplantList.CollectionChanged += this.OnAugmentablesChanged;
            ImplantList.CollectionChanged += this.OnImplantsChanged;

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

            mEssence = new Attribute(this, "Essence");

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

            // Add to Augmentables and PropertyChanged listener
            foreach (Attribute a in Attributes.Values)
            {
                a.PropertyChanged += this.OnAttributeChanged;
                Augmentables.Add(a.Name, a);
            }

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
                case "Metatype":
                    if (GlobalData.PriorityLevels[Priorities.Metatype].Metatypes.Contains(this.mMetatype))
                        Metatype = mMetatype;
                    else
                        Metatype = "Human";
                    break;
                case "Special":
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

                    mSpecialAttribute.Name = SpecialKind.ToString();

                    OnPropertyChanged("SpecialAttriute");
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
        }

        private void OnSkillChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("SkillPointsSpent");
        }

        private void OnSkillGroupListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (KeyValuePair<string, SkillGroup> kvp in e.OldItems)
                    kvp.Value.PropertyChanged -= this.OnSkillGroupChanged;
            if (e.NewItems != null)
                foreach (KeyValuePair<string, SkillGroup> kvp in e.NewItems)
                    kvp.Value.PropertyChanged += this.OnSkillGroupChanged;
        }

        private void OnSkillGroupChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("SkillGroupPointsSpent");
        }

        private void OnAttributeChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Points")
            {
                if (sender == mSpecialAttribute || sender == mEdge)
                    OnPropertyChanged("SpecialAttributePointsSpent");
                else
                    OnPropertyChanged("AttributePointsSpent");
            }
        }

        private void OnImplantsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Essence");
        }

        /// <summary>
        /// Keeps the dictionary containing all Augmentables up to date as Traits are
        /// added and removed from other lists/dictionarys on the character
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAugmentablesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            HashSet<string> propNames = new HashSet<string>();
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

                            //if (a is MeleeWeapon)
                            //{
                            //    MeleeWeapons.Remove((string)valueType.GetProperty("Key").GetValue(item, null));
                            //    propNames.Add("MeleeWeapons");
                            //}

                            //if (a is RangedWeapon)
                            //{
                            //    RangedWeapons.Remove((string)valueType.GetProperty("Key").GetValue(item, null));
                            //    propNames.Add("RangedWeapons");
                            //}
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

                            // Upadate weapons lists
                            //if (a is MeleeWeapon)
                            //{
                            //    MeleeWeapons.Add((string)valueType.GetProperty("Key").GetValue(item, null), (a as MeleeWeapon));
                            //    propNames.Add("MeleeWeapons");
                            //}

                            //if (a is RangedWeapon)
                            //{
                            //    RangedWeapons.Add((string)valueType.GetProperty("Key").GetValue(item, null), (a as RangedWeapon));
                            //    propNames.Add("RangedWeapons");
                            //}
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

        #endregion // Private Methods
    }
}
