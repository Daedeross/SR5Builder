﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SR5Builder.DataModels;

namespace SR5Builder.Loaders
{
    public class AugmentLoader
    {
        public AugmentKind Kind { get; set; }

        public string Target { get; set; }

        public float[] Bonus { get; set; }

        public Augment ToAugment(IAugmentContainer owner)
        {
            return new Augment(owner, this);
        }

        public Augment ToAugment(IAugmentContainer owner, string targetName)
        {
            return new Augment(owner, this, targetName);
        }
    }
}