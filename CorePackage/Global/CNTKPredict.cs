using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CNTK;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace CorePackage.Global
{
    public static  class CNTKPredict
    {
        private static string LastModelLoaded { get; set; }
        private static Function Model { get; set; }
        private static DeviceDescriptor _device = DeviceDescriptor.GPUDevice(0);

        public static void LoadModel(string model)
        {
            string path = $"{Entity.Type.Resource.Instance.Directory}/{model}";

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Model file not found for prediction: {path}");
            }

            if (path.Equals(LastModelLoaded)) return;
            try
            {
                Model = Function.Load(path, _device);
                LastModelLoaded = path;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}\nCallStack: {1}\n Inner Exception: {2}", ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.Message : "No Inner Exception");
                throw ex;
            }
        }

        public static Matrix<double> Predict(Matrix<double> inputs, string shape)
        {
            if (Model == null) return new DenseMatrix(0, 0);

            Variable inputVar = Model.Arguments.Single();

            double[] original = inputs.ToRowMajorArray();
            for (int i = 0; i < original.Length; i++)
            {
                if (i != 0 && i % 28 == 0)
                    Console.Write("\n");
                Console.Write((int)(original[i] * 10) + ",");
            }
            List<float> converted = original.Select(o => (float) o).ToList();

            NDShape inputShape = inputVar.Shape;
            var inputDataMap = new Dictionary<Variable, Value>();
            var inputVal = Value.CreateBatch(inputShape, converted, _device);
            inputDataMap.Add(inputVar, inputVal);

            Variable outputVar = Model.Output;

            // Create output data map. Using null as Value to indicate using system allocated memory.
            // Alternatively, create a Value object and add it to the data map.
            var outputDataMap = new Dictionary<Variable, Value>();
            outputDataMap.Add(outputVar, null);

            // Start evaluation on the device
            Model.Evaluate(inputDataMap, outputDataMap, _device);

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
