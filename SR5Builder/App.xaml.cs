using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace SR5Builder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SR5Builder.Helpers.Log.Close();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SR5Builder.Helpers.Log.Initialize();
        }
    }
}
