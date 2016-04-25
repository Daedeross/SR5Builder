using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using DrWPF.Windows.Data;

namespace SR5Builder.DataModels
{
    public struct PriorityLevel
    {
        public Priority Priority;

        public string[] Metatypes;

        public int AttributePoints;

        public int SkillPoints;

        public int SkillGroupPoints;

        public int Resources;
    }

    public class Priorities : DataModelBase
    {
        public CharGenMethod Method { get; protected set; }

        private Priority mMetatype = Priority.U;
        public Priority Metatype
        {
            get { return mMetatype; }
            set
            {
                if (mMetatype != value)
                {
                    if (value == Priority.U)
                    {
                        mMetatype = value;
                        OnPropertyChanged("Metatype");
                    }
                    else if (mMetatype == Priority.U || Method == CharGenMethod.SumToTen)
					{
						mMetatype = value;
                        OnPropertyChanged("Metatype");
					}
					else
                    {
						SwapPriorities(mMetatype, value);
                        mMetatype = value;
                        OnPropertyChanged("Metatype");
                    }
                }
            }
        }

        private Priority mAttributes = Priority.U;
        public Priority Attributes
        {
            get { return mAttributes; }
            set
            {
                if (mAttributes != value)
                {
                    if (value == Priority.U)
                    {
                        mAttributes = value;
                        OnPropertyChanged("Attributes");
                    }
                    else if (mAttributes == Priority.U || Method == CharGenMethod.SumToTen)
					{
						mAttributes = value;
                        OnPropertyChanged("Attributes");
					}
					else
                    {
						SwapPriorities(mAttributes, value);
                        mAttributes = value;
                        OnPropertyChanged("Attributes");
                    }
                }
            }
        }

        private Priority mSpecial = Priority.U;
        public Priority Special
        {
            get { return mSpecial; }
            set
            {
                if (mSpecial != value)
                {
                    if (value == Priority.U)
                    {

                        mSpecial = value;
                        OnPropertyChanged("Special");
                    }
                    else if (mSpecial == Priority.U || Method == CharGenMethod.SumToTen)
					{
						mSpecial = value;
                        OnPropertyChanged("Special");
					}
					else
                    {
						SwapPriorities(mSpecial, value);
                        mSpecial = value;
                        OnPropertyChanged("Special");
                    }
                }
            }
        }

        private Priority mSkills = Priority.U;
        public Priority Skills
        {
            get { return mSkills; }
            set
            {
                if (mSkills != value)
                {
                    if (value == Priority.U)
                    {
                        mSkills = value;
                        OnPropertyChanged("Skills");
                    }
                    else if (mSkills == Priority.U || Method == CharGenMethod.SumToTen)
					{
						mSkills = value;
                        OnPropertyChanged("Skills");
					}
					else
                    {
						SwapPriorities(mSkills, value);
                        mSkills = value;
                        OnPropertyChanged("Skills");
                    }
                }
            }
        }

        private Priority mResources = Priority.U;
        public Priority Resources
        {
            get { return mResources; }
            set
            {
                if (mResources != value)
                {
                    if (value == Priority.U)
                    {
                        mResources = value;
                        OnPropertyChanged("Resources");
                    }
                    else if (mResources == Priority.U || Method == CharGenMethod.SumToTen)
					{
						mResources = value;
                        OnPropertyChanged("Resources");
					}
					else
                    {
						SwapPriorities(mResources, value);
                        mResources = value;
                        OnPropertyChanged("Resources");
                    }
                }
            }
        }

        public static string[] MetatypeText;

        public static string[] AttributesText;

        public static string[] SpecialText;

        public static string[] SkillsText;

        public static string[] ResourcesText;

