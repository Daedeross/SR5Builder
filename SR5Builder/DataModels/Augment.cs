using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Specialized;
using System.Xml.Serialization;

using SR5Builder.Prototypes;

using DrWPF.Windows.Data;

namespace SR5Builder.DataModels
{
    /// <summary>
    /// The class that represents all ways a trait can modify another trait.
    /// </summary>
    /// <remarks>
    /// Each target trait and property must be a sepparate Augment. Example:
    /// The <b>Improved Reflexes</b> adept power gives +1 to Reaction and +1 to
    /// Physical Initiative Dice per level. That means that <b>Improved Reflexes</b>
    /// needs two augmetns, one for the reaction bonus, and one for the extra dice.
    /// 
    /// It is up to the targeted trait (which must implement <see cref="IAugmentable"/>)
    /// how to handle the Augment.
    /// </remarks>
    public class Augment: DataModelBase
    {
        #region Private Fields

        private string targetName;  // The name of the target trait.
        private AugmentKind kind;

        #endregion // Private fields

        #region Properties

        private IAugmentContainer mOwnerTrait;
        public IAugmentContainer OwnerTrait
        {
            get { return mOwnerTrait; }
            set
            {
                if (value != mOwnerTrait)
                {
                    if (mOwnerTrait != null)
                    {
                        mOwnerTrait.PropertyChanged -= this.OnTraitChanged;
                    }

                    mOwnerTrait = value;

                    mOwnerTrait.PropertyChanged += this.OnTraitChanged;

                }
            }
        }

        private IAugmentable mTarget;
        public IAugmentable Target
        {
            get { return mTarget; }
            set
            {
                if (value != mTarget)
                {
                    if (mTarget != null)
                        mTarget.Augments.Remove(this);

                    mTarget = value;

                    if (mTarget != null)
                        mTarget.Augments.Add(this);

                    OnPropertyChanged(nameof(Target));
                }
            }
        }

        /// <summary>
        /// See <see cref="AugmentKind"/>. Essentialy determines what property is modified.
        /// </summary>
        public AugmentKind Kind { get { return kind; } }

        /// <summary>
        /// Array indexed by the owner's Rating to determin the  bonus to give.
        /// If owner does not have a rating or it is out of range for the array,
        /// Index zero [0] is used.
        /// </summary>
        public decimal[] BonusArray { get; set; }

        //protected int mBonus;
        [XmlIgnore]
        public decimal Bonus
        {
            get
            {
                if (mOwnerTrait.BaseRating > 0 && mOwnerTrait.BaseRating < BonusArray.Length)
                {
                    return BonusArray[mOwnerTrait.BaseRating];
                }
                else return BonusArray[0];
            }
        }

        /// <summary>
        /// The name of the Trait that this Augment modifies.
        /// </summary>
        public string TargetName {  get { return targetName; } }

        #endregion // Properties

        #region Constructors

        public Augment(IAugmentContainer owner, AugmentPrototype loader)
        {
            OwnerTrait = owner;
            targetName = loader.Target;
            kind = loader.Kind;
            BonusArray = loader.Bonus.ToArray();

            mOwnerTrait.Owner.Augmentables.CollectionChanged += this.OnAugmentablesChanged;

            SetTarget(targetName);
        }

        public Augment(IAugmentContainer owner, AugmentPrototype loader, string targetName)
        {
            OwnerTrait = owner;
            this.targetName = targetName;
            kind = loader.Kind;
            BonusArray = loader.Bonus.ToArray();

            mOwnerTrait.Owner.Augmentables.CollectionChanged += this.OnAugmentablesChanged;

            SetTarget(targetName);
        }

        #endregion // Constructors

        #region Private Methods

        private void SetTarget(string targetName)
        {
            
            IAugmentable a;
            mOwnerTrait.Owner.Augmentables.TryGetValue(targetName, out a);
            Target = a;
        }

        private void OnTraitChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Bonus));
        }

        // Keeps watch on the Characters Augmentable Collection and
        // updates Target reference if necccessary
        private void OnAugmentablesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
                foreach (KeyValuePair<string, IAugmentable> kvp in e.OldItems)
                    if (kvp.Value == Target)
                    {
                        Target = null;
                        break;
                    }

            if (e.NewItems != null)
                 foreach (KeyValuePair<string, IAugmentable> kvp in e.NewItems)
                     if (kvp.Key == targetName)
                     {
                         Target = kvp.Value;
                         break;
                     }
        }

        #endregion // Private Methods

        #region Public Methods

        public override void Dispose()
        {
            Target?.OnAugmentRemoving(kind);
            Target = null;
            base.Dispose();
        }

        /// <summary>
        /// Returns an array of Type objects which implement IAugmentable
        /// </summary>
        /// <returns>Array of Type objects</returns>
        public static Type[] GetAugmentables()
        {
            return (from T in Assembly.GetAssembly(typeof(IAugmentable)).GetTypes()
                    where T.IsAssignableFrom(typeof(IAugmentable))
                    select T).ToArray();
        }

        /// <summary>
        /// Returs an array of type names of classes which implement IAugmentable.
        /// </summary>
        /// <returns>Array of strings</returns>
        public static string[] GetAugmentableNames()
        {
            return (from T in Assembly.GetAssembly(typeof(IAugmentable)).GetTypes()
                    where T.IsAssignableFrom(typeof(IAugmentable))
                    select T.Name).ToArray();
        }

        #endregion // Public Methods
    }
}
