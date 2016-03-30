﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace SR5Builder.DataModels
{
    public class Attribute: LeveledTrait
    {       
        protected PropertyInfo mMin;
        public override int Min
        {
            get
            {
                if (mOwner.MetatypeStats != null)
                    return (int)mMin.GetValue(mOwner.MetatypeStats, null);
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
                    return (int)mMax.GetValue(mOwner.MetatypeStats, null);
                else
                    return 0;
            }
        }

        public override int BaseRating
        {
            get { return mBaseRating + Min; }
            set
            {
                if (value >= Min
                    && value <= Max)
                base.BaseRating = value - Min;
            }
        }

        public override int AugmentedRating
        {
            get
            {
                return BaseRating + BonusRating;
            }
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
                OnPropertyChanged("Min");
                OnPropertyChanged("Max");
                OnPropertyChanged("BaseRating");
                OnPropertyChanged("AugmentedRating");
            }
        }
    }
}
