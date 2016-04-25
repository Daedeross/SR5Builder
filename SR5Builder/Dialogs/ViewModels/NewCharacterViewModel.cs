using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.DataModels;

namespace SR5Builder.ViewModels
{
    public class NewCharacterViewModel : DialogViewModel
    {
        public override string Message
        {
            get
            {
                return "Select or load the settings.";
            }
        }

        public GenSettings CurrentSettings { get; protected set; }
    }
}
