using System.IO;
using System.Xml.Serialization;

namespace SR5Builder.DataModels
{
    /// <summary>
    /// This class contains settings and customizations for character generation.
    /// Each SR5Character contains its own instance of it.
    /// </summary>
    public class GenSettings
    {
        public CharGenMethod Method { get; set; }

        #region Scaling Karma Costs

        /// <summary>
        /// For Karma advancement of Attributes.
        /// Karma cost is [New Rating] x AttributeKarmaMult (default 5 in base rules).
        /// </summary>
        public int AttributeKarmaMult { get; set; }

        /// <summary>
        /// For Karma advancement of Skill Groups.
        /// Karma cost is [New Rating] x SkillGroupKarmaMult (default 5 in base rules).
        /// </summary>
        public int SkillGroupKarmaMult { get; set; }

        /// <summary>
        /// For Karma advancement of Active Skills (including social).
        /// Karma cost is [New Rating] x ActiveSkillKarmaMult (default 2 in base rules).
        /// </summary>
        public int ActiveSkillKarmaMult { get; set; }

        /// <summary>
        /// For Karma advancement of Magic Skills.
        /// Karma cost is [New Rating] x MagicSkillKarmaMult (default 2 in base rules).
        /// </summary>
        public int MagicSkillKarmaMult { get; set; }

        /// <summary>
        /// For Karma advancement of Resonance Skills.
        /// Karma cost is [New Rating] x ResonanceSkillKarmaMult (default 2 in base rules).
        /// </summary>
        public int ResonanceSkillKarmaMult { get; set; }

        /// <summary>
        /// For Karma advancement of Knowledge Skills.
        /// Karma cost is [New Rating] x KnowledgeSkillKarmaMult (default 1 in base rules).
        /// </summary>
        public int KnowledgeSkillKarmaMult { get; set; }

        /// <summary>MagicSkill
        /// For Karma advancement of Language Skills (including social).
        /// Karma cost is [New Rating] x LanguageSkillKarmaMult (default 1 in base rules).
        /// </summary>
        public int LanguageSkillKarmaMult { get; set; }

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

        public int InitiationKarmaBase { get; set; }

        public int InitiationKarmaMult { get; set; }

        public int SubmersionKarmaBase { get; set; }

        public int SubmersionKarmaMult { get; set; }

        public int InitationKarma(int value, int min = 0)
        {
            return InitiationKarmaBase * (value - min) + (ValueAt(value) - ValueAt(min)) * InitiationKarmaMult;
        }

        public int SubmersionKarma(int value, int min = 0)
        {
            return SubmersionKarmaBase * (value - min) + (ValueAt(value) - ValueAt(min)) * SubmersionKarmaMult;
        }

        #endregion Scaling Karma Costs

        #region Flat Karma Costs

        public int ComplexFormKarma { get; set; }

        public int SpellKarma { get; set; }

        public int SpecializationKarma { get; set; }

        public int MartialArtsStyleKarma { get; set; }

        public int MartialArtsTechniqueKarma { get; set; }

        public int InPlayQualityMult { get; set; }

        #endregion // Flat Karma Costs

        /// <summary>
        /// For Use in scaling Karma calulations (Attributes, skills, Initiation, & Submersion).
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
            StreamReader reader = new StreamReader(filename);

            GenSettings value = (GenSettings)ser.Deserialize(reader);

            reader.Close();
            return value;
        }
    }
}
