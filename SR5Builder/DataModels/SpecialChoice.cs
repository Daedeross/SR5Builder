using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SR5Builder.DataModels
{


    public class SpecialChoice : DataModelBase
    {
        public Priority Priority { get;  set; }

        // Name is in Base class

        public SpecialKind Kind { get;  set; }

        public string Description { get;  set; }

        public int Attribute { get;  set; }

        public string SkillGroupType { get;  set; }

        public int SkillGroupRating { get;  set; }

        public int SkillGroupCount { get;  set; }

        public string SkillType { get;  set; }

        public int SkillRating { get;  set; }

        public int SkillCount { get;  set; }

        public int Spells { get;  set; }

        public int ComplexForms { get;  set; }

        #region Factory Methods

        public static SpecialChoice None(Priority priority)
        {
            SpecialChoice sc = new SpecialChoice();
            sc.Priority = priority;
            sc.mName = "None";
            sc.Kind = SpecialKind.None;
            sc.Description = "Nothing to see here...";
            sc.SkillType = "";
            return sc;
        }

        #endregion // Factory Methods

        #region Serialization
        public void SaveToFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(SpecialChoice));
            StreamWriter writer = new StreamWriter(filename);

            ser.Serialize(writer, this);

            writer.Close();
        }

        //public static SpecialChoice LoadFromFile(string filename)
        //{
        //    XmlSerializer ser = new XmlSerializer(typeof(SpecialChoice));
        //    StreamReader reader = new StreamReader(filename);

        //    SpecialChoice value = (SpecialChoice)ser.Deserialize(reader);

        //    reader.Close();

        //    return value;
        //}

        public static List<SpecialChoice> LoadFromFile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<SpecialChoice>));
            StreamReader reader = new StreamReader(filename);

            List<SpecialChoice> value = (List<SpecialChoice>)ser.Deserialize(reader);

            reader.Close();

            return value;
        }
        #endregion

        #region Test Methods

        //public static void CreateFile()
        //{
        //    SpecialChoice magicianA = new SpecialChoice();
        //    magicianA.Priority = Priority.A;
        //    magicianA.Kind = SpecialType.Magician;
        //    magicianA.Magic = 6;
        //    magicianA.SkillCount = 2;
        //    magicianA.SkillGroupCount = 0;
        //    magicianA.SkillRating = 5;
        //    magicianA.SkillType = "Magical";
        //    magicianA.Spells = 10;

        //    SpecialChoice magicianB = new SpecialChoice();
        //    magicianB.Priority = Priority.B;
        //    magicianB.Kind = SpecialType.Magician;
        //    magicianB.Magic = 4;
        //    magicianB.SkillCount = 2;
        //    magicianB.SkillGroupCount = 0;
        //    magicianB.SkillRating = 4;
        //    magicianB.SkillType = "Magical";
        //    magicianB.Spells = 7;

        //    List<SpecialChoice> list = new List<SpecialChoice>(2);
        //    list.Add(magicianA);
        //    list.Add(magicianB);

        //    XmlSerializer ser = new XmlSerializer(typeof(List<SpecialChoice>));
        //    StreamWriter writer = new StreamWriter("Magician.xml");

        //    ser.Serialize(writer, list);

        //    writer.Close();
        //}

        #endregion
    }
}