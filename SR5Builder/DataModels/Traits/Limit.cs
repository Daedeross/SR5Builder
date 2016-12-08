﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Attribute = SR5Builder.DataModels.Attribute;

namespace SR5Builder.DataModels
{
    public class Limit: LeveledTrait
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        protected Attribute mPrimaryAttribute;
        public Attribute PrimaryAttribute
        {
            get { return mPrimaryAttribute; }
            set
            {
                if (!ReferenceEquals(value, mPrimaryAttribute))
                {
                    if (mPrimaryAttribute != null)
                        mPrimaryAttribute.PropertyChanged -= this.OnAttributeChanged;
                    mPrimaryAttribute = value;

                    if (mPrimaryAttribute != null)
                        mPrimaryAttribute.PropertyChanged += this.OnAttributeChanged;

                    OnPropertyChanged(nameof(PrimaryAttribute));
                    OnPropertyChanged(nameof(BaseRating));
                    OnPropertyChanged(nameof(AugmentedRating));
                }
            }
        }

        protected Attribute mSecondaryAttribute;
        public Attribute SecondaryAttribute
        {
            get { return mSecondaryAttribute; }
            set
            {
                if (!ReferenceEquals(value, mSecondaryAttribute))
                {
                    if (mSecondaryAttribute != null)
                        mSecondaryAttribute.PropertyChanged -= this.OnAttributeChanged;
                    mSecondaryAttribute = value;

                    if (mSecondaryAttribute != null)
                        mSecondaryAttribute.PropertyChanged += this.OnAttributeChanged;

                    OnPropertyChanged(nameof(SecondaryAttribute));
                    OnPropertyChanged(nameof(BaseRating));
                    OnPropertyChanged(nameof(AugmentedRating));
                }
            }
        }

        protected Attribute mTertiaryAttribute;
        public Attribute TertiaryAttribute
        {
            get { return mTertiaryAttribute; }
            set
            {
                if (!ReferenceEquals(value, mTertiaryAttribute))
                {
                    if (mTertiaryAttribute != null)
                        mTertiaryAttribute.PropertyChanged -= this.OnAttributeChanged;
                    mTertiaryAttribute = value;

                    if (mTertiaryAttribute != null)
                        mTertiaryAttribute.PropertyChanged += this.OnAttributeChanged;

                    OnPropertyChanged(nameof(TertiaryAttribute));
                    OnPropertyChanged(nameof(BaseRating));
                    OnPropertyChanged(nameof(AugmentedRating));
                }
            }
        }

        public override int BaseRating
        {
            get
            {
                return (int)Math.Ceiling((double)(PrimaryAttribute.BaseRating * 2 + SecondaryAttribute.BaseRating + TertiaryAttribute.BaseRating) / 3);
            }
            set { }
        }

        public override int ImprovedRating
        {
            get
            {
                return (int)Math.Ceiling((double)(PrimaryAttribute.ImprovedRating * 2 + SecondaryAttribute.ImprovedRating + TertiaryAttribute.ImprovedRating) / 3);
            }

            set { }
        }

        public override int AugmentedRating
        {
            get
            {
                return BonusRating + (int)Math.Ceiling((double)(PrimaryAttribute.AugmentedRating * 2 + SecondaryAttribute.AugmentedRating + TertiaryAttribute.AugmentedRating) / 3);
            }
        }

        #endregion // Properties

        #region Constructors

        public Limit(SR5Character owner, Attribute primary, Attribute secondary, Attribute tertiary)
            :base(owner)
        {
            PrimaryAttribute = primary;
            SecondaryAttribute = secondary;
            TertiaryAttribute = tertiary;
        }

        #endregion // Constructors

        #region Private Methods

