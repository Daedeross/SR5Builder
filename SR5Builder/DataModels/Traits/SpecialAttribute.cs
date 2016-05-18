using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public class SpecialAttribute: Attribute
    {
        public override int Min
        {
            get
            {
                if (mOwner.SpecialChoice == null)
                {
                    return 0;
                }
                return (int)mMin.GetValue(mOwner.SpecialChoice, null);
            }
        }

        public override int Max
        {
            get
            {
                if (mOwner.SpecialChoice == null || mOwner.SpecialChoice.Attribute == 0)
                {
                    return 0;
                }
                return 6 + mOwner.AdvancedGrade;
            }
        }

        public override int BonusRating
        {
            get
            {
                return mOwner.Essence.AugmentedRating - 6;
            }
            set
            {
                //base.BonusRating = value;
            }
        }

        public SpecialAttribute(SR5Character owner)
            :base (owner)
        {
            mOwner = owner;
            mName = mOwner.SpecialKind.ToString();

            mMin = (typeof(SpecialChoice)).GetProperty("Attribute");

            mOwner.Essence.PropertyChanged += this.OnEssenceChanged;
        }

        private void OnEssenceChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(BonusRating));
            OnPropertyChanged(nameof(AugmentedRating));
        }
    }
}