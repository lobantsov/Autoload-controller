using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private List<ApplicationStruck> _autoLoadStrucks;
        public Autoload_controller(List<ApplicationStruck> _autoLoadStrucks)
        {
            this._autoLoadStrucks = _autoLoadStrucks;
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
        }

        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (SystemParameters.PowerLineStatus == PowerLineStatus.Offline)
            {
                foreach (var VARIABLE in _autoLoadStrucks)
                {
                    Console.WriteLine(VARIABLE.Name);
                }
            }
        }
    }
}
