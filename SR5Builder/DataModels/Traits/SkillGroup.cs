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
                        OnPropertyChanged(nameof(kvp.Value.Owner));
                        OnPropertyChanged(nameof(kvp.Value.TotalPool));
                    }
                }
            }
        }

        public Dictionary<string, Skill> Skills { get; set; }
        
        public override int BaseRating
        {
            get { return mBaseRating; }
            set
            {
                if (mBaseRating != value)
                {
                    mBaseRating = value;
                    foreach (KeyValuePair<string, Skill> kvp in Skills)
                    {
                        kvp.Value.BaseRating = mBaseRating;
                        OnPropertyChanged(nameof(BaseRating));
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
