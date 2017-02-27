using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Loaders
{
    public class SkillLoader: LeveledTraitLoader
    {
        public string LinkedAttribute { get; set; }
        public string Limit { get; set; }
        public string GroupName { get; set; }
        public SkillType Kind { get; set; }

        public Skill ToSkill(SR5Character character)
        {
            Skill skill = new Skill(character)
            {
                Name = Name,
                Kind = Kind,
                Min = 1,
                BaseRating = Base,
                ImprovedRating = Improved,
                GroupName = GroupName,
                LinkedAttribute = LinkedAttribute,
                UsualLimit = Limit
            };
            return skill;
        }
    }
}
