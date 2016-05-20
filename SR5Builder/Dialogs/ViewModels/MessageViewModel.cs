using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SR5Builder.ViewModels
{
    public class MessageViewModel : DialogViewModel
    {
        private string mMessage;
        public override string Message
        {
            get
            {
                return mMessage;
            }
        }

        public MessageViewModel()
        {

        }

        public MessageViewModel(string message)
        {
            mMessage = message;
            OnPropertyChanged(nameof(Message));
        }
    }
}
