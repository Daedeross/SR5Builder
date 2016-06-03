using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace SR5Builder.Prototypes
{
    public class GearModPrototype: TraitPrototype, INotifyPropertyChanged
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
                if (mRating < 0)
                {
                    int mulMinIndex = Min < CostMultVector.Length ? Min : 0;
                    int mulMaxIndex = Max < CostMultVector.Length ? Max : 0;
                    decimal mulMin = CostMultVector[mulMinIndex];
                    decimal mulMax = CostMultVector[mulMaxIndex];
                    string output = "";
                    if (mulMin != 1 && mulMax != 1)
                    {
                        if (mulMin != mulMax)
                        {
                            output += string.Format("×({0}-{1})", mulMin, mulMax);
                        }
                        else
                        {
                            output += string.Format("×{0}", mulMin);
                        }
                    }

                    int flatMinIndex = Min < FlatCostVector.Length ? Min : 0;
                    int flatMaxIndex = Max < FlatCostVector.Length ? Max : 0;
                    decimal flatMin = FlatCostVector[flatMinIndex];
                    decimal flatMax = FlatCostVector[flatMaxIndex];

                    if (flatMin != flatMax)
                    {
                        output += string.Format("+({0}-{1})", flatMin, flatMax) + GlobalData.CostFormat.CurrencySymbol;
                    }
                    else if (flatMin != 0)
                    {
                        output += string.Format("+{0}", flatMin.ToString("C", GlobalData.CostFormat));
                    }

                    return output;
                }
                int flatIndex = mRating < FlatCostVector.Length ? mRating : 0;
                int multIndex = mRating < CostMultVector.Length ? mRating : 0;
                if (CostMultVector[multIndex] != 1)
                {
                    if (FlatCostVector[flatIndex] != 0)
                        return "×" + CostMultVector[multIndex] + "+" + FlatCostVector[flatIndex].ToString("C", GlobalData.CostFormat);
                    else
                        return "×" + CostMultVector[multIndex];
                }
                else return "+" + FlatCostVector[flatIndex].ToString("C", GlobalData.CostFormat);
            }
        }

        public int[] CapacityVector { get; set; }
        
        public string[] Taboo { get; set; }

        public string Notes { get; set; }

        public List<AugmentPrototype> Augments { get; set; }

        #region Display Properties

        private int mRating = -1;
        [XmlIgnore]
        public int Rating
        {
            get { return mRating; }
            set
            {
                if (mRating != value)
                {
                    mRating = value;
                    OnPropertyChanged(nameof(Rating));
                    OnPropertyChanged(nameof(DisplayRating));
                    OnPropertyChanged(nameof(DisplayCost));
                    OnPropertyChanged(nameof(Capacity));
                }
            }
        }

        [XmlIgnore]
        public string DisplayRating
        {
            get
            {
                if (mRating >= 0)
                {
                    if (mRating == 0)
                    {
                        return "—";
                    }
                    return mRating.ToString();
                }
                else
                {
                    if (Min == Max)
                    {
                        if (Min == 0)
                        {
                            return "—";
                        }
                        return Min.ToString();
                    }
                    return string.Format("{0} - {1}", Min, Max);
                }
            }
        }

        [XmlIgnore]
        public int Capacity
        {
            get
            {
                int index = mRating < 0 ? 0 : mRating;
                index = index < CapacityVector.Length ? index : CapacityVector.Length;
                return CapacityVector[index];
            }
        }

        #endregion  //Display Properties

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public GearModPrototype Clone(int rating)
        {
            GearModPrototype clone = (GearModPrototype)MemberwiseClone();
            clone.mRating = rating;
            return clone;
        }
    }
}
