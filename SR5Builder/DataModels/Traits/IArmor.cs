﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public interface IArmor
    {
        int ArmorRating { get; }

        bool IsClothing { get; }
    }
}
