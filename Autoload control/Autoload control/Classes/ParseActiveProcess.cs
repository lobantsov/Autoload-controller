using System.Management;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using Autoload_control.Classes.GetIco;

namespace Autoload_control.Classes;

public class ParseActiveProcess
{
    GetIcons _getIcons = GetIcons.GetInstace();
    public List<ApplicationStruck> GetActiveApplication()
    {
        List<ApplicationStruck> mainProcesses = new List<ApplicationStruck>();

        try
        {
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    string processName = obj["Name"] as string;
                    string mainExecutable = obj["ExecutablePath"] as string;
                    if (Validation(processName, mainExecutable))
                    {
                        if (!mainProcesses.Any(process => process.Command == mainExecutable))
                        {
                            mainProcesses.Add(new ApplicationStruck()
                            {
                                Name = processName,
                                Command = mainExecutable
                            });
                        }
                    }
                }
            }
            foreach (var VARIABLE in mainProcesses)
            {
                VARIABLE.IconPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Icons",
                    _getIcons.GetFileIcon(VARIABLE.Command, VARIABLE.Name));
                
                if (VARIABLE.IconPath.Contains("desktop.ini.ico"))
                {
                    VARIABLE.IconPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Icons","Unknow.exe.ico");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        return mainProcesses;
    }

    private bool Validation(string processName, string mainExecutable)
    {
        if (!string.IsNullOrEmpty(mainExecutable) &&
            !mainExecutable.ToLower().Contains("\\windows\\system32\\") &&
            !mainExecutable.ToLower().Contains("\\windows\\systemapps\\") &&
            !mainExecutable.ToLower().Contains("explorer") &&
            !mainExecutable.ToLower().Contains("\\windows\\immersivecontrolpanel\\") &&
            !mainExecutable.ToLower().Contains("\\windowsapps\\") &&
            processName != null )
            // && processName.ToLower().Contains("your_main_process_identifier"))
            return true;
        return false;
    }

}