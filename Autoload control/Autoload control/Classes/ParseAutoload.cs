using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Autoload_control.Classes
{
    class ParseAutoload
    {
        public List<AutoLoadStruck> GetAutostartApps()
        {
            const string registryKeyPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            List<AutoLoadStruck> autoLoadList = new List<AutoLoadStruck>();

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registryKeyPath))
                {
                    if (key != null)
                    {
                        string[] valueNames = key.GetValueNames();

                        foreach (string valueName in valueNames)
                        {
                            string applicationPath = key.GetValue(valueName)?.ToString();
                            bool isEnabled = IsAutostartEnabled(registryKeyPath, valueName);

                            autoLoadList.Add(new AutoLoadStruck { Name = valueName, IsEnabled = isEnabled });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return autoLoadList;
        }

        private bool IsAutostartEnabled(string keyPath, string valueName)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyPath))
                {
                    if (key != null)
                    {
                        object value = key.GetValue(valueName);
                        return value != null;
                    }
                }
            }
            catch (Exception)
            {
                // Handle exceptions if necessary
            }

            return false;
        }
    }
}