using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SR5Builder.DataModels;
using System.Globalization;

namespace SR5Builder.Loaders
{
    [XmlInclude(typeof(WeaponLoader)),
    XmlInclude(typeof(MeleeWeaponLoader)),
    XmlInclude(typeof(RangedWeaponLoader))]
    public class GearLoader: TraitLoader
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
        public string DisplayCost
        {
            get
            {
                if (RatingCost != 0)
                {
                    if (FlatCost != 0)
                    {
                        return FlatCost + " + Rating ×" + RatingCost.ToString("C", GlobalData.CostFormat);
                    }
                    return "Rating ×" + RatingCost.ToString("C", GlobalData.CostFormat);
                }
                return FlatCost.ToString("C", GlobalData.CostFormat);
            }
        }

        public int Min { get; set; }

        public int Max { get; set; }

        public List<string> ExtArray { get; set; }

        public string ExtKind { get; set; }

        public string ExtLabel { get; set; }

        public bool HasRating { get; set; }

        public int Rating { get; set; }

        public Availability Availability { get; set; }

        public decimal FlatCost { get; set; }

        public decimal RatingCost { get; set; }

        public int Capacity { get; set; }

        public decimal FlatEssence { get; set; }

        public decimal RatingEssence { get; set; }

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
