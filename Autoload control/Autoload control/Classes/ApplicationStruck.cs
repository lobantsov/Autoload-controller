using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Autoload_control.Classes
{
    public class ApplicationStruck
    {
        public string Name { get; set; }
        public string Command { get; set; }
        public string Location;
        public string User;
        public string IconPath { get; set; }

        public bool IsSelected { get; set; }
        
    }
}
