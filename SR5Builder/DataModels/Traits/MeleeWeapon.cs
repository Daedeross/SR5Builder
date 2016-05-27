﻿using SR5Builder.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public class MeleeWeapon: Weapon
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        public override SR5Character Owner
        {
            get
            {
                return base.Owner;
            }
            set
            {
                if (mOwner != null)
                    mOwner.Strength.PropertyChanged -= this.OnStrengthChanged;

                base.Owner = value;

                if (mOwner != null)
                {
                    mOwner.Strength.PropertyChanged += this.OnStrengthChanged;
                }
            }
        }

        private bool mUseStrength;
        public bool UseStrength
        {
            get { return mUseStrength; }
            set
            {
                if (value != mUseStrength)
                {
                    mUseStrength = value;
                    RaisePropertyChanged(nameof(UseStrength));
                    RaisePropertyChanged(nameof(DV));
                }
            }
        }

        public override int DV
        {
            get
            {
                if (mUseStrength)
                    return mDV + mOwner.Strength.AugmentedRating;
                return mDV;
            }
        }

        protected int mBaseReach;
        public int BaseReach
        {
            get { return mBaseReach; }
            set
            {
                if (value != mBaseReach)
                {
                    mBaseReach = value;
                    RaisePropertyChanged(nameof(BaseReach));
                    RaisePropertyChanged(nameof(Reach));
                }
            }
        }

        public int Reach
        {
            get
            {
                if (mOwner.MetatypeStats != null)
                    return mOwner.MetatypeStats.Reach + mBaseReach;
                else return mBaseReach;
            }
        }

        #endregion // Properties

        #region Constructors

        public MeleeWeapon(SR5Character owner)
            :base(owner)
        {
            owner.Strength.PropertyChanged += OnStrengthChanged;
        }

        public MeleeWeapon(SR5Character owner, MeleeWeaponPrototype loader)
            :base(owner)
        {
            owner.Strength.PropertyChanged += OnStrengthChanged;
            CopyFromLoader(loader);
        }

        #endregion // Constructors

        #region Commands

        #endregion // Commands

        #region Private Methods

        private void OnStrengthChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(DV));
            RaisePropertyChanged(nameof(DisplayDamage));
        }

        #endregion // Private Methods

        #region Public Methods

        public virtual void CopyFromLoader(MeleeWeaponPrototype loader)
        {
            base.CopyFromLoader(loader);

            mUseStrength = loader.UseStrength;
            mBaseReach = loader.Reach;
        }

        #endregion // Public Methods
    }
}
