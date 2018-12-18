using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CNTK;
using CorePackage.Global;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace CorePackageCNTK
{
    public class Predictor : IPredictor
    {
        private static string LastModelLoaded { get; set; }
        private static CNTK.Function Model { get; set; }

        public static void InitPredictor()
        {
            CorePackage.Execution.Predict.PredictorInstance = new Predictor();
        }

        public void LoadModel(string path)
        {
            path = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + ".dnn";
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Model file not found for prediction: {path}");
            }

            if (path.Equals(LastModelLoaded)) return;
            try
            {
                Model = CNTKHelper.LoadModel(path);
                LastModelLoaded = path;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}\nCallStack: {1}\n Inner Exception: {2}", ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.Message : "No Inner Exception");
                throw ex;
            }
        }

        public Matrix<double> Predict(Matrix<double> inputs)
        {
            if (Model == null) return new DenseMatrix(0, 0);

            CNTK.Variable inputVar = Model.Arguments.Single();

            double[] original = inputs.ToRowMajorArray();
            for (int i = 0; i < original.Length; i++)
            {
                if (i != 0 && i % 28 == 0)
                    Console.Write("\n");
                Console.Write((int)(original[i] * 10) + ",");
            }
            List<float> converted = original.Select(o => (float)o).ToList();

            NDShape inputShape = inputVar.Shape;
            var inputDataMap = new Dictionary<CNTK.Variable, Value>();
            var inputVal = Value.CreateBatch(inputShape, converted, CNTKHelper.Device());
            inputDataMap.Add(inputVar, inputVal);

            CNTK.Variable outputVar = Model.Output;

            // Create output data map. Using null as Value to indicate using system allocated memory.
            // Alternatively, create a Value object and add it to the data map.
            var outputDataMap = new Dictionary<CNTK.Variable, Value>();
            outputDataMap.Add(outputVar, null);

            // Start evaluation on the device
            Model.Evaluate(inputDataMap, outputDataMap, CNTKHelper.Device());

            // Get evaluate result as dense output
            var outputVal = outputDataMap[outputVar];
            var outputData = outputVal.GetDenseData<float>(outputVar);

            double[,] arr = new double[outputData.Count, outputData[0].Count];
            for (int i = 0; i < outputData.Count; i++)
            {
                for (int j = 0; j < outputData[i].Count; j++)
                {
                    arr[i, j] = outputData[i][j];
                }
            }
            return DenseMatrix.OfArray(arr);
        }
    }
}
