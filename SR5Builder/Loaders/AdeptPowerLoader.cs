using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace SR5Builder.Loaders
{
    [XmlRoot(Namespace="SR5Builder/AdeptPowers.xsd")]
    public class AdeptPowerLoader: TraitExtLoader
    {
        public int Min { get; set; }

        public int Max { get; set; }

        public float FlatPoints { get; set; }

        public float PointsPerLevel { get; set; }

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

        public AugmentLoader[] Augments { get; set; }

        public AdeptPower ToPower(SR5Character owner, string ext)
        {
            if ((ExtKind != null && ExtKind.Length > 0) && (ext == null || ext.Length == 0))
            {
                throw new ArgumentException("Power requires an name extension.", "ext");
            }

            AdeptPower p = new AdeptPower(owner);
            p.Min = Min;
            p.Max = Max;
            p.Name = Name;
            if (ExtKind != null && ExtKind.Length > 0)
            {
                p.Name += " [" + ext + "]";
            }
            p.FlatPoints = FlatPoints;
            p.PointPerLevel = PointsPerLevel;
            p.Book = Book;
            p.Page = Page;
            p.BaseRating = 1;

            foreach (AugmentLoader a in Augments)
            {
                if (a.Target == "%ext%")
                    p.GivenAugments.Add(a.ToAugment(p, ext));
                else p.GivenAugments.Add(a.ToAugment(p));
            }

            return p;
        }

        public static List<AdeptPowerLoader> LoadFromFile(string filename)
        {
            List<AdeptPowerLoader> list;

            XmlSerializer ser = new XmlSerializer(typeof(List<AdeptPowerLoader>), "SR5Builder/AdeptPowers.xsd");
            StreamReader reader = new StreamReader(filename);

            list = (List<AdeptPowerLoader>)ser.Deserialize(reader);

            reader.Close();

            return list;
        }
    }
}
