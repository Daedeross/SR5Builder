using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Prototypes.Loaders
{
    public class SkillLoader: SkillPrototype
    {
        public int Base { get; set; }
        public int Improved { get; set; }
        public string Ext { get; set; }

        public Skill ToSkill(SR5Character character)
        {
            Skill skill = base.ToSkill(character, Ext);
            skill.BaseRating = Base;
            skill.ImprovedRating = Improved;
            return skill;
        }

        public SkillLoader()
        { }

        public SkillLoader(Skill s)
        {
            Name = s.Name;
            Base = s.BaseRating;
            GroupName = s.GroupName;
            Improved = s.ImprovedRating;
            Kind = s.Kind;
            Limit = s.UsualLimit;
            LinkedAttribute = s.LinkedAttribute;
        }
    }
}
