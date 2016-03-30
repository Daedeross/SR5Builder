using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SR5Builder.Loaders;

namespace SR5Builder.DataModels
{
    public enum DamageType
    {
        NA = 0,
        S = 1,
        P = 2,
    }

    public class Weapon: Gear
    {
        #region Private Fields

        #endregion // Private fields

        #region Properties

        protected Skill mSkillUsed;
        public string SkillUsed
        {
            get { return mSkillUsed.Name; }
            set
            {
                if (mSkillUsed == null || mSkillUsed.Name != value)
                {
                    if (mSkillUsed == null)
                        mOwner.Agility.PropertyChanged -= this.OnSkillChanged;
                    else
                        mSkillUsed.PropertyChanged -= this.OnSkillChanged;

                    if (!mOwner.SkillList.TryGetValue(value, out mSkillUsed))
                    {
                        foreach (SkillGroup sg in mOwner.SkillGroupsList.Values)
                            if (sg.Skills.TryGetValue(value, out mSkillUsed))
                                break;
                    }

                    if (mSkillUsed == null)
                        mOwner.Agility.PropertyChanged += this.OnSkillChanged;
                    else
                        mSkillUsed.PropertyChanged += this.OnSkillChanged;

                    OnPropertyChanged("SkillUsed");
                }
            }
        }

        protected int mDV;
        protected int mBonusDV;
        public virtual int DV
        {
            get
            {
                int tmp = 0;
                if (mSkillUsed != null)
                    tmp = mSkillUsed.DamageBonus;

                return mDV + mBonusDV + 0;
            }
        }

        protected DamageType mDamageType;
        protected DamageType mAugDamageType;
        public DamageType DamageType
        {
            get
            {
                return (DamageType)Math.Max((int)mDamageType, (int)mAugDamageType);
            }
            set
            {
                if (value != mDamageType)
                {
                    mDamageType = value;
                    OnPropertyChanged("DamageType");
                    OnPropertyChanged("DisplayDamage");
                }
            }
        }

        public string DisplayDamage
        {
            get
            {
                return DV.ToString() + DamageType.ToString();
            }
        }

        protected int mAP;
        protected int mBonusAP;
        public int AP
        {
            get { return mAP + mBonusAP; }
            set
            {
                if (value != mAP)
                {
                    AP = value;
                    OnPropertyChanged("AP");
                }
            }
        }

            #region Acc

        protected int mAcc;
        public virtual int Acc
        {
            get { return mAcc; }
            set
            {
                if (value != mAcc)
                {
                    mAcc = value;
                    OnPropertyChanged("Acc");
                    OnPropertyChanged("AugmentedAcc");
                }
            }
        }

        protected int mBonusAcc;
        public int AugmentedAcc
        {
            get
            {
                int tmp = 0;
                if (mSkillUsed != null)
                    tmp = mSkillUsed.AccuracyBonus;

                return Acc + mBonusAcc + tmp;
            }
        }

        public string DisplayAcc
        {
            get
            {
                if (mBonusAcc != 0)
                    return Acc + " (" + AugmentedAcc + ")";
                else return Acc.ToString();
            }
        }

            #endregion

        public int TotalPool
        {
            get
            {
                if (mSkillUsed == null)
                {
                    return mOwner.Agility.AugmentedRating - 1;
                }
                else return mSkillUsed.AugmentedPool;
            }
        }

        #endregion // Properties

        #region Constructors

        public Weapon(SR5Character owner)
            : base(owner)
        {
            mOwner.Agility.PropertyChanged += this.OnSkillChanged;
        }

        public Weapon(SR5Character owner, WeaponLoader loader)
            :base(owner)
        {
            mOwner.Agility.PropertyChanged += this.OnSkillChanged;

            CopyFromLoader(loader);
        }

        #endregion // Constructors

        #region Private Methods

        private void OnSkillChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("AugmentedAcc");
            OnPropertyChanged("TotalPool");
        }

        protected override void RecalcBonus(HashSet<string> propNames = null)
        {
            // Resets
            mBonusDV = 0;
            mAugDamageType = DataModels.DamageType.NA;
            mBonusAcc = 0;

            base.RecalcBonus(propNames);
        }

        protected override HashSet<string> AddAugment(Augment a, HashSet<string> propNames)
        {
            switch (a.Kind)
            {
                case AugmentKind.None:
                    break;
                case AugmentKind.Rating:
                    return base.AddAugment(a, propNames);
                case AugmentKind.DamageValue:
                    mBonusDV += (int)a.Bonus;
                    propNames.Add("DV");
                    propNames.Add("DisplayDamage");
                    break;
                case AugmentKind.DamageType:
                    mAugDamageType = (DamageType)Math.Max((int)mAugDamageType, (int)a.Bonus);
                    propNames.Add("DamageType");
                    propNames.Add("DisplayDamage");
                    break;
                case AugmentKind.Accuracy:
                    mBonusAcc += (int)a.Bonus;
                    propNames.Add("Acc");
                    propNames.Add("AugmentedAcc");
                    break;
                case AugmentKind.Availability:
                    return base.AddAugment(a, propNames);
                case AugmentKind.Restriction:
                    return base.AddAugment(a, propNames);
                case AugmentKind.AP:
                    mBonusAP += (int)a.Bonus;
                    propNames.Add("AP");
                    break;
                case AugmentKind.RC:
                    break;
                default:
                    break;
            }
            return propNames;
        }

        #endregion // Private Methods

        #region Public Methods

        public virtual void CopyFromLoader(WeaponLoader loader)
        {
            base.CopyFromLoader(loader);

            mDV = loader.DV;
            mDamageType = loader.DamageType;
            mAP = loader.AP;
            mAcc = loader.Acc;
        }

        #endregion // Public Methods
    }
}
