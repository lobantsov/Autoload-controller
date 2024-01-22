using System.Drawing;
using System.IO;

namespace Autoload_control.Classes.GetIco;

public class GetIcons
{
    private static GetIcons _getIconsInstance;
    
    private GetIcons()
    { }

    public static GetIcons GetInstace()
    {
        return _getIconsInstance ?? (_getIconsInstance = new GetIcons());
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
            return filename+".ico";
        }
        catch (Exception ex)
        {
            return "";
        }
    }
}