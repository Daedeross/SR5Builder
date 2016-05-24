using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace SR5Builder.DataModels
{
    public class Essence :  Attribute
    {
        public override int Max
        {
            get { return int.MaxValue; }
        }

        public override int Min
        {
            get { return 0; }
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
                    RaisePropertyChanged(nameof(AdditionalLoss));
                    RaisePropertyChanged(nameof(Loss));
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
        
        private ObservableCollection<IEssenceCost> EssenceCosts;

        public Essence(SR5Character owner, string name)
            : base(owner)
        {
            mName = name;
            EssenceCosts = new ObservableCollection<IEssenceCost>();
        }

        public void Subscribe()
        {
            mOwner.GearList.CollectionChanged += OnGearCollectionChanged;
            mOwner.ImplantList.CollectionChanged += OnGearCollectionChanged;
        }

        private void OnSubscribedChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IEssenceCost.TotalEssence))
            {
                RecalcEssenceLoss();
            }
        }

        private void OnGearModsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool recalc = false;

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
                            IEssenceCost mod = a as IEssenceCost;
                            if (mod != null)
                            {
                                EssenceCosts.Remove(mod);
                                mod.PropertyChanged -= OnSubscribedChanged;
                                recalc |= mod.TotalEssence != 0;
                            }
                        }
                    }
                }
            }

            if (e.NewItems != null)
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
                            IEssenceCost mod = a as IEssenceCost;
                            if (mod != null)
                            {
                                EssenceCosts.Remove(mod);
                                mod.PropertyChanged += OnSubscribedChanged;
                                recalc |= mod.TotalEssence != 0;
                            }
                        }
                    }
                }
            }

            if (recalc)
            {
                RecalcEssenceLoss();
            }
        }

        private void OnGearCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            bool recalc = false;

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

                            Gear gear = a as Gear;

                            if (gear != null)
                            {   
                                (gear).PropertyChanged -= OnSubscribedChanged;
                                (gear).Mods.CollectionChanged -= OnGearModsChanged;

                                // it had essence cost then we must recalc values
                                recalc |= gear.TotalEssence != 0;

                                // remove mods from local collection
                                foreach (GearMod mod in (gear).Mods)
                                {
                                    EssenceCosts.Remove(mod);
                                    mod.PropertyChanged -= OnSubscribedChanged;
                                    recalc |= mod.TotalEssence != 0;
                                }

                                EssenceCosts.Remove(gear);
                            }
                        }
                    }
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    Type valueType = item.GetType();
                    if (valueType.IsGenericType)
                    {
                        Type baseType = valueType.GetGenericTypeDefinition();
                        if (baseType == typeof(KeyValuePair<,>))
                        {
                            object a = valueType.GetProperty("Value").GetValue(item, null);

                            Gear gear = a as Gear;

                            if (gear != null)
                            {
                                gear.PropertyChanged += OnSubscribedChanged;
                                gear.Mods.CollectionChanged += OnGearModsChanged;

                                // it had essence cost then we must recalc values
                                recalc |= gear.TotalEssence != 0;

                                // add mods to local collection
                                //updating = true;
                                foreach (GearMod mod in (gear).Mods)
                                {
                                    EssenceCosts.Add(mod);
                                    mod.PropertyChanged += OnSubscribedChanged;
                                    recalc |= mod.TotalEssence != 0;
                                }
                                //updating = true;

                                EssenceCosts.Add(gear);
                            }
                        }
                    }
                }
            }

            if (recalc)
            {
                RecalcEssenceLoss();
            }
        }

        private void RecalcEssenceLoss()
        {
            decimal oldFloor = Floor;
            decimal oldLossCeiling = LossCeiling;
            decimal oldLoss = mLoss;
            mLoss = 0;
            foreach (IEssenceCost item in EssenceCosts)
            {
                mLoss += item.TotalEssence;
            }

            // This <em>should</em> be always true.
            if (mLoss != oldLoss)
            {
                RaisePropertyChanged(nameof(Loss));
                RaisePropertyChanged(nameof(Remaining));
                
                if (LossCeiling != oldLossCeiling)
                {
                    RaisePropertyChanged(nameof(LossCeiling));
                    RaisePropertyChanged(nameof(LossFloor));
                }

                if (Floor != oldFloor)
                {
                    RaisePropertyChanged(nameof(Ceiling));
                    RaisePropertyChanged(nameof(Floor));
                }
            }
        }
    }
}
