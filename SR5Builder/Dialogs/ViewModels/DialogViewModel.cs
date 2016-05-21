using Helpers;
using SR5Builder.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SR5Builder.ViewModels
{
    public abstract class DialogViewModel : ViewModelBase
    {
        private bool? mResult;

        public bool? Result
        {
            get { return mResult; }
            set
            {
                if (mResult != value)
                {
                    mResult = value;
                    System.Diagnostics.Debug.WriteLine("Result");
                    OnPropertyChanged(nameof(Result));
                }
            }
        }

        public abstract string Message { get; }

        #region Commands

            #region OkCommand

        private RelayCommand mOkCommand;
        public ICommand OkCommand
        {
            get
            {
                if (mOkCommand == null)
                {
                    mOkCommand = new RelayCommand(p => this.OkExecute(), p => this.OkCanExecute());
                }
                return mOkCommand;
            }
        }

        private void OkExecute()
        {
            Result = true;
        }

        private bool OkCanExecute()
        {
            return true;
        }

            #endregion // OkCommand

            #region CancelCommand

        private RelayCommand mCancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                if (mCancelCommand == null)
                {
                    mCancelCommand = new RelayCommand(p => this.CancelExecute(), p => this.CancelCanExecute());
                }
                return mCancelCommand;
            }
        }

        private void CancelExecute()
        {
            Result = false;
        }

        private bool CancelCanExecute()
        {
            return true;
        }

            #endregion // CancelCommand

        #endregion // Commands

    }
}
