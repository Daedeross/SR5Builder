using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public abstract class LeveledTrait: BaseTrait, IAugmentable, IComparable<LeveledTrait>, IComparable<int>
    {
        #region Properties
        
        public virtual int ExtraMin { get; set; }
        public virtual int Min { get; set; }

        public virtual int ExtraMax { get; set; }
        public virtual int Max { get; set; }

        public string[] LevelNames { get; set; }

        protected int mBaseRating;
        public virtual int BaseRating
        {
            get { return mBaseRating; }
            set
            {
                if (mBaseRating != value)
                {
                    mBaseRating = value;
                    OnPropertyChanged(nameof(BaseRating));
                    OnPropertyChanged(nameof(AugmentedRating));
                    OnPropertyChanged(nameof(Points));
                    OnPropertyChanged(nameof(DisplayValue));
                }
            }
        }

        protected decimal mBonusRating;
        public virtual int BonusRating
        {
            get
            {
                if(mBonusRating > mOwner.Settings.MaxAugment)
                    mBonusRating = mOwner.Settings.MaxAugment;
                if (mBaseRating < 0)
                    mBonusRating = 0;
                return (int)mBonusRating;
            }
            set
            {
                if ((int)mBonusRating != value)
                {
                    mBonusRating = value;
                    OnPropertyChanged(nameof(BonusRating));
                    OnPropertyChanged(nameof(AugmentedRating));
                }
            }
        }

        protected int mImprovement;
        public virtual int ImprovedRating
        {
            get { return mImprovement + BaseRating; }
            set
            {
                if (value <= Max && value >= Min)
                {
                    mImprovement = value - BaseRating;
                }
            }
        }

        public override int Points
        {
            get
            {
                return mBaseRating;
            }
            set
            {
                base.Points = value;
            }
        }

        public virtual int AugmentedRating
        {
            get { return ImprovedRating + BonusRating; }
        }

        public virtual string DisplayValue
        {
            get
            {
                int i = AugmentedRating;
                if (   LevelNames != null
                    && LevelNames.Length > 0
                    && i < LevelNames.Length
                    && i >= 0)
                {
                    return ToString() + $" [{LevelNames[i]}]";
                }
                else
                {
                    return ToString();
                }
            }
        }

        #endregion Properties

        #region Constructors

        public LeveledTrait()
            :base()
        {
            Initialize();
        }

        public LeveledTrait(SR5Character owner)
            :base(owner)
        {
            Initialize();
        }

        private void Initialize()
        {
            Augments = new ObservableCollection<Augment>();
            Augments.CollectionChanged += this.OnAugmentCollectionChanged;
        }

        #endregion // Contsructors

        public override string ToString()
        {
            if (mBonusRating != 0)
                return ImprovedRating + $" ({AugmentedRating})";
            else
                return ImprovedRating.ToString();
        }

        #region IAugmentable Implemenation

        //protected ObservableCollection<Augment> mAugments;
        public ObservableCollection<Augment> Augments { get; set; }

        public HashSet<string> RemovedNames { get; set; } = new HashSet<string>();

        public void OnAugmentChanged(object sender, PropertyChangedEventArgs e)
        {
            RecalcBonus();
        }

        public void OnAugmentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            HashSet<string> propNames = new HashSet<string>();
            if (e.OldItems != null)
            {
                foreach (Augment a in e.OldItems)
                {
                    propNames.Add(a.TargetName);
                    a.PropertyChanged -= this.OnAugmentChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (Augment a in e.NewItems)
                {
                    a.PropertyChanged += this.OnAugmentChanged;
                }
            }

            RecalcBonus(propNames);
        }

        protected virtual void RecalcBonus(HashSet<string> propNames = null)
        {
            //create HashSet if needed
            if (propNames == null)
                propNames = new HashSet<string>();

            foreach (string name in RemovedNames)
            {
                propNames.Add(name);
            }
            RemovedNames.Clear();

            //Clear Bonus
            ExtraMax = 0;
            ExtraMin = 0;
            mBonusRating = 0;

            foreach (Augment a in Augments)
                propNames = AddAugment(a, propNames);

            if (mBonusRating > mOwner.Settings.MaxAugment)
                mBonusRating = mOwner.Settings.MaxAugment;

            // Call PropertyChanged
            foreach (string name in propNames)
            {
                OnPropertyChanged(name);
            }
        }

        protected virtual HashSet<string> AddAugment(Augment a, HashSet<string> propNames)
        {
            if (a.Kind == AugmentKind.Rating)
            {
                mBonusRating += a.Bonus;
                propNames.Add(nameof(BonusRating));
                propNames.Add(nameof(AugmentedRating));
                propNames.Add(nameof(DisplayValue));
            }
            if (a.Kind == AugmentKind.Max)
            {
                ExtraMax += (int)a.Bonus;
                propNames.Add(nameof(Max));
            }
            return propNames;
        }

        public virtual void OnAugmentRemoving(AugmentKind kind)
        {
            switch (kind)
            {
                case AugmentKind.None:
                    break;
                case AugmentKind.Rating:
                    RemovedNames.Add(nameof(BonusRating));
                    RemovedNames.Add(nameof(AugmentedRating));
                    RemovedNames.Add(nameof(DisplayValue));
                    break;
                case AugmentKind.Max:
                    RemovedNames.Add(nameof(Max));
                    break;
                case AugmentKind.DamageValue:
                    break;
                case AugmentKind.DamageType:
                    break;
                case AugmentKind.Accuracy:
                    break;
                case AugmentKind.Availability:
                    break;
                case AugmentKind.Restriction:
                    break;
                case AugmentKind.RC:
                    break;
                case AugmentKind.AP:
                    break;
                default:
                    break;
            }
        }

        #endregion IAugmentable Implemenation

        public int CompareTo(LeveledTrait other)
        {
            return (this.mName + mBaseRating).CompareTo(other.Name + mBaseRating);
        }

        public int CompareTo(int other)
        {
            return this.AugmentedRating.CompareTo(other);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        #region Operators

        public static implicit operator int(LeveledTrait lt)
        {
            return lt?.AugmentedRating ?? 0;
        }

        public static int operator -(LeveledTrait t)
        {
            return -t?.AugmentedRating ?? 0 ;
        }

        public static int operator +(LeveledTrait l, LeveledTrait r)
        {
            return l?.AugmentedRating ?? 0 + r?.AugmentedRating ?? 0;
        }
        public static int operator +(LeveledTrait l, int r)
        {
            return l?.AugmentedRating ?? 0  + r;
        }
        public static int operator +(int l, LeveledTrait r)
        {
            return l + r?.AugmentedRating ?? 0 ;
        }

        public static int operator -(LeveledTrait l, LeveledTrait r)
        {
            return l?.AugmentedRating ?? 0  - r?.AugmentedRating ?? 0 ;
        }
        public static int operator -(LeveledTrait l, int r)
        {
            return l?.AugmentedRating ?? 0  - r;
        }
        public static int operator -(int l, LeveledTrait r)
        {
            return l - r?.AugmentedRating ?? 0 ;
        }

        public static int operator *(LeveledTrait l, LeveledTrait r)
        {
            return l?.AugmentedRating ?? 0  * r?.AugmentedRating ?? 0 ;
        }
        public static int operator *(LeveledTrait l, int r)
        {
            return l?.AugmentedRating ?? 0  * r;
        }
        public static int operator *(int l, LeveledTrait r)
        {
            return l * r?.AugmentedRating ?? 0 ;
        }

        public static int operator /(LeveledTrait l, LeveledTrait r)
        {
            return l?.AugmentedRating ?? 0  / r?.AugmentedRating ?? 0 ;
        }
        public static int operator /(LeveledTrait l, int r)
        {
            return l?.AugmentedRating ?? 0  / r;
        }
        public static int operator /(int l, LeveledTrait r)
        {
            return l / r?.AugmentedRating ?? 0 ;
        }

        public static int operator %(LeveledTrait l, LeveledTrait r)
        {
            return l?.AugmentedRating ?? 0  + r?.AugmentedRating ?? 0 ;
        }
        public static int operator %(LeveledTrait l, int r)
        {
            return l?.AugmentedRating ?? 0  + r;
        }
        public static int operator %(int l, LeveledTrait r)
        {
            return l + r?.AugmentedRating ?? 0 ;
        }

        #region Comparison

        //public static bool operator ==(LeveledTrait l, LeveledTrait r)
        //{
        //    return l.AugmentedRating == r.AugmentedRating;
        //}
        //public static bool operator ==(LeveledTrait l, int r)
        //{
        //    return l.AugmentedRating == r;
        //}
        //public static bool operator ==(int l, LeveledTrait r)
        //{
        //    return l == r.AugmentedRating;
        //}

        //public static bool operator !=(LeveledTrait l, LeveledTrait r)
        //{
        //    return l.AugmentedRating != r.AugmentedRating;
        //}
        //public static bool operator !=(LeveledTrait l, int r)
        //{
        //    return l.AugmentedRating != r;
        //}
        //public static bool operator !=(int l, LeveledTrait r)
        //{
        //    return l != r.AugmentedRating;
        //}


        public static bool operator <(LeveledTrait l, LeveledTrait r)
        {
            return l.AugmentedRating < r.AugmentedRating;
        }
        public static bool operator <(LeveledTrait l, int r)
        {
            return l.AugmentedRating < r;
        }
        public static bool operator <(int l, LeveledTrait r)
        {
            return l < r.AugmentedRating;
        }
        
        public static bool operator >(LeveledTrait l, LeveledTrait r)
        {
            return l.AugmentedRating > r.AugmentedRating;
        }
        public static bool operator >(LeveledTrait l, int r)
        {
            return l.AugmentedRating > r;
        }
        public static bool operator >(int l, LeveledTrait r)
        {
            return l > r.AugmentedRating;
        }


        public static bool operator <=(LeveledTrait l, LeveledTrait r)
        {
            return l.AugmentedRating < r.AugmentedRating;
        }
        public static bool operator <=(LeveledTrait l, int r)
        {
            return l.AugmentedRating < r;
        }
        public static bool operator <=(int l, LeveledTrait r)
        {
            return l < r.AugmentedRating;
        }

        public static bool operator >=(LeveledTrait l, LeveledTrait r)
        {
            return l.AugmentedRating > r.AugmentedRating;
        }
        public static bool operator >=(LeveledTrait l, int r)
        {
            return l.AugmentedRating > r;
        }
        public static bool operator >=(int l, LeveledTrait r)
        {
            return l > r.AugmentedRating;
        }

        #endregion

        #endregion
    }
}
