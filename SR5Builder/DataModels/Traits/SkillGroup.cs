using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public class SkillGroup : LeveledTrait, IKarmaCost
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
                        RaisePropertyChanged(nameof(kvp.Value.Owner));
                        RaisePropertyChanged(nameof(kvp.Value.TotalPool));
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
                        RaisePropertyChanged(nameof(BaseRating));
                    }
                }
            }
        }

        public int Karma
        {
            get { return mOwner.Settings.SkillGroupKarma(ImprovedRating, BaseRating); }
        }

        #endregion // Properties

        public SkillGroup(SR5Character c)
            :base(c)
        {

        }
    }
}
