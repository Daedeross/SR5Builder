using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public interface IEssenceCost: INotifyPropertyChanged
    {
        decimal TotalEssence { get; }
    }
}
