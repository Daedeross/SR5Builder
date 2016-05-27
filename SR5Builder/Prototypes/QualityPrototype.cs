using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressionEvaluator;

namespace SR5Builder.Prototypes
{
    public class QualityPrototype: TraitPrototype
    {
        public string PrereqExpression { get; set; }

        public bool PrereqMet(DataModels.SR5Character character)
        {
            return PrereqDelegate(character);
        }

        private Func<DataModels.SR5Character, bool> PrereqDelegate;

        private CompiledExpression<bool> PrereqCE;


    }
}
