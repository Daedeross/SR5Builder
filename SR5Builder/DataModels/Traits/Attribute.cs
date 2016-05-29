﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;
using SR5Builder.Prototypes;

namespace SR5Builder.DataModels
{
    public class Attribute: LeveledTrait, IKarmaCost
    {       
        protected PropertyInfo mMin;
        public override int Min
        {
            get
            {
                if (mOwner?.MetatypeStats != null)
                    return (int)mMin.GetValue(mOwner.MetatypeStats, null) + ExtraMin;
                else
                    return 0;
            }
        }

        protected PropertyInfo mMax;
        public override int Max
        {
            get
            {
                if (mOwner.MetatypeStats != null)
                    return (int)mMax.GetValue(mOwner.MetatypeStats, null) + ExtraMax;
                else
                    return 0;
            }
        }

        public override int BaseRating
        {
            get { return mBaseRating + Min; }
            set
            {
                if (value != mBaseRating + Min)
                {
                    if (value < Min)
                    {
                        mBaseRating = 0;
                    }
                    else if (value > Max)
                    {
                        mBaseRating = Max - Min;
                    }
                    else
                    {
                        mBaseRating = value - Min;
                    }
                    OnPropertyChanged(nameof(BaseRating));
                    OnPropertyChanged(nameof(Points));
                    OnPropertyChanged(nameof(ImprovedRating));
                    OnPropertyChanged(nameof(AugmentedRating));
                }
            }
        }

        public virtual int Karma
        {
            get { return mOwner.Settings.AttributeKarma(ImprovedRating, BaseRating); }
        }

        public Attribute(SR5Character owner, string name)
            : base(owner)
        {
            Name = name;
            mMin = (typeof(MetatypeStats)).GetProperty(name + "Min");
            mMax = (typeof(MetatypeStats)).GetProperty(name + "Max");

            mBaseRating = 0;
            if (mOwner != null)
                mOwner.PropertyChanged += this.OnOwnerChanged;
        }

        public Attribute(SR5Character owner)
            :base(owner)
        {
            mBaseRating = 0;
            if (mOwner != null)
                mOwner.PropertyChanged += this.OnOwnerChanged;
        }

        private void OnOwnerChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "MetatypeStats")
            {
                OnPropertyChanged(nameof(Min));
                OnPropertyChanged(nameof(Max));
                OnPropertyChanged(nameof(BaseRating));
                OnPropertyChanged(nameof(AugmentedRating));
            }
        }
    }
}
