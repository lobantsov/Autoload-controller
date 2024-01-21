using System.Drawing;
using System.Management;
using Microsoft.Win32;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.Reflection;
namespace Autoload_control.Classes;

public class ParseActiveProcess
{
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
                        Console.WriteLine($"Process: {processName}");
                        Console.WriteLine($"Main Executable: {mainExecutable}");
                        Console.WriteLine();
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
            !mainExecutable.ToLower().Contains("\\WindowsApps\\") &&
            processName != null )
            // && processName.ToLower().Contains("your_main_process_identifier"))
            return true;
        return false;
    }

}