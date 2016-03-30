using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SR5Builder.DataModels
{
    public interface IAugmentable: INotifyPropertyChanged
    {
        string Name { get; set; }

        ObservableCollection<Augment> Augments { get; set; }

        void OnAugmentChanged(object sender, PropertyChangedEventArgs e);

        void OnAugmentCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e);
    }
}
