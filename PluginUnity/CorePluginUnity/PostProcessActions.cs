using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Core.Plugin.Unity
{
    public static class PostProcessActions
    {
        private static readonly string dependenciesPath;

        static PostProcessActions()
        {
            dependenciesPath = Application.dataPath + "/../";
        }

        [PostProcessBuild(1)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            switch (target)
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    if (!Editor.Components.Buttons.MLButton.ValidateDependenciesStatus())
                    {
                        Debug.Log("Embedding Machine Learning Package into build.");
                        foreach (var file in Editor.Components.Buttons.MLButton.GetDependencyList())
                        {
                            var dll = file + ".dll";
                            var path = Path.GetDirectoryName(pathToBuiltProject) + "/" + Path.GetFileNameWithoutExtension(pathToBuiltProject) + "_Data/Managed/" + dll;
                            //var path = Path.GetDirectoryName(pathToBuiltProject) + "/" + dll;
                            //Debug.Log("Copying [" + dll + "] [" + path);
                            File.Copy(dependenciesPath + dll, path, true);
                            //File.Copy(dependenciesPath + dll, pathToBuiltProject + Path.GetFileName(dll), true);
                        }
                    }
                    break;
                default:
                    break;
            }
            //Debug.Log(pathToBuiltProject);
        }
    }
}