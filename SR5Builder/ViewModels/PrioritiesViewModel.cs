using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.DataModels;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace SR5Builder.ViewModels
{
    public class PrioritiesViewModel : ViewModelBase
    {   
        #region Private Fields

        #endregion // Private fields

        #region Properties

        private Priorities mPriorities;

        public Priorities Priorities
        {
            get { return mPriorities; }
            set { mPriorities = value; }
        }

        public ObservableCollection<PriorityLevelViewModel> PrioritiesList
        {
            get;
            set;
        }

        #endregion // Properties

        #region Constructors

        private void Initialize()
        {
            List<PriorityLevelViewModel> list = new List<PriorityLevelViewModel>(5);
            list.Add(new PriorityLevelViewModel(Priority.A, mPriorities));
            list.Add(new PriorityLevelViewModel(Priority.B, mPriorities));
            list.Add(new PriorityLevelViewModel(Priority.C, mPriorities));
            list.Add(new PriorityLevelViewModel(Priority.D, mPriorities));
            list.Add(new PriorityLevelViewModel(Priority.E, mPriorities));

            PrioritiesList = new ObservableCollection<PriorityLevelViewModel>(list);
        }

        public PrioritiesViewModel()
        {
            mPriorities = new Priorities();

            Initialize();

            mPriorities.Metatype = Priority.C;
            mPriorities.Attributes = Priority.A;
            mPriorities.Special = Priority.E;
            mPriorities.Skills = Priority.B;
            mPriorities.Resources = Priority.D;
        }

        public PrioritiesViewModel(Priorities priorities)
        {
            mPriorities = priorities;
            Initialize();
        }

        #endregion // Constructors

        #region Commands

            #region Command Properties

            #endregion // Command Properties

            #region Command Delegates

            #endregion // Command Delegates

        #endregion // Commands

        #region Private Methods

        #endregion // Private Methods

        #region Public Methods

        #endregion // Public Methods
    }
}