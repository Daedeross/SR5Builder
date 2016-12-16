using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrWPF.Windows.Data;
using System.Collections.Specialized;
using SR5Builder.Prototypes;

namespace SR5Builder.DataModels
{
    //public enum Restriction
    //{
    //    N = 0,
    //    R = 1,
    //    F = 2,
    //}

    public class Gear: LeveledTrait, IEssenceCost, IArmor
    {
        public bool IsArmor { get; protected set; }

        #region Properties

        public bool HasRating { get; set; }

        public override int BaseRating
        {
            get { return base.BaseRating; }
            set
            {
                base.BaseRating = value;
                OnPropertyChanged(nameof(Availability));
                OnPropertyChanged(nameof(Cost));
                OnPropertyChanged(nameof(TotalEssence));
                if (IsArmor)
                {
                    OnPropertyChanged(nameof(ArmorRating));
                }
            }
        }

        private int mBonusArmor;
        public int ArmorRating
        {
            get
            {
                return IsArmor ? BaseRating + mBonusArmor: 0;
            }
        }

        public virtual bool IsClothing
        {
            get { return true; }
        }

        protected Availability[] mAvailabilityVector;
        public Availability BaseAvailability
        {
            get
            {
                int index = BaseRating;
                if (index >= mAvailabilityVector.Length)
                {
                    index = mAvailabilityVector.Length - 1;
                }
                return mAvailabilityVector[index];
            }
        }

        protected Availability mBonusAvailability;
        public virtual Availability Availability
        {
            get { return BaseAvailability + mBonusAvailability; }
        }

        private int mCount = 1;
        public int Count
        {
            get { return mCount; }
            set
            {
                if (value != mCount)
                {
                    mCount = value;
                    OnPropertyChanged(nameof(Count));
                    OnPropertyChanged(nameof(Cost));
                }
            }
        }

        protected decimal[] mCostVector;
        public decimal[] CostVector { get { return mCostVector; } }

        public decimal BaseCost
        {
            get
            {
                int index = BaseRating;
                if (index >= mCostVector.Length)
                {
                    index = mCostVector.Length - 1;
                }
                return mCostVector[index];
            }
        }

        private decimal mExtraCost;
        private decimal mCostMult = 1;
        public virtual decimal Cost
        {
            get { return ((BaseCost * mCostMult) + mExtraCost) * Count; }
        }

        protected decimal[] mEssenceVector;
        public decimal[] EssenceVector { get { return mEssenceVector; } }

        public virtual decimal TotalEssence
        {
            get
            {
                int index = BaseRating;
                if (index >= mEssenceVector.Length)
                {
                    index = mEssenceVector.Length - 1;
                }
                return mEssenceVector[index];
            }
        }

        //public string DisplayCost { get { return Cost.ToString("C", GlobalData.CostFormat); } }
        protected int[] mCapacityVector;
        public int[] CapacityVector { get { return mCapacityVector; } }

        public int Capacity
        {
            get
            {
                int index = BaseRating;
                if (index >= mCapacityVector.Length)
                {
                    index = mCapacityVector.Length - 1;
                }
                return mCapacityVector[index];
            }
        }

        private int mCapacityUsed;
        public int CapacityUsed
        {
            get { return mCapacityUsed; }
            set
            {
                if (mCapacityUsed != value)
                {
                    mCapacityUsed = value;
                    OnPropertyChanged(nameof(CapacityUsed));
                }
            }
        }

        public int CapacityRemaining
        {
            get { return Capacity - mCapacityUsed; }
        }

        public string DisplayCapacity
        {
            get
            {
                if (Capacity != 0)
                {
                    return mCapacityUsed + "/" + mCapacityUsed;
                }
                else return "n/a";
            }
        }

        public Dictionary<string, GearMod> BaseMods { get; set; }

        public List<string> AvailableMods { get; set; }

        public List<string> ModCategories { get; set; }

        public ObservableDictionary<string, GearMod> Mods { get; set; }

        public string Notes { get; set; }

        public override string ToString()
        {
            return BaseRating != 0 ? base.ToString() : "";
        }

        #endregion // Properties

        #region Constructors

        public Gear(SR5Character owner)
            : base(owner)
        {
            Initialize();
            Notes = "";
        }

        public Gear(SR5Character owner, GearPrototype proto)
            : base(owner)
        {
            Initialize();

            CopyFromPrototype(proto);
        }

