using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            set { }
        }

        public override decimal Cost
        {
            get { return 0; }
        }

        #endregion // Properties

        #region Constructors

        public UnarmedAttack(SR5Character owner)
            :base(owner)
        {
            Name = "Unarmed Attack";
            UseStrength = true;
            mDamageType = DamageType.S;

            owner.PhysicalLimit.PropertyChanged += this.OnLimitChanged;
        }

        #endregion // Constructors

        #region Private Methods

        private void OnLimitChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Limit.AugmentedRating))
            {
                OnPropertyChanged(nameof(Acc));
                OnPropertyChanged(nameof(DisplayAcc));
            }
        }

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}
