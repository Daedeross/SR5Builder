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
            return base.NewSkillGroup(owner);
        }

    }
}
