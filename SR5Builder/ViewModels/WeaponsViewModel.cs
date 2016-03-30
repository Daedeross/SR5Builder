using DrWPF.Windows.Data;
using SR5Builder.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace SR5Builder.ViewModels
{
    public class WeaponsViewModel: ViewModelBase
    {
        #region Private Fields

        SR5Character character;

        #endregion // Private fields

        #region Properties

            #region Weapons

        public ObservableDictionary<string, MeleeWeapon> MeleeWeapons { get; set; }

        public ObservableDictionary<string, RangedWeapon> RangedWeapons { get; set; }

            #endregion // Weapons

        #endregion // Properties

        #region Constructors

        public WeaponsViewModel(SR5Character character)
        {
            this.character = character;

            MeleeWeapons = new ObservableDictionary<string, MeleeWeapon>();
            RangedWeapons = new ObservableDictionary<string, RangedWeapon>();

            character.GearList.CollectionChanged += this.OnGearChanged;
        }

        #endregion // Constructors

        #region Commands

        #endregion // Commands

        #region Private Methods

        private void OnGearChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (KeyValuePair<string, Gear> kvp in e.OldItems)
                {
                    if (kvp.Value is MeleeWeapon)
                        MeleeWeapons.Remove(kvp.Key);
                    else if (kvp.Value is RangedWeapon)
                        RangedWeapons.Remove(kvp.Key);
                }
            }

            if (e.NewItems != null)
            {
                System.Diagnostics.Debug.WriteLine(e.NewItems[0].GetType());
                foreach (KeyValuePair<string, Gear> kvp in e.NewItems)
                {
                    System.Diagnostics.Debug.WriteLine(kvp.Value.GetType());
                    if (kvp.Value is MeleeWeapon)
                        MeleeWeapons.Add(kvp.Key, (MeleeWeapon)kvp.Value);
                    else if (kvp.Value is RangedWeapon)
                        RangedWeapons.Add(kvp.Key, (RangedWeapon)kvp.Value);
                }
            }
        }

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}
