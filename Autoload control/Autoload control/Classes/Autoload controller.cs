using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace Autoload_control.Classes
{
    class Autoload_controller
    {
        public Autoload_controller()
        {
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            PowerLineStatus powerLineStatus = SystemParameters.PowerLineStatus;
            MessageBox.Show("Power line status: " + powerLineStatus);

        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            MessageBox.Show("woi");
        }
    }
}
