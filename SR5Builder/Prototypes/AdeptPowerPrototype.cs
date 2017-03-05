using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace SR5Builder.Prototypes
{
    [XmlRoot(Namespace="SR5Builder/AdeptPowers.xsd")]
    public class AdeptPowerPrototype: TraitExtPrototype
    {
        string[] Prerequisites { get; set; }

        public int Min { get; set; }

        public int Max { get; set; }

        public decimal FlatPoints { get; set; }

        public decimal PointsPerLevel { get; set; }

        [XmlIgnore]
        public string DisplayPoints
        {
            get
            {
                string tmp;
                if (FlatPoints != 0)
                    tmp = FlatPoints + "+" + PointsPerLevel;
                else
                    tmp = PointsPerLevel.ToString();

                if (Max > 1)
                    tmp += " per lv";

                return tmp;
            }
        }

        public AugmentPrototype[] Augments { get; set; }

        public virtual AdeptPower ToPower(SR5Character owner, string ext)
        {
            CheckExt(ext);

            AdeptPower p = new AdeptPower(owner, Name, ext)
            {
                Min = Min,
                Max = Max,
                Name = string.Format(Name, ext),
                FlatPoints = FlatPoints,
                PointPerLevel = PointsPerLevel,
                Book = Book,
                Page = Page,
                BaseRating = 1
            };
            foreach (AugmentPrototype a in Augments)
            {
                if (a.Target == "%ext%")
                    p.GivenAugments.Add(a.ToAugment(p, ext));
                else p.GivenAugments.Add(a.ToAugment(p));
            }

            return p;
        }

        public static List<AdeptPowerPrototype> LoadFromFile(string filename)
        {
            List<AdeptPowerPrototype> list;

            XmlSerializer ser = new XmlSerializer(typeof(List<AdeptPowerPrototype>), "SR5Builder/AdeptPowers.xsd");
            StreamReader reader = new StreamReader(filename);

            list = (List<AdeptPowerPrototype>)ser.Deserialize(reader);

            reader.Close();

            return list;
        }
    }
}
