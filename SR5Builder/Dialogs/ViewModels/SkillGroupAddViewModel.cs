using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers;
using System.Windows.Input;

namespace SR5Builder.ViewModels
{
    class SkillGroupAddViewModel : DialogViewModel
    {
        #region Private Fields

        private string[] skillNames;
        private string groupName;

        #endregion // Private fields

        #region Properties

        public override string Message
        {
            get
            {
                string skNames = String.Join(", ", skillNames);



                if (skillNames.Length == 1)
                {
                    return "The skill group " + groupName + " contains the skill " + skNames + " which is already on your character. Adding this group will remove that individual skill.";
                }
                else
                    return "The skill group " + groupName + " contain the skills " + skNames + " which are already on your character. Adding this group will remove those individual skills.";
            }
        }

        #endregion // Properties

        #region Constructors

        public SkillGroupAddViewModel(string groupName, string[] skillNames)
        {
            this.groupName = groupName;
            this.skillNames = skillNames;
        }

        #endregion // Constructors

        #region Commands

        #endregion // Commands

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}
