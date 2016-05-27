using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressionEvaluator;

namespace SR5Builder.DataModels.Traits
{
    public class Quality : LeveledTrait, IKarmaCost
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
                    + mOwner.Settings.InPlayQualityMult * (mKarmaPerRating * ImprovedRating - BaseRating);
            }
        }

        public bool IsValid
        {
            get { return isValidDelegate(scope); }
        }

        public Quality(SR5Character owner, string name)
            : base(owner)
        {
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
    }
}
