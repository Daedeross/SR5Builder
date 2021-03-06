﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.Prototypes
{
    public abstract class TraitPrototype: IComparable<TraitPrototype>
    {
        public string Name { get; set; }

        public string Book { get; set; }

        public int Page { get; set; }

        public string Category { get; set; }

        public int CompareTo(TraitPrototype other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}
