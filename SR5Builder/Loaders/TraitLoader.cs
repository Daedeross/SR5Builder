using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Loaders
{
    public abstract class TraitLoader
    {
        public string Name { get; set; }

        public string Book { get; set; }

        public int Page { get; set; }

        public string Category { get; set; }
    }
}
