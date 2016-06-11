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

        protected decimal[] mFlatCostVector;
        public decimal FlatCost
        {
            get
            {
                int index = Math.Max(BaseRating, 0);
                if (index >= mFlatCostVector.Length)
                {
                    index = mFlatCostVector.Length - 1;
                }
                return mFlatCostVector[index];
            }
        }

        protected decimal[] mCostMultVector;
        public decimal CostMult
        {
            get
            {
                int index = BaseRating;
                if (index >= mCostMultVector.Length)
                {
                    index = mCostMultVector.Length - 1;
                }
                return mCostMultVector[index];
            }
        }


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

        protected int[] mCapacityVector;
        public int[] CapacityVector { get { return mCapacityVector; } }

        public int Capacity
        {
            get
            {
                int index = BaseRating;
                if (index >= mCapacityVector.Length)
                {
                    index = mCapacityVector.Length - 1;
                }
                return mCapacityVector[index];
            }
        }

        public ObservableCollection<Augment> GivenAugments { get; set; }

        public string Notes { get; set; }

        public override string ToString()
        {
            return BaseRating != 0 ? base.ToString() : "";
        }

        #endregion // Properties

        #region Constructors

        public GearMod(Gear gearPiece)
            :base(gearPiece.Owner)
        {
            mBaseRating = 0;
            mFlatCostVector = new decimal[1] { 0 };
            mCostMultVector = new decimal[1] { 1 };
            GivenAugments = new ObservableCollection<Augment>();
        }

        public GearMod(Gear gearPiece, GearModPrototype proto)
            : base(gearPiece.Owner)
        {
            Name = proto.Name;
            Book = proto.Book;
            Page = proto.Page;
            Category = proto.Category;
            SubCategory = proto.SubCategory ?? "None";
            Min = proto.Min;
            Max = proto.Max;
            mBaseRating = proto.Rating > 0 ? Min : proto.Rating;
            GearPiece = gearPiece;
            mFlatCostVector = new decimal[proto.FlatCostVector.Length];
            mCostMultVector = new decimal[proto.CostMultVector.Length];
            mCapacityVector = new int[proto.CapacityVector.Length];
            proto.FlatCostVector.CopyTo(mFlatCostVector, 0);
            proto.CostMultVector.CopyTo(mCostMultVector, 0);
            proto.CapacityVector.CopyTo(mCapacityVector, 0);
            
            Notes = string.Copy(Notes ?? "");
            CreateAugments(proto);
        }

        private void CreateAugments(GearModPrototype proto)
        {
            GivenAugments = new ObservableCollection<Augment>();
            foreach (AugmentPrototype a in proto.Augments)
            {
                if (a.Target == "%owner%")
                {
                    GivenAugments.Add(a.ToAugment(this, GearPiece.Name));
                }
                else
                {
                    GivenAugments.Add(a.ToAugment(this));
                }
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
