using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ExpressionEvaluator;

namespace SR5Builder.DataModels
{
    public class Quality : LeveledTrait, IKarmaCost, IAugmentContainer
    {
        public struct ValidScope
        {
            public SR5Character Character;
            public Quality This;
        }

        private int mKarmaPerRating;
        public int Karma
        {
            get
            {
                return mKarmaPerRating * BaseRating
                    + mOwner.Settings.InPlayQualityMult * (mKarmaPerRating * (ImprovedRating - BaseRating));
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

        public Quality(SR5Character owner, int karma)
            : base(owner)
        {
            mKarmaPerRating = karma;
            scope = new ValidScope() { Character = owner, This = this };
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
        }

        public void ClearAugments()
        {
            foreach (Augment a in GivenAugments)
            {
                a.Target = null;
            }
        }
    }
}