        private void Initialize()
        {
            BaseMods = new Dictionary<string, GearMod>();
            AvailableMods = new List<string>();
            ModCategories = new List<string>();
            Mods = new ObservableDictionary<string, GearMod>();
            Mods.CollectionChanged += this.OnModsChanged;
        }

        #endregion // Constructors

        #region Private Methods

        private void OnModsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (KeyValuePair<string, GearMod> kvp in e.OldItems)
                {
                    mExtraCost -= kvp.Value.FlatCost;
                    decimal mult = kvp.Value.CostMult;
                    mCostMult /= mult > 0 ? mult : 1;
                    mCapacityUsed -= kvp.Value.Capacity;
                }
            }

            if (e.NewItems != null)
            {
                foreach (KeyValuePair<string, GearMod> kvp in e.NewItems)
                {
                    mExtraCost += kvp.Value.FlatCost;
                    decimal mult = kvp.Value.CostMult;
                    mCostMult *= mult > 0 ? mult : 1;
                    mCapacityUsed += kvp.Value.Capacity;
                }
            }

            OnPropertyChanged(nameof(Cost));
            OnPropertyChanged(nameof(CapacityUsed));
        }

        private void LoadBaseMods(string[] modNames)
        {
            if (modNames != null)
            {
                foreach (string name in modNames)
                {
                    GearModPrototype l;
                    if (GlobalData.GearMods.TryGetValue(name, out l))
                    {
                        BaseMods.Add(name, l.ToMod(this));
                    }
                }
            }
        }

        protected override void RecalcBonus(HashSet<string> propNames = null)
        {
            // Reset avail
            mBonusAvailability = Availability.Zero;
            mBonusArmor = 0;

            // Do for base
            // This should call the vitual AddAugment() method for each augment
            base.RecalcBonus(propNames);
        }

        protected override HashSet<string> AddAugment(Augment a, HashSet<string> propNames)
        {
            switch (a.Kind)
            {
                case AugmentKind.None:
                    break;
                case AugmentKind.Rating:
                    return base.AddAugment(a, propNames);
                case AugmentKind.DamageValue:
                    break;
                case AugmentKind.DamageType:
                    break;
                case AugmentKind.Accuracy:
                    break;
                case AugmentKind.Availability:
                    mBonusAvailability.Level += (int)a.Bonus;
                    propNames.Add(nameof(Availability));
                    break;
                case AugmentKind.Restriction:
                    mBonusAvailability.Restriction = (Restriction)Math.Max((int)a.Bonus, (int)mBonusAvailability.Restriction);
                    propNames.Add(nameof(Availability));
                    break;
                case AugmentKind.Armor:
                    mBonusArmor += (int)a.Bonus;
                    propNames.Add(nameof(ArmorRating));
                    break;
                default:
                    break;
            }

            return propNames;
        }

        #endregion // Private Methods

        #region Public Methods

        private bool AddMod(string modName)
        {
            if (!BaseMods.ContainsKey(modName) && !Mods.ContainsKey(modName))
            {
                GearModPrototype l;
                if (GlobalData.GearMods.TryGetValue(modName, out l))
                {
                    Mods.Add(modName, l.ToMod(this));
                }
            }
            return false;
        }

        public virtual void CopyFromPrototype(GearPrototype proto)
        {
            mName = proto.Name;
            Book = proto.Book;
            Page = proto.Page;
            IsArmor = proto.IsArmor;
            mBaseRating = proto.Rating;
            HasRating = proto.Min != 0;
            // load max and min rating if applicable
            Min = proto.Min != 0 ? proto.Min : proto.Rating;
            Max = proto.Max != 0 ? proto.Max : proto.Rating;
            mCostVector = new decimal[proto.CostVector.Length];
            mEssenceVector = new decimal[proto.EssenceVector.Length];
            mCapacityVector = new int[proto.CapacityVector.Length];
            mAvailabilityVector = new Availability[proto.AvailabilityVector.Length];
            proto.CostVector.CopyTo(mCostVector, 0);
            proto.EssenceVector.CopyTo(mEssenceVector, 0);
            proto.CapacityVector.CopyTo(mCapacityVector, 0);
            proto.AvailabilityVector.CopyTo(mAvailabilityVector, 0);
            Count = 1;
            LoadBaseMods(proto.BaseMods);
            if (proto.Mods != null)
            {
                foreach (string mod in proto.Mods)
                {
                    AvailableMods.Add((string)mod.Clone());
                }
            }
            ModCategories = (proto.ModCategories ?? new string[0]).ToList();
            Notes = proto.Notes ?? "";
        }

        #endregion // Public Methods
    }
}
