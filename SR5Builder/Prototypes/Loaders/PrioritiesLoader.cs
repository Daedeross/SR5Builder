using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SR5Builder.DataModels;

namespace SR5Builder.Prototypes.Loaders
{
    public class PrioritiesLoader
    {
        public Priority Metatype { get; set; }
        public Priority Attributes { get; set; }
        public Priority Special { get; set; }
        public Priority Skills { get; set; }
        public Priority Resources { get; set; }
    }
}
