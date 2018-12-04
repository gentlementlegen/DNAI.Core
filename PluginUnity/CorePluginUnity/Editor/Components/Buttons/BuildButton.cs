using UnityEditor;
using UnityEngine;

namespace Core.Plugin.Unity.Editor.Components.Buttons
{
    class BuildButton : Button
    {
        private readonly DulyEditor _editor;
        private int _maxScriptCount;
        private int _currentScriptCount;


        public BuildButton() : base("Build", AssetDatabase.LoadAssetAtPath<Texture>(Constants.ResourcesPath + "build.png"))
        {
            OnClicked += Build;
            _editor = DulyEditor.Instance;
        }

        private void Build()
        {
            Context.UnityTask.Run(async () =>
            {
                //await scriptManager.CompileAsync(_selectedScripts.FindIndices(x => x));
                try
                {
                    _editor.IsCompiling = true;
                    var lAI = _editor.ScriptDrawer.ListAI;
                    _maxScriptCount = lAI.Count;
                    for (int i = 0; i < _maxScriptCount; i++)
                    {
                        _currentScriptCount = i + 1;

                        if (EditorUtility.DisplayCancelableProgressBar("Compiling DNAI scripts",
                            $"Processed {_currentScriptCount}/{_maxScriptCount} scripts",
                            _currentScriptCount / (float)_maxScriptCount))
                        {
                            Debug.Log("Compilation canceled");
                            i = _maxScriptCount;
                            continue;
                        }

                        await lAI[i].scriptManager.CompileAsync();
                        AssetDatabase.ImportAsset(Constants.CompiledPath +
                                                  lAI[i].scriptManager.AssemblyName + ".dll");
                    }

                    EditorUtility.ClearProgressBar();
                    _editor.IsCompiling = false;
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    Debug.LogError(
                        $"Could not find the DNAI file {ex.FileName}. Make sure it exists in the Scripts folder.");
                }
                finally
                {
                    EditorUtility.ClearProgressBar();
                }
            }).ContinueWith((e) =>
            {
                if (e.IsFaulted)
                {
                    Debug.LogError(e?.Exception.GetBaseException().Message + " " +
                                   e?.Exception.GetBaseException().StackTrace);
                }

                _editor.IsCompiling = false;
            });
        }
    }
}
