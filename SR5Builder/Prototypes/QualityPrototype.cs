using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ExpressionEvaluator;
using SR5Builder.Helpers;

namespace SR5Builder.Prototypes
{
    public class QualityPrototype: TraitPrototype
    {
        public int Max { get; set; }

        public int Karma { get; set; }
        
        public string PrereqExpression { get; set; }

        public bool PrereqMet(DataModels.SR5Character character)
        {
            if (PrereqDelegate == null)
            {
                if (PrereqExpression != null)
                {
                    PrereqCE = new CompiledExpression<bool>(PrereqExpression);
                    PrereqDelegate = PrereqCE.ScopeCompile<DataModels.SR5Character>();
                }
                else
                {
                    PrereqDelegate = c => true;
                }
            }
            return PrereqDelegate(character);
        }

        private Func<DataModels.SR5Character, bool> PrereqDelegate;

        private CompiledExpression<bool> PrereqCE;

        public static List<QualityPrototype> LoadFromFile(string filename)
        {
            FileInfo fi = new FileInfo(filename);
            if (!fi.Exists)
            {
                Log.LogMessage("Unable to load {0}", filename);
            }

            using (StreamReader reader = new StreamReader(filename))
            {
                try
                {
                    XmlSerializer ser = new XmlSerializer(typeof(List<QualityPrototype>));
                    List<QualityPrototype> obj = (List<QualityPrototype>)ser.Deserialize(reader);
                    return obj;
                }
                catch (IOException e)
                {
                    Log.LogMessage(e.Message);
                    return new List<QualityPrototype>();
                }
            }
        }
    }
}