        /// <summary>
        /// Verifies that the Priorities have been selected appropriately.
        /// </summary>
        /// <returns>True if the selected priorities are valid.</returns>
        public bool Verify()
        {
            switch (Method)
            {
                case CharGenMethod.NPC:				
                case CharGenMethod.KarmaGen:
                case CharGenMethod.LifeModules:
                case CharGenMethod.BuildPoints:
                    return true;
                case CharGenMethod.Priority:
					return (mAttributes.Mask() |
							mMetatype.Mask() |
							mResources.Mask() |
							mSkills.Mask() |
							mSpecial.Mask()
							) == 0x3e;
                case CharGenMethod.SumToTen:
					int sum = (int)mMetatype + (int)mAttributes + (int)mSpecial +
							  (int)mSkills + (int)mResources;		  
					sum -= 5; // compensate for E=1 in code while E=0 in rules (accross 5 columns)
					
					// return true is sum is ten and all priorities are at least E
                    return (sum == 10) &&
						   (mAttributes.Mask() |
							mMetatype.Mask() |
							mResources.Mask() |
							mSkills.Mask() |
							mSpecial.Mask()
							) != 0x0;
                default:
                    return false;
            }
        }

        public Priorities()
        {
            Initialize();
        }
		
		public void Reset()
		{
			mAttributes = Priority.U;
			mMetatype = Priority.U;
			mSpecial = Priority.U;
			mSkills = Priority.U;
			mResources = Priority.U;
		}

        /// <summary>
        /// TODO: Error checking, maybe fold into property
        /// </summary>
        /// <param name="m"></param>
        public void ChangeMethod(CharGenMethod m)
        {
            Method = m;
        }

        /// <summary>
        /// Hard coded initially. Later will generate text from loaded PriorityLevel list.
        /// </summary>
        private void Initialize()
        {
            if(MetatypeText == null)
            {
                MetatypeText = new string[5] {
                    "Human (9)\nElf (8)\nDwarf (7)\nOrk (7)\nTroll (5)",
                    "Human (7)\nElf (6)\nDwarf (4)\nOrk (4)\nTroll (0)",
                    "Human (5)\nElf (3)\nDwarf (1)\nOrk (0)",
                    "Human (3)\nElf (0)", "Human(1)"
                    };
                AttributesText = new string[5] {
                    "24", "20", "16", "14", "12"
                    };
                SpecialText = new string[5] {
                    "Magician or Mystic Adept: Magic 6, two Rating 5 Magical skills, 10 spells\nTechnomancer: Resonance 6, two Rating 5 Resonance skills, 5 complex forms",
                    "Magician or Mystic Adept: Magic 4, two Rating 4 Magical skills, 7 spells\nTechnomancer: Resonance 4, two Rating 4 Resonance skills, 2 complex forms\nAdept: Magic 6, one Rating 4 Active skill\nAspected Magician: Magic 5, one Rating 4 Magical skill group",
                    "Magician or Mystic Adept: Magic 3, 5 spells\nTechnomancer: Resonance 3, 1 complex form\nAdept: Magic 4, one Rating 2 Active skill\nAspected Magician: Magic 3, one Rating 2 Magical skill group",
                    "Adept: Magic 2\nAspected Magician: Magic 2",
                    "-"
                    };
                SkillsText = new string[5] {
                    "46/10",
                    "36/5",
                    "28/2",
                    "22/0",
                    "18/0",
                    };
                ResourcesText = new string[5] {
                    "450,000¥",
                    "275,000¥",
                    "140,000¥",
                    "50,000¥",
                    "6,000¥"
                    };
            }
        }
		
		private void SwapPriorities(Priority oldP, Priority newP)
		{
			if (mMetatype == newP)
			{
				mMetatype = oldP;
				OnPropertyChanged("Metatype");
			}
			else if (mAttributes == newP)
			{
				mAttributes = oldP;
				OnPropertyChanged("Attributes");
			}
			else if (mSkills == newP)
			{
				mSkills = oldP;
				OnPropertyChanged("Skills");
			}
			else if (mSpecial == newP)
			{
				mSpecial = oldP;
				OnPropertyChanged("Special");
			}
			else if (mResources == newP)
			{
				mResources = oldP;
				OnPropertyChanged("Resources");
			}
		}
    }
}