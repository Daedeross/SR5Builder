using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SR5Builder.Loaders
{
    public class GearModLoader: TraitLoader
    {
        #region Properties

        public int Rating { get; set; }

        public int FlatCost { get; set; }

        public decimal CostMult { get; set; }

        [XmlIgnore]
        public string DisplayCost
        {
            get
            {
                if (CostMult != 1)
                {
                    return "×" + CostMult + " +" + FlatCost + "¥";
                }
                else return "+" + FlatCost + "¥";
            }
        }

        public int Capacity { get; set; }

        public string[] Taboo { get; set; }

        public List<AugmentLoader> Augments { get; set; }

        #endregion // Properties

        #region Constructors

        public GearModLoader()
        {
            //Taboo = string[0];
        }

        #endregion // Constructors

        #region Methods

        public GearMod ToMod(Gear gearPiece)
        {
            return new GearMod(gearPiece, this);
        }

        #endregion // Methods
    }
}
