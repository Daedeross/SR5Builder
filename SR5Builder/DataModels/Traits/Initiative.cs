using System;
using System.Collections.Generic;
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
            set
            {
                if (value != mAttributeOne)
                {
                    mAttributeOne = value;
                    OnPropertyChanged("AttributeOne");
                    OnPropertyChanged("BaseRating");
                    OnPropertyChanged("AugmentedRating");
                }
            }
        }

        protected Attribute mAttributeTwo;
        public Attribute AttributeTwo
        {
            get { return mAttributeTwo; }
            set
            {
                if (value != mAttributeTwo)
                {
                    mAttributeTwo = value;
                    OnPropertyChanged("AttributeTwo");
                    OnPropertyChanged("BaseRating");
                    OnPropertyChanged("AugmentedRating");
                }
            }
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
            get { return mAttributeOne.BaseRating + mAttributeTwo.BaseRating; }
            set { } // not settable;
        }

        public override int AugmentedRating
        {
            get { return mAttributeOne.AugmentedRating + mAttributeTwo.AugmentedRating + BonusRating; }
        }

            #endregion // Overrrides

        #endregion // Properties

        #region Constructors

        #endregion // Constructors

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}
