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
                OnPropertyChanged("PowerPoints");
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
                    OnPropertyChanged("FlatPoints");
                    OnPropertyChanged("PowerPoints");
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
                    OnPropertyChanged("PointPerLevel");
                    OnPropertyChanged("PowerPoints");
                }
            }
        }

        public decimal PowerPoints
        {
            get { return mFlatPoints + mBaseRating * mPointsPerLevel; }
        }

        public override int Karma
        {
            get { return 0; }
            set { }
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
    }
}
