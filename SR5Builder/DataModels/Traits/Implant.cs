using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SR5Builder.Loaders;

namespace SR5Builder.DataModels
{
    public class Implant: Gear, IAugmentContainer
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        private decimal mEssenceCost;
        public decimal EssenceCost
        {
            get { return mEssenceCost; }
            set
            {
                if (mEssenceCost != value)
                {
                    mEssenceCost = value;
                    OnPropertyChanged("EssenceCost");
                }
            }
        }

        public ObservableCollection<Augment> GivenAugments { get; set; }

        #endregion // Properties

        #region Constructors

        public Implant(SR5Character owner, ImplantLoader loader)
            :base (owner, loader)
        {

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
