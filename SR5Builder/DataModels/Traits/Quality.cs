using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ExpressionEvaluator;
using System.ComponentModel;
using SR5Builder.Helpers;

namespace SR5Builder.DataModels
{
    public class Quality : LeveledTrait, IKarmaCost, IAugmentContainer
    {
        private Dictionary<INotifyPropertyChanged, HashSet<string>> watched;

        public struct ValidScope
        {
            public SR5Character Character;
            public Quality This;
        }

        public int[] KarmaArray { get; set; }

        private int mKarmaPerRating;
        public int Karma
        {
            get
            {
                if (KarmaArray == null || KarmaArray.Length == 0)
                {
                    return mKarmaPerRating * BaseRating
                                + mOwner.Settings.InPlayQualityMult * (mKarmaPerRating * (ImprovedRating - BaseRating)); 
                }
                else
                {
                    int baseKarma;
                    if (BaseRating < KarmaArray.Length)
                    {
                        baseKarma = KarmaArray[BaseRating];
                    }
                    else
                    {
                        baseKarma = 0;
                        Log.LogMessage($"Warning: Index out of bounds for {Name}: BaseRating = {BaseRating}");
                    }
                    int extraKarma;
                    if (ImprovedRating < KarmaArray.Length)
                    {
                        extraKarma = KarmaArray[ImprovedRating];
                    }
                    else
                    {
                        extraKarma = 0;
                        Log.LogMessage($"Warning: Index out of bounds for {Name}: ImprovedRating = {ImprovedRating}");
                    }
                    return baseKarma + mOwner.Settings.InPlayQualityMult * extraKarma;
                }
            }
        }

        public override int BaseRating
        {
            get
            {
                return base.BaseRating;
            }

            set
            {
                base.BaseRating = value;
                OnPropertyChanged(nameof(Karma));
            }
        }

        public bool IsValid
        {
            get { return isValidDelegate(scope); }
        }

        public ObservableCollection<Augment> GivenAugments { get; set; }
            = new ObservableCollection<Augment>();
        
        public override string DisplayValue
        {
            get
            {
                int i = AugmentedRating;
                if (   LevelNames != null
                    && Max > 1
                    && i >= 0
                    && i < LevelNames.Length)
                {
                    return $" [{LevelNames[i]}]";
                }
                else
                {
                    return base.DisplayValue;
                }
            }
        }

        public Quality(SR5Character owner, int karma, string validExpression)
            : base(owner)
        {
            mKarmaPerRating = karma;
            scope = new ValidScope() { Character = owner, This = this };
            isValidExprText = validExpression;
            CompileExpression();
        }

        private ValidScope scope;

        private string isValidExprText;

        private Func<ValidScope, bool> isValidDelegate;

        private CompiledExpression<bool> isValidExpr;

        private void CompileExpression()
        {
            if (isValidExprText != null)
            {
                isValidExpr = new CompiledExpression<bool>(isValidExprText);
                isValidDelegate = isValidExpr.ScopeCompile<ValidScope>();
            }
            else
                isValidDelegate =  (s) => true;

            SetWatched();
        }

        private void SetWatched()
        {
            if (watched != null)
            {
                foreach (var key in watched.Keys)
                {
                    key.PropertyChanged -= this.OnWatchedChanged;
                }
            }

            watched = ExpressionUtils.FindMembers(isValidExpr, scope);
            foreach (var key in watched.Keys)
            {
                key.PropertyChanged += this.OnWatchedChanged;
            }
        }

        public void ClearAugments()
        {
            foreach (Augment a in GivenAugments)
            {
                a.Target = null;
            }
        }

        private void OnWatchedChanged(object sender, PropertyChangedEventArgs e)
        {
            HashSet<string> names;
            INotifyPropertyChanged w = sender as INotifyPropertyChanged;
            if (w != null && watched.TryGetValue(w, out names))
            {
                if (names.Contains(e.PropertyName))
                {
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public void Cleanup()
        {
            foreach (var key in watched.Keys)
            {
                key.PropertyChanged -= this.OnWatchedChanged;
            }
            ClearAugments();
        }

        public override void Dispose()
        {
            base.Dispose();

            Cleanup();
        }
    }
}
