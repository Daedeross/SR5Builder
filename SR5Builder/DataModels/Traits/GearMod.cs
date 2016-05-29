using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.Prototypes;
using System.Collections.ObjectModel;

namespace SR5Builder.DataModels
{
    public class GearMod: LeveledTrait, IAugmentContainer, IEssenceCost
    {
        #region Properties

        public string SubCategory { get; set; }

        protected Gear mGearPiece;
        public Gear GearPiece
        {
            get { return mGearPiece; }
            set
            {
                if (value != mGearPiece)
                {
                    mGearPiece = value;
                    OnPropertyChanged(nameof(GearPiece));
                }
            }
        }

        protected int mFlatCost;
        public int FlatCost
        {
            get { return mFlatCost; }
            set
            {
                if (value != mFlatCost)
                {
                    mFlatCost = value;
                    OnPropertyChanged(nameof(FlatCost));
                }
            }
        }

        public decimal CostMult { get; set; }

        public string DisplayCost
        {
            get
            {
                if (CostMult != 1)
                {
                    if (FlatCost != 0)
                        return "×" + CostMult + "+" + FlatCost.ToString("C", GlobalData.CostFormat);
                    else
                        return "×" + CostMult;
                }
                else return "+" + FlatCost.ToString("C", GlobalData.CostFormat);
            }
        }

        private decimal mFlatEssence;
        public decimal FlatEssence
        {
            get { return mFlatEssence; }
            set
            {
                if (value != mFlatEssence)
                {
                    mFlatEssence = value;
                    OnPropertyChanged(nameof(FlatEssence));
                    OnPropertyChanged(nameof(TotalEssence));
                }
            }
        }

        private decimal mRatingEssence;
        public decimal RatingEssence
        {
            get { return mRatingEssence; }
            set { mRatingEssence = value; }
        }

        public decimal TotalEssence
        {
            get { return mFlatEssence + mRatingEssence * mBaseRating; }
        }

        public int Capacity { get; set; }

        public ObservableCollection<Augment> GivenAugments { get; set; }

        public string Notes { get; set; }

        #endregion // Properties

        #region Constructors

        public GearMod(Gear gearPiece)
            :base(gearPiece.Owner)
        {
            mBaseRating = 0;
            FlatCost = 0;
            CostMult = 1;
            GivenAugments = new ObservableCollection<Augment>();
        }

        public GearMod(Gear gearPiece, GearModPrototype loader)
            : base(gearPiece.Owner)
        {
            Name = loader.Name;
            Book = loader.Book;
            Page = loader.Page;
            Category = loader.Category;
            SubCategory = loader.SubCategory ?? "None";
            Capacity = loader.Capacity;
            mBaseRating = 1;
            GearPiece = gearPiece;
            FlatCost = loader.FlatCost;
            CostMult = loader.CostMult;
            Notes = string.Copy(Notes ?? "");
            CreateAugments(loader);
        }

        private void CreateAugments(GearModPrototype loader)
        {
            GivenAugments = new ObservableCollection<Augment>();
            foreach (AugmentPrototype a in loader.Augments)
            {
                GivenAugments.Add(a.ToAugment(this));
            }
        }

        #endregion // Constructors

        #region Public Methods

        #endregion // Public Methods

        #region IAugmentContainer Interface

        public void ClearAugments()
        {
            throw new NotImplementedException();
        }

        #endregion IAugmentContainer Interface
    }
}
