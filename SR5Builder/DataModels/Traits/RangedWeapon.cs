using SR5Builder.Loaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public class RangedWeapon: Weapon
    {
        public int AmmoCount { get; set; }

        public FireMode[] FireModes { get; set; }

        public string DisplayModes
        {
            get
            {
                
                if (FireModes == null || FireModes.Length == 0)
                    return "—";

                return String.Join(" / ", FireModes);
            }
        }

        public ReloadMethod ReloadMethod { get; set; }

        protected int mRC;
        protected int mBonusRC;
        public int RC
        {
            get { return mRC; }
            set
            {
                if (value != mRC)
                {
                    mRC = value;
                    OnPropertyChanged("RC");
                }
            }
        }

        public int AugmentedRC
        {
            get { return RC + mBonusRC; }
        }

        public string DisplayRC
        {
            get
            {
                if (mBonusRC != 0)
                {
                    if (mRC != 0)
                        return RC + " (" + AugmentedRC + ")";

                    return "(" + AugmentedRC + ")";
                }
                else
                {
                    if (mRC != 0)
                        return mRC.ToString();

                    return "—";
                }
            }
        }

        public RangedWeapon(SR5Character owner)
            :base(owner)
        {
            //FireModes = new FireMode[0];
        }

        public RangedWeapon(SR5Character owner, RangedWeaponLoader loader)
            : base(owner)
        {
            CopyFromLoader(loader);
        }

        public virtual void CopyFromLoader(RangedWeaponLoader loader)
        {
            base.CopyFromLoader(loader);

            AmmoCount = loader.AmmoCount;
            ReloadMethod = loader.ReloadMethod;
            mRC = loader.RC;
            if (loader.FireModes != null)
            {
                FireModes = new FireMode[loader.FireModes.Length];
                Array.Copy(loader.FireModes, FireModes, FireModes.Length);
            }
        }

        protected override void RecalcBonus(HashSet<string> propNames = null)
        {
            // Resets
            mBonusRC = 0;

            base.RecalcBonus(propNames);
        }

        protected override HashSet<string> AddAugment(Augment a, HashSet<string> propNames)
        {
            //return base.AddAugment(a, propNames);

            switch (a.Kind)
            {
                case AugmentKind.None:
                    break;
                case AugmentKind.Rating:
                    return base.AddAugment(a, propNames);
                case AugmentKind.DamageValue:
                    return base.AddAugment(a, propNames);
                case AugmentKind.DamageType:
                    return base.AddAugment(a, propNames);
                case AugmentKind.Accuracy:
                    return base.AddAugment(a, propNames);
                case AugmentKind.Availability:
                    return base.AddAugment(a, propNames);
                case AugmentKind.Restriction:
                    return base.AddAugment(a, propNames);
                case AugmentKind.AP:
                    mBonusAP += (int)a.Bonus;
                    propNames.Add("AP");
                    break;
                case AugmentKind.RC:
                    mBonusRC += (int)a.Bonus;
                    propNames.Add("RC");
                    break;
                default:
                    break;
            }

            return propNames;
        }
    }
}
