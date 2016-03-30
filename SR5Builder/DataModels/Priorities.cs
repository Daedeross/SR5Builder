﻿using System;
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
                    else
                    {
                        if (mAttributes == value)
                        {
                            mAttributes = mMetatype;
                            OnPropertyChanged("Attributes");
                        }
                        else if (mSpecial == value)
                        {
                            mSpecial = mMetatype;
                            OnPropertyChanged("Special");
                        }
                        else if (mSkills == value)
                        {
                            mSkills = mMetatype;
                            OnPropertyChanged("Skills");
                        }
                        else if (mResources == value)
                        {
                            mResources = mMetatype;
                            OnPropertyChanged("Resources");
                        }
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
                    else
                    {
                        if (mMetatype == value)
                        {
                            mMetatype = mAttributes;
                            OnPropertyChanged("Metatype");
                        }
                        else if (mSpecial == value)
                        {
                            mSpecial = mAttributes;
                            OnPropertyChanged("Special");
                        }
                        else if (mSkills == value)
                        {
                            mSkills = mAttributes;
                            OnPropertyChanged("Skills");
                        }
                        else if (mResources == value)
                        {
                            mResources = mAttributes;
                            OnPropertyChanged("Resources");
                        }
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
                    else
                    {
                        if (mMetatype == value)
                        {
                            mMetatype = mSpecial;
                            OnPropertyChanged("Metatype");
                        }
                        else if (mAttributes == value)
                        {
                            mAttributes = mSpecial;
                            OnPropertyChanged("Attributes");
                        }
                        else if (mSkills == value)
                        {
                            mSkills = mSpecial;
                            OnPropertyChanged("Skills");
                        }
                        else if (mResources == value)
                        {
                            mResources = mSpecial;
                            OnPropertyChanged("Resources");
                        }
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
                    else
                    {
                        if (mMetatype == value)
                        {
                            mMetatype = mSkills;
                            OnPropertyChanged("Metatype");
                        }
                        else if (mAttributes == value)
                        {
                            mAttributes = mSkills;
                            OnPropertyChanged("Attributes");
                        }
                        else if (mSpecial == value)
                        {
                            mSpecial = mSkills;
                            OnPropertyChanged("Special");
                        }
                        else if (mResources == value)
                        {
                            mResources = mSkills;
                            OnPropertyChanged("Resources");
                        }
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
                    else
                    {
                        if (mMetatype == value)
                        {
                            mMetatype = mResources;
                            OnPropertyChanged("Metatype");
                        }
                        else if (mAttributes == value)
                        {
                            mAttributes = mResources;
                            OnPropertyChanged("Attributes");
                        }
                        else if (mSkills == value)
                        {
                            mSkills = mResources;
                            OnPropertyChanged("Skills");
                        }
                        else if (mSpecial == value)
                        {
                            mSpecial = mResources;
                            OnPropertyChanged("Special");
                        }
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

        public bool Verify()
        {
            return (mMetatype != Priority.U &&
                    mAttributes != Priority.U &&
                    mSpecial != Priority.U &&
                    mSkills != Priority.U &&
                    mResources != Priority.U);
        }

        public Priorities()
        {
            Initialize();
        }

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
    }
}