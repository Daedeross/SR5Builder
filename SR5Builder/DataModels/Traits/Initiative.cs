﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public class Initiative: LeveledTrait
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        protected Attribute mAttributeOne;
        public Attribute AttributeOne
        {
            get { return mAttributeOne; }
            //set
            //{
            //    if (value != mAttributeOne)
            //    {
            //        mAttributeOne = value;
            //        OnPropertyChanged(nameof(AttributeOne));
            //        OnPropertyChanged(nameof(BaseRating));
            //        OnPropertyChanged(nameof(AugmentedRating));
            //    }
            //}
        }

        protected Attribute mAttributeTwo;
        public Attribute AttributeTwo
        {
            get { return mAttributeTwo; }
            //set
            //{
            //    if (value != mAttributeTwo)
            //    {
            //        mAttributeTwo = value;
            //        OnPropertyChanged(nameof(AttributeTwo));
            //        OnPropertyChanged(nameof(BaseRating));
            //        OnPropertyChanged(nameof(AugmentedRating));
            //    }
            //}
        }

        #region Overrides

        public override int Min
        {
            get { return 0; }
            set { }
        }

        public override int Max
        {
            get { return System.Int32.MaxValue; }
            set { }
        }

        public override int BaseRating
        {
            get { return mAttributeOne.ImprovedRating + mAttributeTwo.ImprovedRating; }
            set { } // not settable;
        }

        public override int AugmentedRating
        {
            get { return mAttributeOne.AugmentedRating + mAttributeTwo.AugmentedRating + BonusRating; }
        }

        #endregion // Overrrides

        #endregion // Properties

        #region Constructors

        public Initiative(SR5Character owner, Attribute attribute1, Attribute attribute2, string name)
            :base(owner)
        {
            mName = name;
            mAttributeOne = attribute1;
            mAttributeOne.PropertyChanged += this.OnAttributeChanged;
            mAttributeTwo = attribute2;
            mAttributeTwo.PropertyChanged += this.OnAttributeChanged;
        }

        #endregion // Constructors

        #region Public Methods

        public override string ToString()
        {
            if (AugmentedRating != ImprovedRating)
            {
                return String.Format("{0}({1})", ImprovedRating, AugmentedRating);
            }
            else
            {
                return String.Format("{0}", ImprovedRating);
            }
        }

        #endregion // Public Methods

        private void OnAttributeChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Attribute.AugmentedRating):
                    OnPropertyChanged(e.PropertyName);
                    OnPropertyChanged(nameof(DisplayValue));
                    break;
                case nameof(Attribute.ImprovedRating):
                    OnPropertyChanged(e.PropertyName);
                    OnPropertyChanged(nameof(DisplayValue));
                    break;
                case nameof(Attribute.BaseRating):
                    OnPropertyChanged(e.PropertyName);
                    OnPropertyChanged(nameof(DisplayValue));
                    break;
                default:
                    break;
            }
        }

        protected override HashSet<string> AddAugment(Augment a, HashSet<string> propNames)
        {
            return base.AddAugment(a, propNames);
        }
    }
}
