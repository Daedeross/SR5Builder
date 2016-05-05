using System.ComponentModel;

namespace SR5Builder.DataModels
{
    public class PowerPoints : LeveledTrait
    {
        public override int Min
        {
            get
            {
                int? v;
                v = mOwner.SpecialChoice?.PowerPoints;
                return v ?? 0;
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
                return (BaseRating - Min) * mOwner.Settings.PowerPointKarma;
            }
            set { }
        }

        public PowerPoints(SR5Character owner, string name)
            :base (owner)
        {
            mName = name;
        }

        public void OnOwnerChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Contains("Special"))
            {
                OnPropertyChanged("InprovedValue");
                OnPropertyChanged("BaseValue");
                OnPropertyChanged("AugmentedValue");
            }
        }
    }
}
