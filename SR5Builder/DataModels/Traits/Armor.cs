using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public class Armor: LeveledTrait
    {
        public override int Max
        {
            get { return int.MaxValue; }
            set { base.Max = value; }
        }

        public override int Min
        {
            get { return 0; }
            set { base.Min = value; }
        }

        
        public override int BaseRating
        {
            get
            {
                return mBaseRating;
            }
            set { }
        }

        public override int AugmentedRating
        {
            get
            {
                return mBaseRating + (int)mBonusRating;
            }
        }

        protected List<IArmor> Armors { get; set; }
            = new List<IArmor>();

        public Armor(SR5Character owner)
            :base(owner)
        {
            Name = "Armor";
            owner.GearList.CollectionChanged += OnGearChanged;
            owner.ImplantList.CollectionChanged += OnImpantsChanged;
        }

        private void OnGearChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool isArmor = false;
            if (e.OldItems != null)
                foreach (KeyValuePair<string, Gear> kvp in e.OldItems)
                    if (kvp.Value.IsArmor)
                    {
                        isArmor = true;
                        Armors.Remove(kvp.Value);
                        kvp.Value.PropertyChanged -= OnArmorChanged;
                    }

            if (e.NewItems != null)
                foreach (KeyValuePair<String, Gear> kvp in e.NewItems)
                    if (kvp.Value.IsArmor)
                    {
                        isArmor = true;
                        Armors.Add(kvp.Value);
                        kvp.Value.PropertyChanged += OnArmorChanged;
                    }

            if (isArmor)
            {
                RecalcArmor();
            }
        }

        private void OnImpantsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool isArmor = false;
            if (e.OldItems != null)
                foreach (KeyValuePair<string, Implant> kvp in e.OldItems)
                    if (kvp.Value.IsArmor)
                    {
                        isArmor = true;
                        Armors.Remove(kvp.Value);
                        kvp.Value.PropertyChanged -= OnArmorChanged;
                    }

            if (e.NewItems != null)
                foreach (KeyValuePair<String, Implant> kvp in e.NewItems)
                    if (kvp.Value.IsArmor)
                    {
                        isArmor = true;
                        Armors.Add(kvp.Value);
                        kvp.Value.PropertyChanged += OnArmorChanged;
                    }

            if (isArmor)
            {
                RecalcArmor();
            }
        }

        private void OnArmorChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IArmor.ArmorRating))
            {
                RecalcArmor();
            }
        }

        private void RecalcArmor()
        {
            int oldArmor = mBaseRating;
            int max = 0;
            mBaseRating = 0;
            foreach (IArmor a in Armors)
            {
                if (a.IsClothing)
                {
                    max = Math.Max(a.ArmorRating, max);
                }
                else
                {
                    mBaseRating += a.ArmorRating;
                }
            }

            mBaseRating += max;
            if (mBaseRating != oldArmor)
            {
                OnPropertyChanged(nameof(BaseRating));
                OnPropertyChanged(nameof(AugmentedRating));
                OnPropertyChanged(nameof(DisplayValue));
            }
        }
    }
}
