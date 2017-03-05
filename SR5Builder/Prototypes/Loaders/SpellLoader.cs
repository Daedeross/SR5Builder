using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR5Builder.Prototypes.Loaders
{
    public class SpellLoader: SpellPrototype
    {
        public bool IsFree { get; set; }
        public string Ext { get; set; }

        public Spell ToSpell(SR5Character owner)
        {
            return base.ToSpell(owner, IsFree, Ext);
        }

        public SpellLoader()
        { }

        public SpellLoader(Spell s)
            :base (s)
        {
            IsFree = s.Free;
            Ext = "";
        }
    }
}
