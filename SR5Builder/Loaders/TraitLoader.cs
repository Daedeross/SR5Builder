using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Loaders
{
    public abstract class TraitLoader: IComparable<TraitLoader>
    {
        public string Name { get; set; }

        public string Book { get; set; }

        public int Page { get; set; }

        public string Category { get; set; }

        public int CompareTo(TraitLoader other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
