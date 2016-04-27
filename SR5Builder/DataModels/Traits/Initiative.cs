using System;
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
            //        OnPropertyChanged("AttributeOne");
            //        OnPropertyChanged("BaseRating");
            //        OnPropertyChanged("AugmentedRating");
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
            //        OnPropertyChanged("AttributeTwo");
            //        OnPropertyChanged("BaseRating");
            //        OnPropertyChanged("AugmentedRating");
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

        public override int Karma
        {
            get
            {
                return 0;
            }
            set { }
        }

        #endregion // Overrrides

        #endregion // Properties

        #region Constructors

        public Initiative(Attribute attribute1, Attribute attribute2)
        {
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
                case "AugmentedRating":
                    OnPropertyChanged(e.PropertyName);
                    OnPropertyChanged("DisplayValue");
                    break;
                case "ImprovedRating":
                    OnPropertyChanged(e.PropertyName);
                    OnPropertyChanged("DisplayValue");
                    break;
                case "BaseRating":
                    OnPropertyChanged(e.PropertyName);
                    OnPropertyChanged("DisplayValue");
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
