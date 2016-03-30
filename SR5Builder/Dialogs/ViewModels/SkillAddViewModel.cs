using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.ViewModels
{
    public class SkillAddViewModel: DialogViewModel
    {
        #region Private Fields

        private string skillName;
        private string groupName;

        #endregion // Private fields

        #region Properties

        public override string Message
        {
            get
            {
                return "The skill '" + skillName + "' is already on your character under the '" + groupName + "' group. If you add this skill, that group will be removed.";
            }
        }

        #endregion // Properties

        #region Constructors

        public SkillAddViewModel(string groupName, string skillName)
        {
            this.groupName = groupName;
            this.skillName = skillName;
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