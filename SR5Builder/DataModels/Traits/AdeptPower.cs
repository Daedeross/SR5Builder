using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SR5Builder.DataModels
{
    public class AdeptPower : LeveledTrait, IAugmentContainer
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        public ObservableCollection<Augment> GivenAugments { get; set; }

        public override int BaseRating
        {
            get
            {
                return base.BaseRating;
            }
            set
            {
                base.BaseRating = value;
                OnPropertyChanged(nameof(PowerPoints));
            }
        }

        private decimal mFlatPoints;
        public decimal FlatPoints
        {
            get { return mFlatPoints; }
            set
            {
                if (value != mFlatPoints)
                {
                    mFlatPoints = value;
                    OnPropertyChanged(nameof(FlatPoints));
                    OnPropertyChanged(nameof(PowerPoints));
                }
            }
        }

        private decimal mPointsPerLevel;
        public  decimal PointPerLevel
        {
            get { return mPointsPerLevel; }
            set
            {
                if (value != mPointsPerLevel)
                {
                    mPointsPerLevel = value;
                    OnPropertyChanged(nameof(PointPerLevel));
                    OnPropertyChanged(nameof(PowerPoints));
                }
            }
        }

        public decimal PowerPoints
        {
            get { return mFlatPoints + mBaseRating * mPointsPerLevel; }
        }

        #endregion // Properties

        #region Constructors

        public AdeptPower(SR5Character c)
            : base(c)
        {
            GivenAugments = new ObservableCollection<Augment>();
        }

        #endregion // Constructors

        #region Commands

        #endregion // Commands

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods

        public void ClearAugments()
        {
            foreach (Augment a in GivenAugments)
            {
                a.Target = null;
            }
        }


        #region Operators

        public static implicit operator int(AdeptPower lt)
        {
            return lt?.AugmentedRating ?? 0;
        }

        public static int operator -(AdeptPower t)
        {
            return -t?.AugmentedRating ?? 0;
        }

        public static int operator +(AdeptPower l, AdeptPower r)
        {
            return l?.AugmentedRating ?? 0 + r?.AugmentedRating ?? 0;
        }
        public static int operator +(AdeptPower l, int r)
        {
            return l?.AugmentedRating ?? 0 + r;
        }
        public static int operator +(int l, AdeptPower r)
        {
            return l + r?.AugmentedRating ?? 0;
        }

        public static int operator -(AdeptPower l, AdeptPower r)
        {
            return l?.AugmentedRating ?? 0 - r?.AugmentedRating ?? 0;
        }
        public static int operator -(AdeptPower l, int r)
        {
            return l?.AugmentedRating ?? 0 - r;
        }
        public static int operator -(int l, AdeptPower r)
        {
            return l - r?.AugmentedRating ?? 0;
        }

        public static int operator *(AdeptPower l, AdeptPower r)
        {
            return l?.AugmentedRating ?? 0 * r?.AugmentedRating ?? 0;
        }
        public static int operator *(AdeptPower l, int r)
        {
            return l?.AugmentedRating ?? 0 * r;
        }
        public static int operator *(int l, AdeptPower r)
        {
            return l * r?.AugmentedRating ?? 0;
        }

        public static int operator /(AdeptPower l, AdeptPower r)
        {
            return l?.AugmentedRating ?? 0 / r?.AugmentedRating ?? 0;
        }
        public static int operator /(AdeptPower l, int r)
        {
            return l?.AugmentedRating ?? 0 / r;
        }
        public static int operator /(int l, AdeptPower r)
        {
            return l / r?.AugmentedRating ?? 0;
        }

        public static int operator %(AdeptPower l, AdeptPower r)
        {
            return l?.AugmentedRating ?? 0 + r?.AugmentedRating ?? 0;
        }
        public static int operator %(AdeptPower l, int r)
        {
            return l?.AugmentedRating ?? 0 + r;
        }
        public static int operator %(int l, AdeptPower r)
        {
            return l + r?.AugmentedRating ?? 0;
        }

        #region Comparison

        public static bool operator <(AdeptPower l, AdeptPower r)
        {
            return l.AugmentedRating < r.AugmentedRating;
        }
        public static bool operator <(AdeptPower l, int r)
        {
            return l.AugmentedRating < r;
        }
        public static bool operator <(int l, AdeptPower r)
        {
            return l < r.AugmentedRating;
        }

        public static bool operator >(AdeptPower l, AdeptPower r)
        {
            return l.AugmentedRating > r.AugmentedRating;
        }
        public static bool operator >(AdeptPower l, int r)
        {
            return l.AugmentedRating > r;
        }
        public static bool operator >(int l, AdeptPower r)
        {
            return l > r.AugmentedRating;
        }


        public static bool operator <=(AdeptPower l, AdeptPower r)
        {
            return l.AugmentedRating < r.AugmentedRating;
        }
        public static bool operator <=(AdeptPower l, int r)
        {
            return l.AugmentedRating < r;
        }
        public static bool operator <=(int l, AdeptPower r)
        {
            return l < r.AugmentedRating;
        }

        public static bool operator >=(AdeptPower l, AdeptPower r)
        {
            return l.AugmentedRating > r.AugmentedRating;
        }
        public static bool operator >=(AdeptPower l, int r)
        {
            return l.AugmentedRating > r;
        }
        public static bool operator >=(int l, AdeptPower r)
        {
            return l > r.AugmentedRating;
        }

        #endregion

        #endregion

    }
}
