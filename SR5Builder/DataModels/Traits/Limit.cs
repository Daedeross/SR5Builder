using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Attribute = SR5Builder.DataModels.Attribute;

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
                if (value != mPrimaryAttribute)
                {
                    if (mPrimaryAttribute != null)
                        mPrimaryAttribute.PropertyChanged -= this.OnAttributeChanged;
                    mPrimaryAttribute = value;

                    if (mPrimaryAttribute != null)
                        mPrimaryAttribute.PropertyChanged += this.OnAttributeChanged;

                    OnPropertyChanged("PrimaryAttribute");
                    OnPropertyChanged("BaseRating");
                    OnPropertyChanged("AugmentedRating");
                }
            }
        }

        protected Attribute mSecondaryAttribute;
        public Attribute SecondaryAttribute
        {
            get { return mSecondaryAttribute; }
            set
            {
                if (value != mSecondaryAttribute)
                {
                    if (mSecondaryAttribute != null)
                        mSecondaryAttribute.PropertyChanged -= this.OnAttributeChanged;
                    mSecondaryAttribute = value;

                    if (mSecondaryAttribute != null)
                        mSecondaryAttribute.PropertyChanged += this.OnAttributeChanged;

                    OnPropertyChanged("SecondaryAttribute");
                    OnPropertyChanged("BaseRating");
                    OnPropertyChanged("AugmentedRating");
                }
            }
        }

        protected Attribute mTertiaryAttribute;
        public Attribute TertiaryAttribute
        {
            get { return mTertiaryAttribute; }
            set
            {
                if (value != mTertiaryAttribute)
                {
                    if (mTertiaryAttribute != null)
                        mTertiaryAttribute.PropertyChanged -= this.OnAttributeChanged;
                    mTertiaryAttribute = value;

                    if (mTertiaryAttribute != null)
                        mTertiaryAttribute.PropertyChanged += this.OnAttributeChanged;

                    OnPropertyChanged("TertiaryAttribute");
                    OnPropertyChanged("BaseRating");
                    OnPropertyChanged("AugmentedRating");
                }
            }
        }

        public override int BaseRating
        {
            get
            {
                return (int)Math.Ceiling((double)(PrimaryAttribute.BaseRating * 2 + SecondaryAttribute.BaseRating + TertiaryAttribute.BaseRating) / 3);
            }
            set
            {
                //base.BaseRating = value;
            }
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

        public Limit()
        {

        }

        public Limit(Attribute primary, Attribute secondary, Attribute tertiary)
            :base()
        {
            PrimaryAttribute = primary;
            SecondaryAttribute = secondary;
            TertiaryAttribute = tertiary;
        }

        #endregion // Constructors

        #region Private Methods

        private void OnAttributeChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Contains("Base"))
                OnPropertyChanged("BaseRating");
            if (e.PropertyName.Contains("Augmented"))
                OnPropertyChanged("AugmentedRating");
        }

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}
