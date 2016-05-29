using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Prototypes
{
    public class SkillPrototype: IComparable<SkillPrototype>
    {
        public string Name { get; set; }

        public string LinkedAttribute { get; set; }

        public string Limit { get; set; }

        public string GroupName { get; set; }

        public SkillType Kind { get; set; }

        public Skill ToSkill(SR5Character owner)
        {
            Skill skill = new Skill(owner);

            skill.Name = Name;
            skill.Kind = Kind;
            skill.Min = 1;
            skill.BaseRating = 1;
            skill.GroupName = GroupName;
            skill.LinkedAttribute = LinkedAttribute;
            skill.UsualLimit = Limit;

            return skill;
        }

        public int CompareTo(SkillPrototype other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
