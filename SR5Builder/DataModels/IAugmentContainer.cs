using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    public interface IAugmentContainer: INotifyPropertyChanged
    {
        SR5Character Owner { get; set; }

        ObservableCollection<Augment> GivenAugments { get; set; }

        int BaseRating { get; set; }

        void ClearAugments();
    }
}
