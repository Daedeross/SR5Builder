using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SR5Builder.DataModels;
using System.Globalization;

namespace SR5Builder.Prototypes
{
    [XmlInclude(typeof(WeaponPrototype)),
    XmlInclude(typeof(MeleeWeaponPrototype)),
    XmlInclude(typeof(RangedWeaponPrototype))]
    public class GearPrototype: TraitPrototype
    {
        #region Properties

        [XmlIgnore]
        public string DisplayName
        {
            get
            {
                if (ExtLabel != null && ExtLabel.Length > 0)
                    return Name + " [" + ExtLabel + "]";
                else return Name;
            }
        }

        [XmlIgnore]
        protected string mDisplayCost;
        public string DisplayCost
        {
            get
            {
                if (mDisplayCost != null)
                {
                    return mDisplayCost;
                }
                if (Max > 1 && CostVector.Length > 1)
                {
                    bool isSimple = true;
                    decimal[] diffs = new decimal[CostVector.Length-1];
                    for (int i = 0; i < CostVector.Length-1; i++)
                    {
                        diffs[i] = CostVector[i+1] - CostVector[i];
                    }
                    for (int i = 1; i < diffs.Length; i++)
                    {
                        if (diffs[i] != diffs[i-1])
                        {
                            isSimple = false;
                            break;
                        }
                    }
                    if (isSimple)
                    {
                        mDisplayCost = "Rating × " + diffs[0].ToString("C", GlobalData.CostFormat);
                    }
                    else
                    {
                        string minCost = CostVector[Min].ToString("C", GlobalData.CostFormat);
                        string maxCost = CostVector[Max].ToString("C", GlobalData.CostFormat);
                        mDisplayCost = minCost + "-" + maxCost;
                    }
                }
                else
                {
                    mDisplayCost = CostVector[0].ToString("C", GlobalData.CostFormat);
                }
                return mDisplayCost;
            }
        }

        public bool IsArmor { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public List<string> ExtArray { get; set; }

        public string ExtKind { get; set; }

        public string ExtLabel { get; set; }

        public bool HasRating { get; set; }

        public int Rating { get; set; }

        public Availability[] AvailabilityVector { get; set; }
            = new Availability[1];

        public string Availability
        {
            get
            {
                if (AvailabilityVector.Length > 1)
                {
                    string minA = AvailabilityVector[Min].ToString();
                    string maxA = AvailabilityVector[Max].ToString();
                    return minA + "-" + maxA;
                }
                else return AvailabilityVector[0].ToString();
            }
        }

        public decimal[] CostVector { get; set; } = new decimal[1];

        public int[] CapacityVector { get; set; } = new int[1];

        public decimal[] EssenceVector { get; set; } = new decimal[1];

        public string EssenceCost
        {
            get
            {
                if (EssenceVector.Length > 1)
                {
                    decimal minE = EssenceVector[Min];
                    decimal maxE = EssenceVector[Max];
                    return string.Format("{0:F1}-{1:F1}", minE, maxE);
                }
                else return EssenceVector[0].ToString("F1");
            }
        }

        public string[] BaseMods { get; set; }

        public string[] ModCategories { get; set; }

        public string[] Mods { get; set; }

        public string Notes { get; set; }

        #endregion // Properties

        #region Constructors

        #endregion // Constructors

        #region Public

        public virtual Gear ToGear(SR5Character owner)
        {
            return new Gear(owner, this);
        }

        #endregion // Public
    }
}
