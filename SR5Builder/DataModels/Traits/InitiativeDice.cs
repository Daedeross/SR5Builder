using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public class InitiativeDice : LeveledTrait
    {
        public override int Karma
        {
            get
            { return 0; }
            set { }
        }

        public override string DisplayValue
        {
            get
            {
                return base.DisplayValue + "d6";
            }
        }

        public InitiativeDice(SR5Character owner, string name, int baseRating)
        {
            mOwner = owner;
            mName = name;
            mBaseRating = baseRating;
        }
    }
}
