using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Prototypes
{
    public class SkillPrototype: TraitExtPrototype, IComparable<SkillPrototype>
    {
        //public string Name { get; set; }

        public string LinkedAttribute { get; set; }

        public string Limit { get; set; }

        public string GroupName { get; set; }

        public SkillType Kind { get; set; }

        public virtual Skill ToSkill(SR5Character owner, string ext = "")
        {
            CheckExt(ext);

            Skill skill = new Skill(owner)
            {
                Name = string.Format(Name, ext),
                Kind = Kind,
                Min = 1,
                BaseRating = 1,
                GroupName = GroupName,
                LinkedAttribute = LinkedAttribute,
                UsualLimit = Limit
            };
            return skill;
        }

        public int CompareTo(SkillPrototype other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
