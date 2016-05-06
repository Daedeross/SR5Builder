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
                if (mOwner.SpecialChoice.Name == "Adept")
                {
                    return mOwner.SpecialAttribute;
                }
                return 0;
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
                return mOwner.SpecialAttribute;
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
                OnPropertyChanged("ImprovedRating");
                OnPropertyChanged("BaseRating");
                OnPropertyChanged("AugmentedRating");
            }
        }
    }
}
