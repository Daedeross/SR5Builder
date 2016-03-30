using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public abstract class LeveledTrait: BaseTrait, IAugmentable
    {
        #region Properties

        public virtual int Min { get; set; }

        public virtual int Max { get; set; }

        protected int mBaseRating;
        public virtual int BaseRating
        {
            get { return mBaseRating; }
            set
            {
                if (mBaseRating != value)
                {
                    mBaseRating = value;
                    OnPropertyChanged("BaseRating");
                    OnPropertyChanged("AugmentdRating");
                    OnPropertyChanged("Points");
                }
            }
        }

        protected float mBonusRating;
        public virtual int BonusRating
        {
            get
            {
                if(mBonusRating > SR5Character.MaxAugment)
                    mBonusRating = SR5Character.MaxAugment;
                if (mBaseRating < 0)
                    mBonusRating = 0;
                return (int)mBonusRating;
            }
            set
            {
                if ((int)mBonusRating != value)
                {
                    mBonusRating = value;
                    OnPropertyChanged("BonusRating");
                    OnPropertyChanged("AugmentedRating");
                }
            }
        }

        public override int Points
        {
            get
            {
                return mBaseRating;
            }
            set
            {
                base.Points = value;
            }
        }

        public virtual int AugmentedRating
        {
            get { return BaseRating + BonusRating; }
        }

        #endregion Properties

        #region Constructors

        public LeveledTrait()
            :base()
        {
            Initialize();
        }

        public LeveledTrait(SR5Character owner)
            :base(owner)
        {
            Initialize();
        }

        private void Initialize()
        {
            Augments = new ObservableCollection<Augment>();
            Augments.CollectionChanged += this.OnAugmentCollectionChanged;
        }

        #endregion // Contsructors

        public override string ToString()
        {
            if (mBonusRating != 0)
                return BaseRating + " (" + AugmentedRating + ")";
            else
                return BaseRating.ToString();
        }

        #region IAugmentable Implemenation

        //protected ObservableCollection<Augment> mAugments;
        public ObservableCollection<Augment> Augments { get; set; }

        public void OnAugmentChanged(object sender, PropertyChangedEventArgs e)
        {
            RecalcBonus();
        }

        public void OnAugmentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //HashSet<string> propNames = new HashSet<string>();
            if (e.OldItems != null)
            {
                foreach (Augment a in e.OldItems)
                {
                    a.PropertyChanged -= this.OnAugmentChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach (Augment a in e.NewItems)
                {
                    a.PropertyChanged += this.OnAugmentChanged;
                    //propNames = AddAugment(a, propNames);
                }
            }

            //foreach (string name in propNames)
            //{
            //    OnPropertyChanged(name);
            //}

            RecalcBonus();
        }

        protected virtual void RecalcBonus(HashSet<string> propNames = null)
        {
            //create HashSet if needed
            if (propNames == null)
                propNames = new HashSet<string>();

            //Clear Bonus
            //float bonus = 0;
            mBonusRating = 0;

            foreach (Augment a in Augments)
                propNames = AddAugment(a, propNames);

            if (mBonusRating > SR5Character.MaxAugment)
                mBonusRating = SR5Character.MaxAugment;

            // Call PropertyChanged
            foreach (string name in propNames)
            {
                OnPropertyChanged(name);
            }
        }

        protected virtual HashSet<string> AddAugment(Augment a, HashSet<string> propNames)
        {
            if (a.Kind == AugmentKind.Rating)
            {
                mBonusRating += a.Bonus;
                propNames.Add("BonusRating");
                propNames.Add("AugmentedRating");
            }

            return propNames;
        }

        #endregion IAugmentable Implemenation

    }
}
