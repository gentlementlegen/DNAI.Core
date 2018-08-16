using System.Collections.Generic;

namespace DNAIPluginPublisher.Model
{
    public class Packer
    {
        public void Pack(IEnumerable<Item> items)
        {
            Logger.Log("Packaging files");
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                //WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "\"D:\\Program Files (x86)\\Unity\\Editor\\Unity.exe\"",
                //Arguments = @"/c ""D:\Program Files (x86)\Unity\Editor\Unity.exe"" -quit -batchmode -projectPath ""something"" -exportPackage toto DNAI",
                Arguments = "-quit -batchmode -projectPath=\"D:\\Folders\\VisualStudio\\Duly\\PluginUnity\\PluginTestProject\" -exportPackage \"Assets/Animations\" C:\\Users\\ferna\\Desktop\\PluginUploader\\TODELEEEE.unityPackage",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            process.StartInfo = startInfo;
            System.Diagnostics.Debug.WriteLine(startInfo.Arguments);
            process.Start();
            process.WaitForExit();
            if (process.ExitCode == 0)
            {
                Logger.Log("DNAI package has been created successfully.");
            }
            else
            {
                Logger.Log("Failed to create Unity Package.");
            }
        }
    }
}