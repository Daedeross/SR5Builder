using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.DataModels;

namespace SR5Builder.ViewModels
{
    public class PriorityLevelViewModel : ViewModelBase
    {
        private Priority mPriorityLevel;

        public Priority PriorityLevel
        {
            get { return mPriorityLevel; }
            set
            {
                if (mPriorityLevel != value)
                {
                    mPriorityLevel = value;
                    OnPropertyChanged("Priority");
                }
            }
        }

        private Priorities mPriorities;

        public Priorities Priorities
        {
            get { return mPriorities; }
            set
            {
                if (mPriorities != value)
                {
                    if (mPriorities != null)
                    {
                        mPriorities.PropertyChanged -= this.BubblePropertyChanged;
                    }
                    mPriorities = value;
                    mPriorities.PropertyChanged += this.BubblePropertyChanged;
                    OnPropertyChanged("Priorities");
                }
            }
        }

        public bool Metatype
        {
            get { return mPriorities.Metatype == mPriorityLevel; }
            set
            {
                mPriorities.Metatype = mPriorityLevel;
            }
        }

        public bool Attributes
        {
            get { return mPriorities.Attributes == mPriorityLevel; }
            set
            {
                mPriorities.Attributes = mPriorityLevel;
            }
        }

        public bool Special
        {
            get { return mPriorities.Special == mPriorityLevel; }
            set
            {
                mPriorities.Special = mPriorityLevel;
            }
        }

        public bool Skills
        {
            get { return mPriorities.Skills == mPriorityLevel; }
            set
            {
                mPriorities.Skills = mPriorityLevel;
            }
        }

        public bool Resources
        {
            get { return mPriorities.Resources == mPriorityLevel; }
            set
            {
                mPriorities.Resources = mPriorityLevel;
            }
        }

        private int intLevel() { return (int)mPriorityLevel - 1; }

        public string MetatypeText { get { return Priorities.MetatypeText[intLevel()]; } }

        public string AttributesText { get { return Priorities.AttributesText[intLevel()]; } }

        public string SpecialText { get { return Priorities.SpecialText[intLevel()]; } }

        public string SkillsText { get { return Priorities.SkillsText[intLevel()]; } }

        public string ResourcesText { get { return Priorities.ResourcesText[intLevel()]; } }

        public PriorityLevelViewModel(Priority level)
        {
            mPriorityLevel = level;
        }

        public PriorityLevelViewModel(Priority level, Priorities p)
        {
            mPriorityLevel = level;
            Priorities = p;
        }
    }
}
