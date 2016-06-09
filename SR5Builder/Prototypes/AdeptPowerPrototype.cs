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

        public AdeptPower ToPower(SR5Character owner, string ext)
        {
            if ((ExtKind != null && ExtKind.Length > 0) && (ext == null || ext.Length == 0))
            {
                throw new ArgumentException("Power requires an name extension.", "ext");
            }

            AdeptPower p = new AdeptPower(owner);
            p.Min = Min;
            p.Max = Max;
            p.Name = string.Format(Name, ext);
            p.FlatPoints = FlatPoints;
            p.PointPerLevel = PointsPerLevel;
            p.Book = Book;
            p.Page = Page;
            p.BaseRating = 1;

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
