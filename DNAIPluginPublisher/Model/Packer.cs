using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DNAIPluginPublisher.Model
{
    /// <summary>
    /// Packs the file in a Unity Package file
    /// </summary>
    public class Packer
    {
        private string _projectPath = "";

        public void Pack(IEnumerable<Item> items, string projectPath, string packageName)
        {
            if (!CheckUnityProjectValidity(items as ICollection<Item>))
                return;
            _projectPath = projectPath;
            Logger.Log("Packaging files. This operation might take a while.");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                //WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = GetUnityPath(),
                //Arguments = @"/c ""D:\Program Files (x86)\Unity\Editor\Unity.exe"" -quit -batchmode -projectPath ""something"" -exportPackage toto DNAI",
                //Arguments = "-quit -batchmode -projectPath=\"D:\\Folders\\VisualStudio\\Duly\\PluginUnity\\PluginTestProject\" -exportPackage \"Assets/Animations\" C:\\Users\\ferna\\Desktop\\PluginUploader\\TODELEEEE.unityPackage",
                Arguments = "-quit -batchmode",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            startInfo.Arguments += " -projectPath=\"" + projectPath + "\"";
            startInfo.Arguments += " -exportPackage ";
            var s = GetSelectedItems(items);
            if (string.IsNullOrEmpty(s))
            {
                Logger.Log("No files selected, aborting.");
                return;
            }
            startInfo.Arguments += s;
            startInfo.Arguments += " \"C:\\Users\\ferna\\Desktop\\PluginUploader\\" + packageName + "\"";

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode == 0)
            {
                Logger.Log("Unity package has been created successfully.");
            }
            else
            {
                Logger.Log("Failed to create Unity Package." + process.StandardError.ReadToEnd() + process.StandardOutput.ReadToEnd());
            }
        }

        private string GetSelectedItems(IEnumerable<Item> items)
        {
            var ret = "";
            foreach (var item in items)
            {
                if (item.IsSelected)
                {
                    if (item is DirectoryItem di)
                    {
                        //ret += "\"" + MakeRelativePath(_projectPath, item.Path) + "\" ";
                        ret += GetSelectedItems(di.Items);
                    }
                    else
                    {
                        ret += "\"" + MakeRelativePath(_projectPath, item.Path) + "\" ";
                    }
                }
            }

            return ret;
        }

        private bool CheckUnityProjectValidity(ICollection<Item> items)
        {
            if (items.Count <= 0)
            {
                Logger.Log("The folder is empty, cannot proceed.");
                return false;
            }
            if (!items.Any(x => x.Name == "Assets"))
            {
                Logger.Log("The current selected folder doesn't appear to be a Unity Project root folder, skipping.");
                return false;
            }
            return true;
        }

        private String MakeRelativePath(String fromPath, String toPath)
        {
            if (String.IsNullOrEmpty(fromPath)) throw new ArgumentNullException("fromPath");
            if (String.IsNullOrEmpty(toPath)) throw new ArgumentNullException("toPath");

            Uri fromUri = new Uri(fromPath);
            Uri toUri = new Uri(toPath);

            if (fromUri.Scheme != toUri.Scheme) { return toPath; } // path can't be made relative.

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }

        // TODO : implement path
        private string GetUnityPath()
        {
            return "\"D:\\Program Files (x86)\\Unity\\Editor\\Unity.exe\"";
        }
    }
}