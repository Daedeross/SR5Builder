using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SR5Builder.Prototypes;

namespace SR5Builder.DataModels
{
    public class Implant: Gear, IAugmentContainer, IArmor
    {
        #region Private Fields
        
        #endregion // Private fields

        #region Properties

        public ObservableCollection<Augment> GivenAugments { get; set; }

        private ImplantGrade mGrade;
        public ImplantGrade Grade
        {
            get { return mGrade; }
            set
            {
                if (value != mGrade)
                {
                    mGrade = value;
                    OnPropertyChanged(nameof(Cost));
                    OnPropertyChanged(nameof(TotalEssence));
                    OnPropertyChanged(nameof(Availability));
                }
            }
        }

        public override decimal Cost
        {
            get
            {
                switch (Grade)
                {
                    case ImplantGrade.Used:
                        return base.Cost * 0.75m;
                    case ImplantGrade.Standard:
                        return base.Cost;
                    case ImplantGrade.Alpha:
                        return base.Cost * 1.2m;
                    case ImplantGrade.Beta:
                        return base.Cost * 1.5m;
                    case ImplantGrade.Delta:
                        return base.Cost * 2.5m;
                    default:
                        return base.Cost;
                }
            }
        }

        public override decimal TotalEssence
        {
            get
            {
                switch (mGrade)
                {
                    case ImplantGrade.Used:
                        return base.TotalEssence * 1.25m;
                    case ImplantGrade.Standard:
                        return base.TotalEssence;
                    case ImplantGrade.Alpha:
                        return base.TotalEssence * 0.8m;
                    case ImplantGrade.Beta:
                        return base.TotalEssence * 0.7m;
                    case ImplantGrade.Delta:
                        return base.TotalEssence * 0.5m;
                    default:
                        return base.TotalEssence;
                }
            }
        }

        public override Availability Availability
        {
            get
            {
                
                switch (mGrade)
                {
                    case ImplantGrade.Used:
                        return base.Availability +(-4);
                    case ImplantGrade.Standard:
                        return base.Availability;
                    case ImplantGrade.Alpha:
                        return base.Availability + 2;
                    case ImplantGrade.Beta:
                        return base.Availability + 4;
                    case ImplantGrade.Delta:
                        return base.Availability + 8;
                    default:
                        return base.Availability;
                }
            }
        }

        public override bool IsClothing
        {
            get { return false; }
        }

        #endregion // Properties

        #region Constructors

        public Implant(SR5Character owner, ImplantPrototype loader)
            :base (owner, loader)
        {
            GivenAugments = new ObservableCollection<Augment>();
            foreach (AugmentPrototype a in loader.Augments)
            {
                GivenAugments.Add(a.ToAugment(this));
            }
        }

        #endregion // Constructors

        #region Commands

        #endregion // Commands

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods

        #region IAugmentContainer Methods

        public void ClearAugments()
        {
            if (GivenAugments != null)
            {
                foreach (Augment a in GivenAugments)
                {
                    a.Target = null;
                } 
            }
        }

        #endregion // IAugmentContainer Methods
    }
}
