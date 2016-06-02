using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SR5Builder.Prototypes
{
    public class GearModPrototype: TraitPrototype
    {
        #region Properties

        public string SubCategory { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public decimal[] FlatCostVector { get; set; }
            = new decimal[1] { 0 };

        public decimal[] CostMultVector { get; set; }
            = new decimal[1] { 1 };

        [XmlIgnore]
        public string DisplayCost
        {
            get
            {
                if (CostMultVector[0] != 1)
                {
                    if (FlatCostVector[0] != 0)
                        return "×" + CostMultVector[0] + "+" + FlatCostVector[0].ToString(GlobalData.CostFormat);
                    else
                        return "×" + CostMultVector[0];
                }
                else return "+" + FlatCostVector[0].ToString(GlobalData.CostFormat);
            }
        }

        public int[] CapacityVector { get; set; }
        
        public string[] Taboo { get; set; }

        public string Notes { get; set; }

        public List<AugmentPrototype> Augments { get; set; }

        #endregion // Properties

        #region Constructors

        public GearModPrototype()
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
