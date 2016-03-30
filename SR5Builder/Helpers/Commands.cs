using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SR5Builder.Helpers
{
    public static class Commands
    {
        public static void OpenSourcePage(string file, int page)
        {
            string path = Properties.Settings.Default.pdfReaderPath;
            string args = "/A \"page=" + page + "\" \"" + file + "\"";
            Debug.WriteLine(path);
            Debug.WriteLine(args);
            Process.Start(path, args);
        }

    }
}
