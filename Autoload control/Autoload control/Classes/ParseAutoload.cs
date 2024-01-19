using System.Drawing;
using System.Management;
using Microsoft.Win32;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System.Reflection;

namespace Autoload_control.Classes
{
    class ParseAutoload
    {
        private List<AutoLoadStruck> autoloadList;
        public List<AutoLoadStruck> GetStartupApplications()
        {
            autoloadList = new List<AutoLoadStruck>();

            // GetStartupApplications(Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"));
            // GetStartupApplications(Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"));
            GetThirdPartyAutoStartApps();
            GetAppInStartUpFolderStartApps(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Windows\\Start Menu\\Programs\\Startup"));
            GetAppInStartUpFolderStartApps(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup)));
            foreach (var VARIABLE in autoloadList)
            {
                VARIABLE.IconPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Icons",
                    GetFileIcon(VARIABLE.Command, VARIABLE.Name));
                
                if (VARIABLE.IconPath.Contains("desktop.ini.ico"))
                {
                    VARIABLE.IconPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Icons","Unknow.exe.ico");
                }
            }

            return autoloadList;
        }

        // private void GetStartupApplications(RegistryKey key)
        // {
        //     if (key != null)
        //     {
        //         string[] valueNames = key.GetValueNames();
        //
        //         foreach (string valueName in valueNames)
        //         {
        //             string valueData = key.GetValue(valueName).ToString();
        //
        //             AutoLoadStruck appInfo = new AutoLoadStruck
        //             {
        //                 Name = valueName,
        //                 IsEnabled = IsAutoLoadEnabled(valueName) 
        //             };
        //
        //             autoloadList.Add(appInfo);
        //         }
        //     }
        // }
        private bool IsAutoLoadEnabled(string appName)
        {
            RegistryKey keyCurrentUser = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            RegistryKey keyLocalMachine = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

            bool isEnabled = false;

            if (keyCurrentUser != null && keyCurrentUser.GetValue(appName) != null)
            {
                isEnabled = true;
            }
            else if (keyLocalMachine != null && keyLocalMachine.GetValue(appName) != null)
            {
                isEnabled = true;
            }

            return isEnabled;
        }
        
        private void GetThirdPartyAutoStartApps()
        {
            try
            {
                using (ManagementObjectSearcher searcher =
                       new ManagementObjectSearcher("root\\CIMv2", "SELECT * FROM Win32_StartupCommand"))
                {

                    foreach (ManagementObject obj in searcher.Get())
                    {
                        string command = obj["Command"] as string;
                        string location = obj["Location"] as string;
                        string user = obj["User"] as string;
                        int endIndex = command.IndexOf("exe", 0, StringComparison.OrdinalIgnoreCase);
                        if (endIndex != -1)
                        {
                            command = command.Substring(0, endIndex + 3).Trim();

                            if (command.Length > 0 && command[0] == '"')
                            {
                                command = command.Substring(1);
                            }
                        }

                        if (location == "Startup")
                        {
                            command = GetShortcutTargetFile(Path.GetFileName(command));
                        }
                        autoloadList.Add(new AutoLoadStruck()
                        {
                            Name = Path.GetFileName(command),
                            IsEnabled = IsAutoLoadEnabled(Path.GetFileName(command)),
                            Command = command,
                            Location = location,
                            User = user
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void GetAppInStartUpFolderStartApps(string path)
        {
            try
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    autoloadList.Add(new AutoLoadStruck()
                    {
                        Name = Path.GetFileName(file)
                    });
                }

                string[] directories = Directory.GetDirectories(path);
                foreach (string directory in directories)
                {
                    GetAppInStartUpFolderStartApps(directory);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public string GetFileIcon(string filePath, string filename)
        {
            try
            {
                string iconFilePath = "";
                    if (File.Exists(filePath))
                    {
                        FileInfo fileInfo = new FileInfo(filePath);

                        if (File.Exists(fileInfo.FullName))
                        {
                            Icon fileIcon = Icon.ExtractAssociatedIcon(fileInfo.FullName);

                            if (fileIcon != null)
                            {
                                iconFilePath = @$"Icons\{filename}.ico";
                                if (!File.Exists(iconFilePath))
                                {

                                    using (FileStream stream = new FileStream(iconFilePath, FileMode.Create))
                                    {
                                        fileIcon.Save(stream);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("File does not exist.");
                    }
                return filename+".ico";
            }
            catch (Exception ex)
            {
                return "";
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private string GetShortcutTargetFile(string shortcutname)
        {
            try
            {
                string startupFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                    "Microsoft", "Windows", "Start Menu", "Programs", "Startup");
                ShellObject shellObject = ShellObject.FromParsingName($"{startupFolderPath}\\{shortcutname}");
                IShellProperty prop = shellObject.Properties.GetProperty(SystemProperties.System.Link.TargetParsingPath);
                string targetFilePath = prop.ValueAsObject.ToString();
                return targetFilePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}