using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Attribute = SR5Builder.DataModels.Attribute;

namespace SR5Builder.DataModels
{
    public class Skill: LeveledTrait, IKarmaCost
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
                    OnPropertyChanged(nameof(Owner));
                    OnPropertyChanged(nameof(TotalPool));
                    OnPropertyChanged(nameof(AugmentedPool));
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

                        OnPropertyChanged(nameof(TotalPool));
                        OnPropertyChanged(nameof(AugmentedPool));
                    }
                }
            }
        }

        public override int Min
        {
            get { return base.Min + ExtraMin; }
            set { base.Min = value - ExtraMin; }
        }

        public int StartingMax 
        {
            get { return Owner.Settings.StartingSkillCap + ExtraMax; }
        }

        public override int Max
        {
            get
            {
                return Owner.Settings.InPlaySkillCap + ExtraMax;
            }
            set { }
        }

        public override int BaseRating
        {
            get
            {
                return base.BaseRating;
            }
            set
            {
                if (value <= StartingMax)
                {
                    base.BaseRating = value;
                    OnPropertyChanged(nameof(TotalPool));
                    OnPropertyChanged(nameof(AugmentedPool));
                }
            }
        }

        public override int ImprovedRating
        {
            get { return base.ImprovedRating; }
            set
            {
                base.ImprovedRating = value;
                OnPropertyChanged(nameof(TotalPool));
                OnPropertyChanged(nameof(AugmentedPool));
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
                    OnPropertyChanged(nameof(AccuracyBonus));
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
                    OnPropertyChanged(nameof(DamageBonus));
                }
            }
        }

        public int Karma
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
            OnPropertyChanged(nameof(TotalPool));
            OnPropertyChanged(nameof(AugmentedPool));
        }

        protected override void RecalcBonus(HashSet<string> propNames = null)
        {
            //create HashSet if needed
            if (propNames == null)
                propNames = new HashSet<string>();

            //Resets
            ExtraMax = 0;
            ExtraMin = 0;
            mAccuracyBonus = 0;
            mDamageBonus = 0;

 	        base.RecalcBonus(propNames);

            foreach (string name in propNames)
            {
                OnPropertyChanged(name);
            }
            if (BaseRating > StartingMax)
            {
                BaseRating = StartingMax;
            }
            if (ImprovedRating > Max)
            {
                ImprovedRating = Max;
            }
        }

        protected override HashSet<string> AddAugment(Augment a, HashSet<string> propNames)
        {
            switch (a.Kind)
            {
                case AugmentKind.None:
                    break;
                case AugmentKind.Rating:
                    propNames = base.AddAugment(a, propNames);
                    propNames.Add(nameof(AugmentedPool));
                    break;
                case AugmentKind.DamageValue:
                    mDamageBonus += (int)a.Bonus;
                    propNames.Add(nameof(DamageBonus));
                    break;
                case AugmentKind.DamageType:
                    break;
                case AugmentKind.Accuracy:
                    mAccuracyBonus += (int)a.Bonus;
                    propNames.Add(nameof(AccuracyBonus));
                    break;
                case AugmentKind.Availability:
                    break;
                case AugmentKind.Restriction:
                    break;
                case AugmentKind.Max:
                    ExtraMax += (int)a.Bonus;
                    propNames.Add(nameof(StartingMax));
                    propNames.Add(nameof(Max));
                    break;
                default:
                    break;
            }
            return propNames;
        }

        public override void OnAugmentRemoving(AugmentKind kind)
        {
            base.OnAugmentRemoving(kind);
            switch (kind)
            {
                case AugmentKind.None:
                    break;
                case AugmentKind.Rating:
                    RemovedNames.Add(nameof(AugmentedPool));
                    break;
                case AugmentKind.Max:
                    RemovedNames.Add(nameof(StartingMax));
                    RemovedNames.Add(nameof(Max));
                    break;
                case AugmentKind.DamageValue:
                    RemovedNames.Add(nameof(DamageBonus));
                    break;
                case AugmentKind.DamageType:
                    break;
                case AugmentKind.Accuracy:
                    RemovedNames.Add(nameof(AccuracyBonus));
                    break;
                case AugmentKind.Availability:
                    break;
                case AugmentKind.Restriction:
                    break;
                case AugmentKind.RC:
                    break;
                case AugmentKind.AP:
                    break;
                default:
                    break;
            }
        }

        #endregion // Private Methods

        #region Operators

        public static implicit operator int(Skill lt)
        {
            return lt?.AugmentedRating ?? 0;
        }

        public static int operator -(Skill t)
        {
            return -t?.AugmentedRating ?? 0;
        }

        public static int operator +(Skill l, Skill r)
        {
            return l?.AugmentedRating ?? 0 + r?.AugmentedRating ?? 0;
        }
        public static int operator +(Skill l, int r)
        {
            return l?.AugmentedRating ?? 0 + r;
        }
        public static int operator +(int l, Skill r)
        {
            return l + r?.AugmentedRating ?? 0;
        }

        public static int operator -(Skill l, Skill r)
        {
            return l?.AugmentedRating ?? 0 - r?.AugmentedRating ?? 0;
        }
        public static int operator -(Skill l, int r)
        {
            return l?.AugmentedRating ?? 0 - r;
        }
        public static int operator -(int l, Skill r)
        {
            return l - r?.AugmentedRating ?? 0;
        }

        public static int operator *(Skill l, Skill r)
        {
            return l?.AugmentedRating ?? 0 * r?.AugmentedRating ?? 0;
        }
        public static int operator *(Skill l, int r)
        {
            return l?.AugmentedRating ?? 0 * r;
        }
        public static int operator *(int l, Skill r)
        {
            return l * r?.AugmentedRating ?? 0;
        }

        public static int operator /(Skill l, Skill r)
        {
            return l?.AugmentedRating ?? 0 / r?.AugmentedRating ?? 0;
        }
        public static int operator /(Skill l, int r)
        {
            return l?.AugmentedRating ?? 0 / r;
        }
        public static int operator /(int l, Skill r)
        {
            return l / r?.AugmentedRating ?? 0;
        }

        public static int operator %(Skill l, Skill r)
        {
            return l?.AugmentedRating ?? 0 + r?.AugmentedRating ?? 0;
        }
        public static int operator %(Skill l, int r)
        {
            return l?.AugmentedRating ?? 0 + r;
        }
        public static int operator %(int l, Skill r)
        {
            return l + r?.AugmentedRating ?? 0;
        }

        #region Comparison

        public static bool operator <(Skill l, Skill r)
        {
            return l.AugmentedRating < r.AugmentedRating;
        }
        public static bool operator <(Skill l, int r)
        {
            return l.AugmentedRating < r;
        }
        public static bool operator <(int l, Skill r)
        {
            return l < r.AugmentedRating;
        }

        public static bool operator >(Skill l, Skill r)
        {
            return l.AugmentedRating > r.AugmentedRating;
        }
        public static bool operator >(Skill l, int r)
        {
            return l.AugmentedRating > r;
        }
        public static bool operator >(int l, Skill r)
        {
            return l > r.AugmentedRating;
        }


        public static bool operator <=(Skill l, Skill r)
        {
            return l.AugmentedRating < r.AugmentedRating;
        }
        public static bool operator <=(Skill l, int r)
        {
            return l.AugmentedRating < r;
        }
        public static bool operator <=(int l, Skill r)
        {
            return l < r.AugmentedRating;
        }

        public static bool operator >=(Skill l, Skill r)
        {
            return l.AugmentedRating > r.AugmentedRating;
        }
        public static bool operator >=(Skill l, int r)
        {
            return l.AugmentedRating > r;
        }
        public static bool operator >=(int l, Skill r)
        {
            return l > r.AugmentedRating;
        }

        #endregion

        #endregion

    }
}
