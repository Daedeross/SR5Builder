using System;
using System.ComponentModel;

namespace SR5Builder.DataModels
{
    public class PowerPoints : Attribute
    {
        public override int Min
        {
            get
            {
                return mOwner.SpecialChoice.PowerPoints;
            }
            set
            {
                base.Min = value;
            }
        }

        public override int Max
        {
            get
            {
                if (mOwner.SpecialChoice.CanBuyPowerPoints)
                {
                    return mOwner.SpecialAttribute.AugmentedRating;
                }
                return mOwner.SpecialChoice.PowerPoints;
            }
            set { }
        }

        public override string DisplayValue
        {
            get
            {
                return AugmentedRating.ToString();
            }
        }

        public override int Karma
        {
            get
            {
                return Math.Min(0, (BaseRating - Min) * mOwner.Settings.PowerPointKarma);
            }
            set { }
        }

        public PowerPoints(SR5Character owner, string name)
            :base (owner)
        {
            mName = name;
            mOwner.PropertyChanged += OnOwnerChanged;
        }

        public void OnOwnerChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Contains("Special"))
            {
                OnPropertyChanged(nameof(Max));
                OnPropertyChanged(nameof(ImprovedRating));
                OnPropertyChanged(nameof(BaseRating));
                OnPropertyChanged(nameof(AugmentedRating));
            }
        }
    }
}
