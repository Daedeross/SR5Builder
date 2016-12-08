using System;
using System.ComponentModel;

namespace SR5Builder.DataModels
{
    public class PowerPoints : Attribute, IKarmaCost
    {
        public override int Min
        {
            get
            {
                if (mOwner.SpecialChoice.FreePowerPoints)
                {
                    return mOwner.SpecialAttribute.AugmentedRating;
                }
                return mOwner.SpecialChoice.PowerPoints;
            }
            set { } // noop
        }

        public override int Max
        {
            get
            {
                if (mOwner.SpecialChoice.CanBuyPowerPoints)
                {
                    return mOwner.SpecialAttribute.AugmentedRating;
                }
                return mOwner.SpecialChoice.PowerPoints;
            }
            set { }
        }

        public override string DisplayValue
        {
            get
            {
                return AugmentedRating.ToString();
            }
        }

        public override int Karma
        {
            get
            {
                return Math.Min(0, (BaseRating - Min) * mOwner.Settings.PowerPointKarma);
            }
        }

        public PowerPoints(SR5Character owner, string name)
            :base (owner)
        {
            mName = name;
            mOwner.PropertyChanged += OnOwnerChanged;
            mOwner.SpecialAttribute.PropertyChanged += OnMagicChanged;
        }

        private void OnMagicChanged(object sender, PropertyChangedEventArgs e)
        {
            if (   mOwner.SpecialChoice.CanBuyPowerPoints
                && e.PropertyName == nameof(SpecialAttribute.AugmentedRating))
            {
                OnPropertyChanged(nameof(Max));
            }
        }

        private void OnOwnerChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Contains("Special") || e.PropertyName.Contains("Rating"))
            {
                if (Max < BaseRating)
                {
                    BaseRating = Max;
                }
                OnPropertyChanged(nameof(Max));
                OnPropertyChanged(nameof(ImprovedRating));
                OnPropertyChanged(nameof(BaseRating));
                OnPropertyChanged(nameof(AugmentedRating));
            }
        }
        
        #region Operators

        public static implicit operator int(PowerPoints lt)
        {
            return lt?.AugmentedRating ?? 0;
        }

        public static int operator -(PowerPoints t)
        {
            return -t?.AugmentedRating ?? 0;
        }

        public static int operator +(PowerPoints l, PowerPoints r)
        {
            return l?.AugmentedRating ?? 0 + r?.AugmentedRating ?? 0;
        }
        public static int operator +(PowerPoints l, int r)
        {
            return l?.AugmentedRating ?? 0 + r;
        }
        public static int operator +(int l, PowerPoints r)
        {
            return l + r?.AugmentedRating ?? 0;
        }

        public static int operator -(PowerPoints l, PowerPoints r)
        {
            return l?.AugmentedRating ?? 0 - r?.AugmentedRating ?? 0;
        }
        public static int operator -(PowerPoints l, int r)
        {
            return l?.AugmentedRating ?? 0 - r;
        }
        public static int operator -(int l, PowerPoints r)
        {
            return l - r?.AugmentedRating ?? 0;
        }

        public static int operator *(PowerPoints l, PowerPoints r)
        {
            return l?.AugmentedRating ?? 0 * r?.AugmentedRating ?? 0;
        }
        public static int operator *(PowerPoints l, int r)
        {
            return l?.AugmentedRating ?? 0 * r;
        }
        public static int operator *(int l, PowerPoints r)
        {
            return l * r?.AugmentedRating ?? 0;
        }

        public static int operator /(PowerPoints l, PowerPoints r)
        {
            return l?.AugmentedRating ?? 0 / r?.AugmentedRating ?? 0;
        }
        public static int operator /(PowerPoints l, int r)
        {
            return l?.AugmentedRating ?? 0 / r;
        }
        public static int operator /(int l, PowerPoints r)
        {
            return l / r?.AugmentedRating ?? 0;
        }

        public static int operator %(PowerPoints l, PowerPoints r)
        {
            return l?.AugmentedRating ?? 0 + r?.AugmentedRating ?? 0;
        }
        public static int operator %(PowerPoints l, int r)
        {
            return l?.AugmentedRating ?? 0 + r;
        }
        public static int operator %(int l, PowerPoints r)
        {
            return l + r?.AugmentedRating ?? 0;
        }

        #region Comparison

        public static bool operator <(PowerPoints l, PowerPoints r)
        {
            return l.AugmentedRating < r.AugmentedRating;
        }
        public static bool operator <(PowerPoints l, int r)
        {
            return l.AugmentedRating < r;
        }
        public static bool operator <(int l, PowerPoints r)
        {
            return l < r.AugmentedRating;
        }

        public static bool operator >(PowerPoints l, PowerPoints r)
        {
            return l.AugmentedRating > r.AugmentedRating;
        }
        public static bool operator >(PowerPoints l, int r)
        {
            return l.AugmentedRating > r;
        }
        public static bool operator >(int l, PowerPoints r)
        {
            return l > r.AugmentedRating;
        }


        public static bool operator <=(PowerPoints l, PowerPoints r)
        {
            return l.AugmentedRating < r.AugmentedRating;
        }
        public static bool operator <=(PowerPoints l, int r)
        {
            return l.AugmentedRating < r;
        }
        public static bool operator <=(int l, PowerPoints r)
        {
            return l < r.AugmentedRating;
        }

        public static bool operator >=(PowerPoints l, PowerPoints r)
        {
            return l.AugmentedRating > r.AugmentedRating;
        }
        public static bool operator >=(PowerPoints l, int r)
        {
            return l.AugmentedRating > r;
        }
        public static bool operator >=(int l, PowerPoints r)
        {
            return l > r.AugmentedRating;
        }

        #endregion

        #endregion

    }
}
