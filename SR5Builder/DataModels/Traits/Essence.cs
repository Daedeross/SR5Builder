using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public class Essence : Attribute
    {
        public override int Karma
        {
            get
            {
                return 0;
            }
            set { }
        }

        public override int Min
        {
            get
            {
                return 0;
            }

            set { }
        }

        public override int Max
        {
            get
            {
                return 6;
            }
            set { }
        }

        public Essence(SR5Character owner, string name)
            : base(owner, name)
        {
            mBaseRating = 6;
        }
    }
}
