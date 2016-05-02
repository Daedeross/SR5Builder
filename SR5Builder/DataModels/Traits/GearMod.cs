using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.Loaders;
using System.Collections.ObjectModel;

namespace SR5Builder.DataModels
{
    public class GearMod: LeveledTrait, IAugmentContainer
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
                    OnPropertyChanged("GearPiece");
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
                    OnPropertyChanged("FlatCost");
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

        public int Capacity { get; set; }

        public ObservableCollection<Augment> GivenAugments { get; set; }

        public string Notes { get; set; }

        public override int Karma
        {
            get { return 0; }
            set { }
        }

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

        public GearMod(Gear gearPiece, GearModLoader loader)
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

        private void CreateAugments(GearModLoader loader)
        {
            GivenAugments = new ObservableCollection<Augment>();
            foreach (AugmentLoader a in loader.Augments)
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
