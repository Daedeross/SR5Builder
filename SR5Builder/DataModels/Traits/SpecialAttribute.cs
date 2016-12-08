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
                return (int)mMin.GetValue(mOwner.SpecialChoice, null) - mOwner.Essence.LossCeiling;
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
                return mOwner.AdvancedGrade + mOwner.Essence.Floor;
            }
        }

        private int loss;
        public override int BaseRating
        {
            get
            {
                return mBaseRating - loss + Min; 
            }
            set
            {
                if (value != BaseRating)
                {
                    int oldRating = mBaseRating;
                    mBaseRating = value + loss - Min;
                    OnPropertyChanged(nameof(BaseRating));
                    OnPropertyChanged(nameof(ImprovedRating));
                    OnPropertyChanged(nameof(AugmentedRating));
                    if (oldRating != mBaseRating)
                    {

                        OnPropertyChanged(nameof(Points));
                    } 
                }
            }
        }

        public SpecialAttribute(SR5Character owner)
            :base (owner)
        {
            mOwner = owner;
            mName = mOwner.SpecialKind.ToString();

            mMin = (typeof(SpecialChoice)).GetProperty("Attribute");

            mOwner.Essence.PropertyChanged += this.OnSubscribedChanged;
            mOwner.PropertyChanged += this.OnSubscribedChanged;
            mOwner.Priorities.PropertyChanged += this.OnSubscribedChanged;
        }

        private void OnSubscribedChanged(object sender, PropertyChangedEventArgs e)
        {
            if (   (ReferenceEquals(sender, mOwner.Essence) && e.PropertyName == nameof(Essence.Floor))
                || (ReferenceEquals(sender, mOwner.Priorities) && e.PropertyName == nameof(Priorities.Special))
                || (ReferenceEquals(sender, mOwner) && e.PropertyName == nameof(SR5Character.SpecialChoice))
               )
            {
                int oldLoss = loss;
                int oldRating = BaseRating;
                loss = mOwner.Essence.LossCeiling;
                int oldBase = mBaseRating;

                if (BaseRating > Max)
                {
                    BaseRating = Max;
                }
                if (BaseRating < Min)
                {
                    BaseRating = Min;
                }
                if (oldRating != BaseRating)
                {
                    OnPropertyChanged(nameof(BaseRating));
                    OnPropertyChanged(nameof(AugmentedRating));
                }
                if (loss != oldLoss)
                {
                    OnPropertyChanged(nameof(Max));
                }
                if (oldBase != mBaseRating)
                {
                    OnPropertyChanged(nameof(Points));
                }
                
                OnPropertyChanged(nameof(Min));
                OnPropertyChanged(nameof(BaseRating));
                OnPropertyChanged(nameof(AugmentedRating));
            }
        }
    }
}