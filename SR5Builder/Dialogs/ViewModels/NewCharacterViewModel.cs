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

        public SettingsViewModel Settings { get; set; }

        public NewCharacterViewModel()
        {
            Settings = new SettingsViewModel();
        }
    }
}
