using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public class InitiativeDice : LeveledTrait
    {
        public override string DisplayValue
        {
            get
            {
                return ToString();
            }
        }

        public override string ToString()
        {
            if (ImprovedRating != AugmentedRating)
            {
                return String.Format("{0}({1})d6", ImprovedRating, AugmentedRating);
            }
            else return string.Format("{0}d6", ImprovedRating);
        }

        public InitiativeDice(SR5Character owner, string name, int baseRating)
        {
            mOwner = owner;
            mName = name;
            mBaseRating = baseRating;
        }
    }
}
