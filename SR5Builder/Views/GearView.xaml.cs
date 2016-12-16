using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for GearView.xaml
    /// </summary>
    public partial class GearView : UserControl
    {
        public GearView()
        {
            InitializeComponent();
        }

        private void ListView_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked =
                e.OriginalSource as GridViewColumnHeader;
            //ListSortDirection direction;
        }
    }
}
