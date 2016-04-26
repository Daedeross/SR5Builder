using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels.Traits
{
    public class PowerPoints : LeveledTrait
    {
        public override int Max
        {
            get
            {
                if (mOwner.SpecialChoice != null)
                {  return mOwner.SpecialChoice.PowerPoints; }
                else return 0;
            }
            set { }
        }

        public override int Karma
        {
            get
            {
                return (BaseRating - Min) * mOwner.Settings.PowerPointKarma;
            }
            set { }
        }
    }
}
