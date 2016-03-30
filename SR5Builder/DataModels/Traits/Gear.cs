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

    public class Gear: LeveledTrait
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        public bool HasRating { get; set; }

        protected Availability mBaseAvailability;
        public Availability BaseAvailability
        {
            get { return mBaseAvailability; }
            set
            {
                mBaseAvailability = value;
                OnPropertyChanged("BaseAvailability");
                OnPropertyChanged("Availability");
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
                    OnPropertyChanged("Count");
                    OnPropertyChanged("Cost");
                    OnPropertyChanged("DisplayCost");
                }
            }
        }

        protected int mBaseCost;
        public int BaseCost
        {
            get { return mBaseCost; }
            set
            {
                if (value != mBaseCost)
                {
                    mBaseCost = value;
                    OnPropertyChanged("BaseCost");
                    OnPropertyChanged("Cost");
                    OnPropertyChanged("DisplayCost");
                }
            }
        }

        private int mExtraCost;
        public int Cost
        {
            get { return (mBaseCost + mExtraCost) * Count; }
        }

        public string DisplayCost { get { return Cost + "¥"; } }

        protected int mCapacity;
        public int Capacity
        {
            get { return mCapacity; }
            set
            {
                if (mCapacity != value)
                {
                    mCapacity = value;
                    OnPropertyChanged("Capacity");
                    OnPropertyChanged("CapacityUsed");
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
                    OnPropertyChanged("CapacityUsed");
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

        public ObservableDictionary<string, GearMod> Mods { get; set; }

        #endregion // Properties

        #region Constructors

        public Gear(SR5Character owner)
            : base(owner)
        {
            Initialize();
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
            Mods = new ObservableDictionary<string, GearMod>();
            Mods.CollectionChanged += this.OnModsChanged;
            AvailableMods = new List<string>();
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
                    mCapacityUsed -= kvp.Value.Capacity;
                }
            }

            if (e.NewItems != null)
            {
                foreach (KeyValuePair<string, GearMod> kvp in e.NewItems)
                {
                    mExtraCost += kvp.Value.FlatCost;
                    mCapacityUsed += kvp.Value.Capacity;
                }
            }

            OnPropertyChanged("Cost");
            OnPropertyChanged("DisplayCost");
            OnPropertyChanged("CapacityUsed");
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
            Min = loader.Rating;
            Max = loader.Rating;
            mBaseAvailability = loader.Availability;
            mBaseCost = loader.Cost;
            mCapacity = loader.Capacity;
            Count = 1;
            LoadBaseMods(loader.BaseMods);
            if (loader.Mods != null)
            {
                foreach (string mod in loader.Mods)
                {
                    AvailableMods.Add((string)mod.Clone());
                }
            }
        }

        #endregion // Public Methods
    }
}
