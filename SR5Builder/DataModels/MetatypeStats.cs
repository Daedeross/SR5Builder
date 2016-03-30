using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace SR5Builder.DataModels
{
    //[XmlRoot(Namespace="SR5Builder/Metatypes.xsd")]
    public class MetatypeStats : DataModelBase, IAugmentable
    {
        public int BodyMin { get; set; }

        public int BodyMax { get; set; }

        public int AgilityMin { get; set; }

        public int AgilityMax { get; set; }

        public int ReactionMin { get; set; }

        public int ReactionMax { get; set; }

        public int StrengthMin { get; set; }

        public int StrengthMax { get; set; }

        public int WillpowerMin { get; set; }

        public int WillpowerMax { get; set; }

        public int LogicMin { get; set; }

        public int LogicMax { get; set; }

        public int IntuitionMin { get; set; }

        public int IntuitionMax { get; set; }

        public int CharismaMin { get; set; }

        public int CharismaMax { get; set; }

        public int EdgeMin { get; set; }

        public int EdgeMax { get; set; }

        private int mReach;
        public int Reach
        {
            get { return mReach; }
            set
            {
                if (value != mReach)
                    mReach = value;
            }
        }

        //private int mBonusReach;
        //[XmlIgnore]
        //public int AugmentedReach
        //{
        //    get { return mReach + mBonusReach; }
        //}

        private VisionType mVisionType;
        public VisionType VisionType
        {
            get { return mVisionType; }
            set
            {
                if (value != mVisionType)
                    mVisionType = value;
            }
        }

        private bool fakeEyes = false;
        [XmlIgnore]
        public VisionType AugmentedVisionType
        {
            get
            {
                if (fakeEyes)
                    return VisionType.Normal;
                else return mVisionType;
            }
        }

        private int mArmorBonus;
        public int ArmorBonus
        {
            get { return mArmorBonus; }
            set { mArmorBonus = value; }
        }

        private bool fakeSkin = false;
        [XmlIgnore]
        public int AugmentedArmorBonus
        {
            get
            {
                if (fakeSkin)
                    return 0;
                else return mArmorBonus;
            }
        }


        public SerializableDictionary<Priority, int> SpecialPoints { get; set; }

        public MetatypeStats()
        {
            SpecialPoints = new SerializableDictionary<Priority, int>();
        }

        public void SaveToFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(MetatypeStats));
            StreamWriter writer = new StreamWriter(filename);

            ser.Serialize(writer, this);

            writer.Close();
        }

        public static MetatypeStats LoadFromFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(MetatypeStats));
            StreamReader reader = new StreamReader(filename);

            MetatypeStats value = (MetatypeStats)ser.Deserialize(reader);

            reader.Close();



            return value;
        }

        #region IAugmentable

        [XmlIgnore]
        public System.Collections.ObjectModel.ObservableCollection<Augment> Augments
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void OnAugmentChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnAugmentCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion // IAugmentable
    }
}