using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SR5Builder.Prototypes;

namespace SR5Builder.DataModels
{
    public class Implant: Gear, IAugmentContainer
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        public ObservableCollection<Augment> GivenAugments { get; set; }

        #endregion // Properties

        #region Constructors

        public Implant(SR5Character owner, ImplantPrototype loader)
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
