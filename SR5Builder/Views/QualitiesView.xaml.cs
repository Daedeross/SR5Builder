using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SR5Builder.Views
{
    /// <summary>
    /// Interaction logic for QualitiesView.xaml
    /// </summary>
    public partial class QualitiesView : UserControl
    {
        public QualitiesView()
        {
            InitializeComponent();
        }

        private void DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GridView gv = ((ListView)sender).View as GridView;
            if (gv != null)
            {
                foreach (var c in gv.Columns)
                {
                    // Code below was found in GridViewColumnHeader.OnGripperDoubleClicked() event handler (using Reflector)
                    // i.e. it is the same code that is executed when the gripper is double clicked
                    if (double.IsNaN(c.Width))
                    {
                        c.Width = c.ActualWidth;
                    }
                    c.Width = double.NaN;
                }
            }
        }
    }
}
