using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SR5Builder.DataModels;

namespace SR5Builder.Prototypes.Loaders
{
    public class SkillGroupLoader: SkillGroupPrototype
    {
        public int Base { get; set; }
        public int Improved { get; set; }
        public SkillGroup ToSkillGroup(SR5Character owner)
        {
            var sg = base.NewSkillGroup(owner);
            sg.BaseRating = Base;
            sg.ImprovedRating = Improved;
            return sg;
        }

        public SkillGroupLoader()
        { }

        public SkillGroupLoader(SkillGroup sg)
        {
            Name = sg.Name;
            Base = sg.BaseRating;
            Improved = sg.ImprovedRating;
            SkillNames = sg.Skills.Keys.ToList();
        }
    }
}
