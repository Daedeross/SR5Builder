using SR5Builder.Prototypes;
using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace SR5Builder.DataModels
{
    /// <summary>
    /// This class contains settings and customizations for character generation.
    /// Each SR5Character contains its own instance of it.
    /// </summary>
    public class GenSettings
    {
        public CharGenMethod Method { get; set; }

        public int StartingKarma { get; set; } = 25;

        public int MaxAugment { get; set; } = 4;

        #region Scaling Karma Costs

        /// <summary>
        /// For Karma advancement of Attributes.
        /// Karma cost is [New Rating] x AttributeKarmaMult (default 5 in base rules).
        /// </summary>
        public int AttributeKarmaMult { get; set; } = 5;

        /// <summary>
        /// For Karma advancement of Skill Groups.
        /// Karma cost is [New Rating] x SkillGroupKarmaMult (default 5 in base rules).
        /// </summary>
        public int SkillGroupKarmaMult { get; set; } = 5;

        /// <summary>
        /// For Karma advancement of Active Skills (including social).
        /// Karma cost is [New Rating] x ActiveSkillKarmaMult (default 2 in base rules).
        /// </summary>
        public int ActiveSkillKarmaMult { get; set; } = 2;

        /// <summary>
        /// For Karma advancement of Magic Skills.
        /// Karma cost is [New Rating] x MagicSkillKarmaMult (default 2 in base rules).
        /// </summary>
        public int MagicSkillKarmaMult { get; set; } = 2;

        /// <summary>
        /// For Karma advancement of Resonance Skills.
        /// Karma cost is [New Rating] x ResonanceSkillKarmaMult (default 2 in base rules).
        /// </summary>
        public int ResonanceSkillKarmaMult { get; set; } = 2;

        /// <summary>
        /// For Karma advancement of Knowledge Skills.
        /// Karma cost is [New Rating] x KnowledgeSkillKarmaMult (default 1 in base rules).
        /// </summary>
        public int KnowledgeSkillKarmaMult { get; set; } = 1;

        /// <summary>MagicSkill
        /// For Karma advancement of Language Skills (including social).
        /// Karma cost is [New Rating] x LanguageSkillKarmaMult (default 1 in base rules).
        /// </summary>
        public int LanguageSkillKarmaMult { get; set; } = 1;

        public int AttributeKarma(int value, int min = 1)
        {
            return AttributeKarmaMult * (ValueAt(value) - ValueAt(min));
        }

        public int SkillGroupKarma(int value, int min = 0)
        {
            return SkillGroupKarmaMult * (ValueAt(value) - ValueAt(min));
        }

        public int ActiveSkillKarma(int value, int min = 0)
        {
            return ActiveSkillKarmaMult * (ValueAt(value) - ValueAt(min));
        }

        public int MagicSkillKarma(int value, int min = 0)
        {
            return MagicSkillKarmaMult * (ValueAt(value) - ValueAt(min));
        }

        public int ResonanceSkillKarma(int value, int min = 0)
        {
            return ResonanceSkillKarmaMult * (ValueAt(value) - ValueAt(min));
        }

        public int KnowledgeSkillKarma(int value, int min = 0)
        {
            return KnowledgeSkillKarmaMult * (ValueAt(value) - ValueAt(min));
        }

        public int LanguageSkillKarma(int value, int min = 0)
        {
            return LanguageSkillKarmaMult * (ValueAt(value) - ValueAt(min));
        }

        public int InitiationKarmaBase { get; set; } = 10;

        public int InitiationKarmaMult { get; set; } = 3;

        public int SubmersionKarmaBase { get; set; } = 10;

        public int SubmersionKarmaMult { get; set; } = 3;

        /// <summary>
        /// Calulates the Karma cost to Initiate from <para>min</para> to <para>value</para>.
        /// </summary>
        /// <param name="value">The end Initiation level.</param>
        /// <param name="min">The base Initiation level (default 0).</param>
        /// <param name="discounts">The number of discounts to initiation (i.e. Group, Ordeal, and Schooling).</param>
        /// <returns>The total Karma cost to Initiate from <para>min</para> to <para>value</para>.</returns>
        public int InitiationKarma(int value, int min = 0, int discounts = 0)
        {
            int val = 0;
            discounts = Math.Min(discounts, 3);
            discounts = Math.Max(discounts, 0);
            for (int i = min + 1; i <= value; i++)
            {
                val += (int)Math.Ceiling((1 - 0.3 * discounts) * (InitiationKarmaBase + InitiationKarmaMult * i));
            }
            return val;
        }

        /// <summary>
        /// Calulates the Karma cost to Submerge from <para>min</para> to <para>value</para>.
        /// </summary>
        /// <param name="value">The end Submersion level.</param>
        /// <param name="min">The base Submersion level (default 0).</param>
        /// <param name="discounts">The number of discounts to initiation (i.e. Group, Ordeal, and Schooling).</param>
        /// <returns>The total Karma cost to Submerge from <para>min</para> to <para>value</para>.</returns>
        public int SubmersionKarma(int value, int min = 0, int discounts = 0)
        {
            int val = 0;
            discounts = Math.Min(discounts, 3);
            discounts = Math.Max(discounts, 0);
            for (int i = min + 1; i <= value; i++)
            {
                val += (int)Math.Ceiling((1 - 0.3 * discounts) * (SubmersionKarmaBase + SubmersionKarmaMult * i));
            }
            return val;
        }

        #endregion Scaling Karma Costs

        #region Flat Karma Costs

        public int ComplexFormKarma { get; set; } = 4;

        public int SpellKarma { get; set; } = 5;

        public int PowerPointKarma { get; set; } = 5;

        public int SpecializationKarma { get; set; } = 7;

        public int MartialArtsStyleKarma { get; set; } = 7;

        public int MartialArtsTechniqueKarma { get; set; } = 5;

        public int InPlayQualityMult { get; set; } = 2;

        #endregion // Flat Karma Costs

        /// <summary>
        /// For use in quadratically scaling Karma calulations (Attributes, skills, Initiation, & Submersion).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private int ValueAt(int value)
        {
            return ((value + 1) * value) / 2;
        }

        public static GenSettings LoadFromfile(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(GenSettings));
            GenSettings value = new GenSettings();
            using (StreamReader reader = new StreamReader(filename))
            {
                value = (GenSettings)ser.Deserialize(reader);
            }
            return value;
        }
        
        public GenSettings(Dictionary<string, object> loader)
        {
            object tmp; // temp pointer to hold value pulled from settings dict

            if (loader.TryGetValue("Method", out tmp) && tmp is CharGenMethod)
            {
                Method = (CharGenMethod)tmp;
            }
            else
            {
                Method = CharGenMethod.Priority;
            }

            if (!loader.TryGetValue("StartingKarma", out tmp) && tmp is int)
            {
                StartingKarma = (int)tmp;
            }
            else
            {
                StartingKarma = 25;
            }

            if (!loader.TryGetValue("AttributeKarmaMult", out tmp) && tmp is int)
            {
                AttributeKarmaMult = (int)tmp;
            }
            else
            {
                AttributeKarmaMult = 5;
            }

            if (!loader.TryGetValue("SkillGroupKarmaMult", out tmp) && tmp is int)
            {
                SkillGroupKarmaMult = (int)tmp;
            }
            else
            {
                SkillGroupKarmaMult = 5;
            }

            if (!loader.TryGetValue("ActiveSkillKarmaMult", out tmp) && tmp is int)
            {
                ActiveSkillKarmaMult = (int)tmp;
            }
            else
            {
                ActiveSkillKarmaMult = 2;
            }

            if (!loader.TryGetValue("MagicSkillKarmaMult", out tmp) && tmp is int)
            {
                MagicSkillKarmaMult = (int)tmp;
            }
            else
            {
                MagicSkillKarmaMult = 2;
            }

            if (!loader.TryGetValue("ResonanceSkillKarmaMult", out tmp) && tmp is int)
            {
                ResonanceSkillKarmaMult = (int)tmp;
            }
            else
            {
                ResonanceSkillKarmaMult = 2;
            }

            if (!loader.TryGetValue("KnowledgeSkillKarmaMult", out tmp) && tmp is int)
            {
                KnowledgeSkillKarmaMult = (int)tmp;
            }
            else
            {
                KnowledgeSkillKarmaMult = 1;
            }

            if (!loader.TryGetValue("LanguageSkillKarmaMult", out tmp) && tmp is int)
            {
                LanguageSkillKarmaMult = (int)tmp;
            }
            else
            {
                LanguageSkillKarmaMult = 1;
            }

            if (!loader.TryGetValue("InitiationKarmaBase", out tmp) && tmp is int)
            {
                InitiationKarmaBase = (int)tmp;
            }
            else
            {
                InitiationKarmaBase = 10;
            }

            if (!loader.TryGetValue("InitiationKarmaMult", out tmp) && tmp is int)
            {
                InitiationKarmaMult = (int)tmp;
            }
            else
            {
                InitiationKarmaMult = 3;
            }

            if (!loader.TryGetValue("SubmersionKarmaBase", out tmp) && tmp is int)
            {
                SubmersionKarmaBase = (int)tmp;
            }
            else
            {
                SubmersionKarmaBase = 10;
            }

            if (!loader.TryGetValue("SubmersionKarmaMult", out tmp) && tmp is int)
            {
                SubmersionKarmaMult = (int)tmp;
            }
            else
            {
                SubmersionKarmaMult = 3;
            }

            if (!loader.TryGetValue("ComplexFormKarma", out tmp) && tmp is int)
            {
                ComplexFormKarma = (int)tmp;
            }
            else
            {
                ComplexFormKarma = 4;
            }

            if (!loader.TryGetValue("SpellKarma", out tmp) && tmp is int)
            {
                SpellKarma = (int)tmp;
            }
            else
            {
                SpellKarma = 5;
            }

            if (!loader.TryGetValue("PowerPointKarma", out tmp) && tmp is int)
            {
                PowerPointKarma = (int)tmp;
            }
            else
            {
                PowerPointKarma = 5;
            }

            if (!loader.TryGetValue("SpecializationKarma", out tmp) && tmp is int)
            {
                SpecializationKarma = (int)tmp;
            }
            else
            {
                SpecializationKarma = 7;
            }

            if (!loader.TryGetValue("MartialArtsStyleKarma", out tmp) && tmp is int)
            {
                MartialArtsStyleKarma = (int)tmp;
            }
            else
            {
                MartialArtsStyleKarma = 7;
            }

            if (!loader.TryGetValue("MartialArtsTechniqueKarma", out tmp) && tmp is int)
            {
                MartialArtsTechniqueKarma = (int)tmp;
            }
            else
            {
                MartialArtsTechniqueKarma = 5;
            }

            if (!loader.TryGetValue("InPlayQualityMult", out tmp) && tmp is int)
            {
                InPlayQualityMult = (int)tmp;
            }
            else
            {
                InPlayQualityMult = 2;
            }
        }

        public GenSettings()
        {
        }
    }
}
