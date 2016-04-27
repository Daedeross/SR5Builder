using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Attribute = SR5Builder.DataModels.Attribute;

namespace SR5Builder.DataModels
{
    public class Skill: LeveledTrait
    {
        #region Comparisons

        public class SkillNameEquals: EqualityComparer<Skill>
        {
            public override bool Equals(Skill x, Skill y)
            {
                return x.mName.Equals(y.mName);
            }

            public override int GetHashCode(Skill obj)
            {
                return obj.mName.GetHashCode();
            }
        }

        #endregion // Comparisons

        #region Properties

        public override SR5Character Owner
        {
            get { return mOwner; }
            set
            {
                if (mOwner != value)
                {
                    //if(mOwner != null)
                    //    mOwner.PropertyChanged -= this.OnCharacterChanged;

                    mOwner = value;
                    //mOwner.PropertyChanged += this.OnCharacterChanged;
                    OnPropertyChanged("Owner");
                    OnPropertyChanged("TotalPool");
                    OnPropertyChanged("AugmentedPool");
                }
            }
        }

        public SkillType Kind { get; set; }

        public string GroupName { get; set; }

        protected Attribute mLinkedAttribute;
        public string LinkedAttribute
        {
            get { return mLinkedAttribute.Name; }
            set
            {
                if (mLinkedAttribute == null || mLinkedAttribute.Name != value)
                {
                    if (value == "Magic" || value == "Resonance")
                    {
                        value = "Special";
                    }
                    Attribute a;
                    if (mOwner.Attributes.TryGetValue(value, out a))
                    {
                        if (mLinkedAttribute != null)
                        {
                            mLinkedAttribute.PropertyChanged -= this.OnAttributeChanged;
                        }
                        mLinkedAttribute = a;

                        mLinkedAttribute.PropertyChanged += this.OnAttributeChanged;

                        OnPropertyChanged("TotalPool");
                        OnPropertyChanged("AugmentedPool");
                    }
                }
            }
        }

        public override int BaseRating
        {
            get
            {
                return base.BaseRating;
            }
            set
            {
                base.BaseRating = value;
                OnPropertyChanged("TotalPool");
                OnPropertyChanged("AugmentedPool");
            }
        }

        public int TotalPool
        {
            get
            {
                if (mLinkedAttribute == null)
                    return ImprovedRating;
                return ImprovedRating + mLinkedAttribute.BaseRating;
            }
        }

        public int AugmentedPool
        {
            get
            {
                if (mLinkedAttribute == null)
                    return AugmentedRating;
                return AugmentedRating + mLinkedAttribute.AugmentedRating;
            }
        }

        public string UsualLimit { get; set; }

        protected int mAccuracyBonus = 0;
        public int AccuracyBonus
        {
            get { return mAccuracyBonus; }
            set
            {
                if (value != mAccuracyBonus)
                {
                    mAccuracyBonus = value;
                    OnPropertyChanged("AccuracyBonus");
                }
            }
        }

        protected int mDamageBonus;
        public int DamageBonus
        {
            get { return mDamageBonus; }
            set
            {
                if (value != mDamageBonus)
                {
                    mDamageBonus = value;
                    OnPropertyChanged("DamageBonus");
                }
            }
        }

        public override int Karma
        {
            get
            {
                switch (Kind)
                {
                    case SkillType.NA:
                        return 0;
                    case SkillType.Active:
                        return mOwner.Settings.ActiveSkillKarma(ImprovedRating, BaseRating);
                    case SkillType.Magical:
                        return mOwner.Settings.MagicSkillKarma(ImprovedRating, BaseRating);
                    case SkillType.Resonance:
                        return mOwner.Settings.ResonanceSkillKarma(ImprovedRating, BaseRating);
                    case SkillType.Knowledge:
                        return mOwner.Settings.KnowledgeSkillKarma(ImprovedRating, BaseRating);
                    case SkillType.Language:
                        return mOwner.Settings.LanguageSkillKarma(ImprovedRating, BaseRating);
                    default:
                        return 0;
                }
            }
            set { }
        }

        #endregion // Properties

        #region Consturctors

        public Skill()
            :base(null)
        {

        }

        public Skill(SR5Character owner)
            :base(owner)
        {
            
        }

        #endregion // Consturctors

        #region Private Methods

        private void OnAttributeChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("TotalPool");
            OnPropertyChanged("AugmentedPool");
        }

        protected override void RecalcBonus(HashSet<string> propNames = null)
        {
            //create HashSet if needed
            if (propNames == null)
                propNames = new HashSet<string>();

            //Resets
            mAccuracyBonus = 0;
            mDamageBonus = 0;

 	        base.RecalcBonus(propNames);
        }

        protected override HashSet<string> AddAugment(Augment a, HashSet<string> propNames)
        {
            switch (a.Kind)
            {
                case AugmentKind.None:
                    break;
                case AugmentKind.Rating:
                    propNames = base.AddAugment(a, propNames);
                    propNames.Add("AugmentedPool");
                    break;
                case AugmentKind.DamageValue:
                    mDamageBonus += (int)a.Bonus;
                    propNames.Add("DamageBonus");
                    break;
                case AugmentKind.DamageType:
                    break;
                case AugmentKind.Accuracy:
                    mAccuracyBonus += (int)a.Bonus;
                    propNames.Add("AccuracyBonus");
                    break;
                case AugmentKind.Availability:
                    break;
                case AugmentKind.Restriction:
                    break;
                default:
                    break;
            }
            return propNames;
        }

        #endregion // Private Methods

    }
}
