using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Loaders
{
    public class SkillLoader: IComparable<SkillLoader>
    {
        public string Name { get; set; }

        public string LinkedAttribute { get; set; }

        public string Limit { get; set; }

        public string GroupName { get; set; }

        public SkillType Kind { get; set; }

        public Skill NewSkill(SR5Character owner)
        {
            Skill skill = new Skill(owner);

            skill.Name = Name;
            skill.Kind = Kind;
            skill.BaseRating = 1;
            skill.GroupName = GroupName;
            skill.LinkedAttribute = LinkedAttribute;
            skill.UsualLimit = Limit;

            return skill;
        }

        public int CompareTo(SkillLoader other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
