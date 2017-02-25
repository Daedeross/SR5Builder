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
            Skill skill = new Skill(character);

            skill.Name = Name;
            skill.Kind = Kind;
            skill.Min = 1;
            skill.BaseRating = Base;
            skill.ImprovedRating = Improved;
            skill.GroupName = GroupName;
            skill.LinkedAttribute = LinkedAttribute;
            skill.UsualLimit = Limit;

            return skill;
        }
    }
}
