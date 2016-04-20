using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SR5Builder.DataModels
{
    public abstract class BaseTrait : DataModelBase, IComparable<BaseTrait>
    {
        public string Book { get; set; }

        public int Page { get; set; }

        public string Category { get; set; }

        protected SR5Character mOwner;
        [XmlIgnore]
        public virtual SR5Character Owner
        {
            get { return mOwner; }
            set
            {
                if (mOwner != value)
                {
                    mOwner = value;
                }
            }
        }

        [XmlIgnore]
        public virtual int Points
        {
            get;
            set;
        }

        public abstract int Karma { get; set; }

        public BaseTrait(SR5Character c)
        {
            mOwner = c;
        }

        public BaseTrait()
        {
            mOwner = null;
        }

        public int CompareTo(BaseTrait other)
        {
            return this.mName.CompareTo(other.mName);
        }
    }
}