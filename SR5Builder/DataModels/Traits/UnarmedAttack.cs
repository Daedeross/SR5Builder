using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public class UnarmedAttack: MeleeWeapon
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        public override int Acc
        {
            get
            {
                return mOwner.PhysicalLimit.AugmentedRating;
            }
            set
            {
                //
            }
        }

        #endregion // Properties

        #region Constructors

        public UnarmedAttack(SR5Character owner)
            :base(owner)
        {
            Name = "Unarmed Attack";
        }

        #endregion // Constructors

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}
