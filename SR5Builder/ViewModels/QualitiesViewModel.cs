using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SR5Builder.DataModels;

namespace SR5Builder.ViewModels
{
    public class QualitiesViewModel: ViewModelBase
    {
        private int mBob;

        public int Bob
        {
            get { return mBob; }
            set
            {
                if (mBob != value)
                {
                    mBob = value;
                    RaisePropertyChanged(nameof(Bob);
                }
            }
        }

    }
}
