using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CF.Helpers
{
    public static class ErrorHelper
    {
        public static void ShowError(Exception ee)
        {
            MessageBox.Show(ee.Message);
        }
    }
}
