using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DrWPF.Windows.Data;

namespace SR5Builder.DataModels
{
    public class Essence :  BaseTrait
    {
        public override int Karma
        {
            get
            {
                return 0;
            }
            set { }
        }

        private decimal mBaseRating;
        public decimal BaseRating
        {
            get { return mBaseRating; }
            set
            {
                mBaseRating = value;
                OnPropertyChanged(nameof(BaseRating));
            }
        }

        private decimal mAdditionalLoss;
        public decimal AdditionalLoss
        {
            get { return mAdditionalLoss; }
            set
            {
                if (mAdditionalLoss != value)
                {
                    mAdditionalLoss = value;
                    OnPropertyChanged(nameof(AdditionalLoss));
                    OnPropertyChanged(nameof(Loss));
                }
            }
        }

        private decimal mLoss;
        public decimal Loss
        {
            get { return mLoss + mAdditionalLoss; }
        }

        public int LossFloor
        {
            get { return (int)Math.Floor(Loss); }
        }

        public int LossCeiling
        {
            get { return (int)Math.Ceiling(Loss); }
        }

        public decimal Remaining
        {
            get { return mBaseRating - Loss; }
        }

        public int Floor
        {
            get { return (int)Math.Floor(Remaining); }
        }

        public int Ceiling
        {
            get { return (int)Math.Ceiling(Remaining); }
        }

        private ObservableDictionary<IEssenceCost> EssenceCosts;

        public Essence(SR5Character owner, string name)
            : base(owner)
        {
            mName = name;
            owner.GearList.CollectionChanged += OnGearCollectionChanged;
        }

        private void OnGearChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IEssenceCost.TotalEssence))
            {
                RecalcEssenceLoss();
            }
        }

        private void OnGearCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            HashSet<string> propNames = new HashSet<string>();

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    Type valueType = item.GetType();
                    if (valueType.IsGenericType)
                    {
                        Type baseType = valueType.GetGenericTypeDefinition();
                        if (baseType == typeof(KeyValuePair<,>))
                        {
                            object a = valueType.GetProperty("Value").GetValue(item, null);

                            if (a is IEssenceCost)
                            {
                                (a as IEssenceCost).PropertyChanged -= OnGearChanged;
                                propNames.Add(nameof(Loss));
                            }
                        }
                    }
                }
            }
        }

        private void RecalcEssenceLoss()
        {
            
        }
    }
}
