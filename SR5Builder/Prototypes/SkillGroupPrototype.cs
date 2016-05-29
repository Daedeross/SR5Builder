using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.DataModels;

namespace SR5Builder.Prototypes
{
    public class SkillGroupPrototype: IComparable<SkillGroupPrototype>
    {
        public string Name { get; set; }

        public List<string> SkillNames { get; set; }

        public SkillGroupPrototype()
        {
            Name = "";
            SkillNames = new List<string>();
        }

        public SkillGroup NewSkillGroup(SR5Character character)
        {
            SkillGroup grp = new SkillGroup(character);
            grp.Name = this.Name;
            grp.Skills = (from skill in GlobalData.PreLoadedSkills["All"]
                          where skill.GroupName == this.Name
                          select skill.ToSkill(character)).ToDictionary(skill => skill.Name);
            grp.BaseRating = 1;

            return grp;
        }

        public int CompareTo(SkillGroupPrototype other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
