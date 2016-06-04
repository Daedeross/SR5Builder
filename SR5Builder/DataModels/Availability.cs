using System;
using System.Xml.Serialization;

namespace SR5Builder.DataModels
{
    public enum Restriction
    {
        N = 0,
        R = 1,
        F = 2,
    }

    [XmlRoot("BaseAvailability")]
    public struct Availability: IEquatable<Availability>,
                                IComparable<Availability>,
                                IComparable,
                                IXmlSerializable
    {
        public Restriction Restriction;

        public int Level;

        public override string ToString()
        {
            if (Level== 0)
                return "—";

            if (Restriction == Restriction.N)
                return Level.ToString();
            else return Level.ToString() + Restriction.ToString();
        }

        public Availability(string str)
        {
            if (str.Length == 0)
            {
                Restriction = Restriction.N;
                Level = 0;
            }
            else if (str.EndsWith("R"))
            {
                Restriction = Restriction.R;
                Level = Convert.ToInt32(str.Replace("R", ""));
            }
            else if (str.EndsWith("F"))
            {
                Restriction = Restriction.F;
                Level = Convert.ToInt32(str.Replace("F", ""));
            }
            else
            {
                Restriction = Restriction.N;
                Level = Convert.ToInt32(str.Replace("N", ""));
            }
        }


        public static Availability Add(Availability a1, Availability a2)
        {
            Availability a = new Availability();
            a.Level = a1.Level + a2.Level;
            a.Restriction = (Restriction)Math.Max((int)a1.Restriction, (int)a2.Restriction);
            return a;
        }

        public static Availability operator +(Availability a1, Availability a2)
        {
            return Add(a1, a2);
        }

        public static Availability Zero
        {
            get
            {
                Availability a = new Availability();
                a.Level = 0;
                a.Restriction = Restriction.N;
                return a;
            }
        }

        #region Interface Implementations

            #region IEquatable<BaseAvailability>

        public bool Equals(Availability other)
        {
            return (other.Level == this.Level) && (other.Restriction == this.Restriction);
        }

            #endregion // IEquatable<BaseAvailability>

            #region IComparable<BaseAvailability>

        public int CompareTo(Availability other)
        {
            return this.Level.CompareTo(other.Level);
        }

            #endregion // IComparable<BaseAvailability>

            #region IComparable

        public int CompareTo(object obj)
        {
            if (obj is Availability)
            {
                return this.CompareTo((Availability)obj);
            }
            else return 0;
        }

            #endregion // IComparable

            #region IXmlSerializable

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            string temp = reader.ReadElementContentAsString();
            if (temp.Length == 0)
            {
                Restriction = Restriction.N;
                Level = 0;
            }
            else if (temp.EndsWith("R"))
            {
                Restriction = Restriction.R;
                Level = Convert.ToInt32(temp.Replace("R", ""));
            }
            else if (temp.EndsWith("F"))
            {
                Restriction = Restriction.F;
                Level = Convert.ToInt32(temp.Replace("F", ""));
            }
            else
            {
                Restriction = Restriction.N;
                Level = Convert.ToInt32(temp.Replace("N", ""));
            }
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteString(this.ToString());
        }

            #endregion // IXmlSerializable

        #endregion
    }
}