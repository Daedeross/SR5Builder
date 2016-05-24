using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DrWPF.Windows.Data;
using System.Collections.Specialized;
using SR5Builder.Loaders;

namespace SR5Builder.DataModels
{
    //public enum Restriction
    //{
    //    N = 0,
    //    R = 1,
    //    F = 2,
    //}

    public class Gear: LeveledTrait, IEssenceCost
    {
        #region Properties

        public bool HasRating { get; set; }

        protected Availability mBaseAvailability;
        public Availability BaseAvailability
        {
            get { return mBaseAvailability; }
            set
            {
                mBaseAvailability = value;
                RaisePropertyChanged(nameof(BaseAvailability));
                RaisePropertyChanged(nameof(Availability));
            }
        }

        protected Availability mBonusAvailability;
        public Availability Availability
        {
            get { return mBaseAvailability + mBonusAvailability; }
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
                    RaisePropertyChanged(nameof(Count));
                    RaisePropertyChanged(nameof(Cost));
                }
            }
        }

        private decimal mFlatCost;
        public decimal FlatCost
        {
            get { return mFlatCost; }
            set
            {
                if (value != mFlatCost)
                {
                    mFlatCost = value;
                    RaisePropertyChanged(nameof(FlatCost));
                    RaisePropertyChanged(nameof(BaseCost));
                    RaisePropertyChanged(nameof(Cost));
                }
            }
        }

        protected decimal mRatingCost;
        public decimal RatingCost
        {
            get { return mRatingCost; }
            set
            {
                if (value != mRatingCost)
                {
                    mRatingCost = value;
                    RaisePropertyChanged(nameof(RatingCost));
                    RaisePropertyChanged(nameof(BaseCost));
                    RaisePropertyChanged(nameof(Cost));
                }
            }
        }

        public decimal BaseCost
        {
            get { return mFlatCost + (HasRating ? mRatingCost * BaseRating : 0); }
        }

        private decimal mExtraCost;
        private decimal mCostMult = 1;
        public decimal Cost
        {
            get { return ((BaseCost * mCostMult) + mExtraCost) * Count; }
        }

        private decimal mFlatEssence;
        public decimal FlatEssence
        {
            get { return mFlatEssence; }
            set
            {
                if (value != mFlatEssence)
                {
                    mFlatEssence = value;
                    RaisePropertyChanged(nameof(FlatEssence));
                    RaisePropertyChanged(nameof(TotalEssence));
                }
            }
        }

        private decimal mRatingEssence;
        public decimal RatingEssence
        {
            get { return mRatingEssence; }
            set { mRatingEssence = value; }
        }

        public decimal TotalEssence
        {
            get { return mFlatEssence + mRatingEssence * mBaseRating; }
        }

        //public string DisplayCost { get { return Cost.ToString("C", GlobalData.CostFormat); } }

        protected int mCapacity;
        public int Capacity
        {
            get { return mCapacity; }
            set
            {
                if (mCapacity != value)
                {
                    mCapacity = value;
                    RaisePropertyChanged(nameof(Capacity));
                    RaisePropertyChanged(nameof(CapacityUsed));
                }
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
                    RaisePropertyChanged(nameof(CapacityUsed));
                }
            }
        }

        public int CapacityRemaining
        {
            get { return mCapacity - mCapacityUsed; }
        }

        public string DisplayCapacity
        {
            get
            {
                if (mCapacity != 0)
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

        #endregion // Properties

        #region Constructors

        public Gear(SR5Character owner)
            : base(owner)
        {
            Initialize();
            Notes = "";
        }

        public Gear(SR5Character owner, GearLoader loader)
            : base(owner)
        {
            Initialize();

            CopyFromLoader(loader);
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

            RaisePropertyChanged(nameof(Cost));
            RaisePropertyChanged(nameof(CapacityUsed));
        }

        private void LoadBaseMods(string[] modNames)
        {
            if (modNames != null)
            {
                foreach (string name in modNames)
                {
                    GearModLoader l;
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
                    propNames.Add("Availability");
                    break;
                case AugmentKind.Restriction:
                    mBonusAvailability.Restriction = (Restriction)Math.Max((int)a.Bonus, (int)mBonusAvailability.Restriction);
                    propNames.Add("Availability");
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
                GearModLoader l;
                if (GlobalData.GearMods.TryGetValue(modName, out l))
                {
                    Mods.Add(modName, l.ToMod(this));
                }
            }
            return false;
        }

        public virtual void CopyFromLoader(GearLoader loader)
        {
            mName = loader.Name;
            Book = loader.Book;
            Page = loader.Page;
            mBaseRating = loader.Rating;
            HasRating = true;           // I think technically all gear has a rating of at least 1
            // load max and min rating if applicable
            Min = loader.Min != 0 ? loader.Min : loader.Rating;
            Max = loader.Max != 0 ? loader.Max : loader.Rating;
            mBaseAvailability = loader.Availability;
            mFlatCost = loader.FlatCost;
            mRatingCost = loader.RatingCost;
            mCapacity = loader.Capacity;
            mFlatEssence = loader.FlatEssence;
            mRatingEssence = loader.RatingEssence;
            Count = 1;
            LoadBaseMods(loader.BaseMods);
            if (loader.Mods != null)
            {
                foreach (string mod in loader.Mods)
                {
                    AvailableMods.Add((string)mod.Clone());
                }
            }
            ModCategories = (loader.ModCategories ?? new string[0]).ToList();
            Notes = loader.Notes ?? "";
        }

        #endregion // Public Methods
    }
}
