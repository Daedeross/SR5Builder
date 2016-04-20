using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public class SkillGroup : LeveledTrait
    {
        #region Properties

        public override SR5Character Owner
        {
            get { return mOwner; }
            set
            {
                if (mOwner != value)
                {
                    mOwner = value;
                    foreach (KeyValuePair<string, Skill> kvp in Skills)
                    {
                        kvp.Value.Owner = mOwner;
                    }
                    OnPropertyChanged("Owner");
                    OnPropertyChanged("TotalPool");
                }
            }
        }

        public Dictionary<string, Skill> Skills { get; set; }

        private int mRating;
        public int Rating
        {
            get { return mRating; }
            set
            {
                if (mRating != value)
                {
                    mRating = value;
                    foreach (KeyValuePair<string, Skill> kvp in Skills)
                    {
                        kvp.Value.BaseRating = mRating;
                        OnPropertyChanged("Rating");
                    }
                }
            }
        }

        public override int Karma
        {
            get { return 0; }
            set { }
        }

        #endregion // Properties

        public SkillGroup(SR5Character c)
            :base(c)
        {

        }
    }
}
