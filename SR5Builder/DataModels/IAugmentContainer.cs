using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SR5Builder.DataModels
{
    /// <summary>
    /// Interface for can Trait which can hold Augments.
    /// </summary>
    public interface IAugmentContainer: INotifyPropertyChanged
    {
        string Name { get; set; }

        /// <summary>
        /// The character instance that owns the traits.
        /// </summary>
        /// <remarks>
        /// Required so the Augmetns can find their target.
        /// </remarks>
        SR5Character Owner { get; set; }

        /// <summary>
        /// The Augments that the trait "owns."
        /// </summary>
        ObservableCollection<Augment> GivenAugments { get; set; }

        /// <summary>
        /// Some augments scale based on their owner's Rating, specifically BaseRating
        /// </summary>
        int BaseRating { get; set; }

        /// <summary>
        /// Used before trait is deleted.
        /// </summary>
        void ClearAugments();
    }
}
