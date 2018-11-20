using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Predict : ExecutionRefreshInstruction
    {
        public Predict()
        {
            AddInput("model", new Entity.Variable(Entity.Type.Ressource.Instance));
            AddInput("weights", new Entity.Variable(Entity.Type.Ressource.Instance));
            AddInput("inputs", new Entity.Variable(Entity.Type.Matrix.Instance));
            AddOutput("outputs", new Entity.Variable(Entity.Type.Matrix.Instance));
        }

        public override void Execute()
        {
            dynamic model = GetInputValue("model");
            dynamic weights = GetInputValue("weights");
            dynamic inputs = GetInputValue("inputs");
            string csvData = Entity.Type.Matrix.Instance.toCSV(inputs);
            string modelPath = $"{Entity.Type.Ressource.Instance.Directory}/{model}";
            string weightsPath = $"{Entity.Type.Ressource.Instance.Directory}/{weights}";

            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException($"Model file not found for prediction: {modelPath}");
            }

            if (!File.Exists(weightsPath))
            {
                throw new FileNotFoundException($"Weights file not found for prediction: {weightsPath}");
            }

            string assemblyPath = Path.GetDirectoryName(GetType().Assembly.Location);
            Process python = new Process();

            python.StartInfo.FileName = $"{assemblyPath}/.Keras_loaded_model/python/Scripts/python";
            python.StartInfo.Arguments = $"\"{assemblyPath}/.Keras_loaded_model/keras_restore_machine_learning.py\" \"{modelPath}\" \"{weightsPath}\"";
            python.StartInfo.UseShellExecute = false;
            python.StartInfo.RedirectStandardInput = true;
            python.StartInfo.RedirectStandardOutput = true;
            python.StartInfo.RedirectStandardError = true;

            try
            {
                python.Start();

                python.StandardInput.Write(csvData.ToCharArray());
                python.StandardInput.Close();

                python.WaitForExit();
                
                if (python.ExitCode != 0)
                {
                    throw new InvalidOperationException($"Couldn't predict (python exit {python.ExitCode}");
                }

                csvData = python.StandardOutput.ReadToEnd();
            }
            catch (Exception err)
            {
                throw new InvalidOperationException($"{python.StartInfo.FileName}/{python.StartInfo.Arguments} {python.StandardError.ReadToEnd()}", err);
            }
            
            dynamic output = Entity.Type.Matrix.Instance.fromCSV(csvData);

            SetOutputValue("outputs", output);
        }
    }
}
