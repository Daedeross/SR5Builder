using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.DataModels;

namespace SR5Builder.ViewModels
{
    public class SelectionViewModel: DialogViewModel
    {
        #region Private Fields

        private SR5Character character;

        private string message;

        #endregion // Private fields

        #region Properties

        public override string Message
        {
            get { return message; }
        }

        public bool Fillable
        {
            get { return (SelectionList == null || SelectionList.Count == 0); }
        }

        public List<string> SelectionList { get; set; }

        public string Selection { get; set; }

        #endregion // Properties

        #region Constructors

        public SelectionViewModel()
        {

        }

        public SelectionViewModel(string message)
            :base ()
        {
            this.message = message;
        }

        #endregion // Constructors

        #region Commands

        #endregion // Commands

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        public void SetMessage(string msg)
        {
            message = msg;
        }

        #endregion // Public Methods
    }
}