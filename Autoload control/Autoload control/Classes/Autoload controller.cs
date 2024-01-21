using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

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
                foreach (var autoLoadStruck in _autoLoadStrucks)
                {
                    string processNameWithoutExtension = Path.GetFileNameWithoutExtension(autoLoadStruck.Command);

                    Process[] processes = Process.GetProcessesByName(processNameWithoutExtension);

                    foreach (Process process in processes)
                    {
                        try
                        {
                            process.Kill();
                            Console.WriteLine($"Завершено процес: {process.ProcessName}");
                        }
                        catch (Exception exception)
                        { }
                    }
                }
            }


        }
    }
}
