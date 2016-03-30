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

        private float mFlatPoints;
        public float FlatPoints
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

        private float mPointsPerLevel;
        public float PointPerLevel
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

        public float PowerPoints
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
    }
}
