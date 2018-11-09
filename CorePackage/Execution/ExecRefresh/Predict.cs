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
            AddInput("model", new Entity.Variable(Entity.Type.Scalar.String));
            AddInput("weights", new Entity.Variable(Entity.Type.Scalar.String));
            AddInput("inputs", new Entity.Variable(Entity.Type.Matrix.Instance));
            AddOutput("outputs", new Entity.Variable(Entity.Type.Matrix.Instance));
        }

        public override void Execute()
        {
            dynamic model = GetInputValue("model");
            dynamic weights = GetInputValue("weights");
            dynamic inputs = GetInputValue("inputs");
            string csvData = Entity.Type.Matrix.Instance.toCSV(inputs);

            string cwd = AppDomain.CurrentDomain.BaseDirectory;
            Process python = new Process();

            python.StartInfo.FileName = $"{cwd}/Keras_loaded_model/python/Scripts/python";
            python.StartInfo.Arguments = $"Keras_loaded_model/keras_restore_machine_learning.py {model} {weights}";
            python.StartInfo.UseShellExecute = false;
            python.StartInfo.RedirectStandardInput = true;
            python.StartInfo.RedirectStandardOutput = true;
            python.StartInfo.RedirectStandardError = true;

            python.Start();

            python.StandardInput.Write(csvData.ToCharArray());
            python.StandardInput.Close();

            python.WaitForExit();
            csvData = python.StandardOutput.ReadToEnd();

            dynamic output = Entity.Type.Matrix.Instance.fromCSV(csvData);

            SetOutputValue("outputs", output);
        }
    }
}
