using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Specialized;
using System.Xml.Serialization;

using SR5Builder.Loaders;

using DrWPF.Windows.Data;

namespace SR5Builder.DataModels
{
    public class Augment: DataModelBase
    {
        #region Private Fields

        private string targetName;
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

                    OnPropertyChanged("Target");
                }
            }
        }

        public AugmentKind Kind { get { return kind; } }

        public float[] BonusArray { get; set; }

        //protected int mBonus;
        [XmlIgnore]
        public float Bonus
        {
            get
            {
                if (mOwnerTrait.BaseRating > 0 && mOwnerTrait.BaseRating < BonusArray.Length)
                {
                    return BonusArray[mOwnerTrait.BaseRating];
                }
                else return 0;
            }
        }

        #endregion // Properties

        #region Constructors

        public Augment(IAugmentContainer owner, AugmentLoader loader)
        {
            OwnerTrait = owner;
            targetName = loader.Target;
            kind = loader.Kind;
            BonusArray = loader.Bonus.ToArray();

            mOwnerTrait.Owner.Augmentables.CollectionChanged += this.OnAugmentablesChanged;

            SetTarget(targetName);
        }

        public Augment(IAugmentContainer owner, AugmentLoader loader, string targetName)
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
            OnPropertyChanged("Bonus");
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


        public static string[] GetAugmentableNames()
        {
            return (from T in Assembly.GetAssembly(typeof(IAugmentable)).GetTypes()
                    where T.IsAssignableFrom(typeof(IAugmentable))
                    select T.Name).ToArray();
        }

        #endregion // Public Methods
    }
}