        private void OnAttributeChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            bool changed = false;
            if (e.PropertyName.Contains("Base"))
            {
                OnPropertyChanged(nameof(BaseRating));
                changed = true;
            }
            else if (e.PropertyName.Contains("Improved"))
            {
                OnPropertyChanged(nameof(ImprovedRating));
                changed = true;
            }
            else if (e.PropertyName.Contains("Augmented"))
            { 
                OnPropertyChanged(nameof(AugmentedRating));
                changed = true;
            }
            if (changed)
            {
                OnPropertyChanged(nameof(DisplayValue));
            }
        }

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
        
        #region Operators

        public static implicit operator int(Limit lt)
        {
            return lt?.AugmentedRating ?? 0;
        }

        public static int operator -(Limit t)
        {
            return -t?.AugmentedRating ?? 0;
        }

        public static int operator +(Limit l, Limit r)
        {
            return l?.AugmentedRating ?? 0 + r?.AugmentedRating ?? 0;
        }
        public static int operator +(Limit l, int r)
        {
            return l?.AugmentedRating ?? 0 + r;
        }
        public static int operator +(int l, Limit r)
        {
            return l + r?.AugmentedRating ?? 0;
        }

        public static int operator -(Limit l, Limit r)
        {
            return l?.AugmentedRating ?? 0 - r?.AugmentedRating ?? 0;
        }
        public static int operator -(Limit l, int r)
        {
            return l?.AugmentedRating ?? 0 - r;
        }
        public static int operator -(int l, Limit r)
        {
            return l - r?.AugmentedRating ?? 0;
        }

        public static int operator *(Limit l, Limit r)
        {
            return l?.AugmentedRating ?? 0 * r?.AugmentedRating ?? 0;
        }
        public static int operator *(Limit l, int r)
        {
            return l?.AugmentedRating ?? 0 * r;
        }
        public static int operator *(int l, Limit r)
        {
            return l * r?.AugmentedRating ?? 0;
        }

        public static int operator /(Limit l, Limit r)
        {
            return l?.AugmentedRating ?? 0 / r?.AugmentedRating ?? 0;
        }
        public static int operator /(Limit l, int r)
        {
            return l?.AugmentedRating ?? 0 / r;
        }
        public static int operator /(int l, Limit r)
        {
            return l / r?.AugmentedRating ?? 0;
        }

        public static int operator %(Limit l, Limit r)
        {
            return l?.AugmentedRating ?? 0 + r?.AugmentedRating ?? 0;
        }
        public static int operator %(Limit l, int r)
        {
            return l?.AugmentedRating ?? 0 + r;
        }
        public static int operator %(int l, Limit r)
        {
            return l + r?.AugmentedRating ?? 0;
        }

        #region Comparison

        public static bool operator <(Limit l, Limit r)
        {
            return l.AugmentedRating < r.AugmentedRating;
        }
        public static bool operator <(Limit l, int r)
        {
            return l.AugmentedRating < r;
        }
        public static bool operator <(int l, Limit r)
        {
            return l < r.AugmentedRating;
        }

        public static bool operator >(Limit l, Limit r)
        {
            return l.AugmentedRating > r.AugmentedRating;
        }
        public static bool operator >(Limit l, int r)
        {
            return l.AugmentedRating > r;
        }
        public static bool operator >(int l, Limit r)
        {
            return l > r.AugmentedRating;
        }

        public static bool operator <=(Limit l, Limit r)
        {
            return l.AugmentedRating < r.AugmentedRating;
        }
        public static bool operator <=(Limit l, int r)
        {
            return l.AugmentedRating < r;
        }
        public static bool operator <=(int l, Limit r)
        {
            return l < r.AugmentedRating;
        }

        public static bool operator >=(Limit l, Limit r)
        {
            return l.AugmentedRating > r.AugmentedRating;
        }
        public static bool operator >=(Limit l, int r)
        {
            return l.AugmentedRating > r;
        }
        public static bool operator >=(int l, Limit r)
        {
            return l > r.AugmentedRating;
        }

        #endregion

        #endregion

    }
}
