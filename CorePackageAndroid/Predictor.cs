using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CorePackage.Global;
using MathNet.Numerics.LinearAlgebra;
using Org.Tensorflow;
using Org.Tensorflow.Contrib.Android;
using CorePackage.Entity.Type;
using Java.Lang;
using MathNet.Numerics.LinearAlgebra.Double;
using Exception = System.Exception;

namespace CorePackageAndroid
{
    public class Predictor : IPredictor
    {
        private static string LastModelLoaded;
        private List<string> _labels = new List<string>()
        {
            "0", "1", "2",
            "3", "4", "5",
            "6", "7", "8",
            "9"

        };
        private TensorFlowInferenceInterface _inferenceInterface;
        private bool _hasNormalizationLayer;

        private int _inputSize;
        private string _inputName;
        private string _outputName;
        private int _lastDim;
        private const string DataNormLayerPrefix = "data_bn";

        public static void InitPredictor()
        {
            CorePackage.Execution.Predict.PredictorInstance = new Predictor();
        }

        public void LoadModel(string path)
        {
            var name = Path.GetFileNameWithoutExtension(path);
            path = name + ".pb";
            if (path.Equals(LastModelLoaded)) return;

            var info = CorePackage.Entity.Type.Resource.Instance.Directory + name + ".txt";
            List<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(info))
            {
                while (sr.Peek() >= 0)
                {
                    lines.Add(sr.ReadLine());
                }
            }

            _inputName = lines[0];
            _outputName = lines[1];
            var shape = lines[2].Replace(" ", "").Replace("(", "").Replace(")", "").Split(",");
            _inputSize = int.Parse(shape[1]);
            _lastDim = int.Parse(shape[3]);
            CorePackage.Entity.Type.Resource.Instance.Directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/";
            var modelFile = File.Open(CorePackage.Entity.Type.Resource.Instance.Directory + path, FileMode.Open);
            
            try
            {
                //var assets = Android.App.Application.Context.Assets;
                //using (var sr = new StreamReader(assets.Open(labelsFileName)))
                //{
                //    var content = sr.ReadToEnd();
                //    _labels = content.Split('\n').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();
                //}

                _inferenceInterface = new TensorFlowInferenceInterface(modelFile);
                LastModelLoaded = path;
                var iter = _inferenceInterface.Graph().Operations();
                while (iter.HasNext && !_hasNormalizationLayer)
                {
                    var op = iter.Next() as Operation;
                    if (op.Name().Contains(DataNormLayerPrefix))
                    {
                        _hasNormalizationLayer = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new RuntimeException("Failed to load the model - check the inner exception for more details" + ex.Message);
            }
        }

        public Matrix<double> Predict(Matrix<double> inputs)
        {
            float[] original = inputs.ToRowMajorArray().Select(x => (float)x).ToArray();

            var outputNames = new[] { _outputName };
            var outputs = new float[_labels.Count];

            _inferenceInterface.Feed(_inputName, original, 1, _inputSize, _inputSize, _lastDim);
            _inferenceInterface.Run(outputNames);
            _inferenceInterface.Fetch(_outputName, outputs);

            double[,] arr = new double[1, outputs.Length];
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < outputs.Length; j++)
                {
                    arr[i, j] = outputs[j];
                }
            }
            return DenseMatrix.OfArray(arr);
        }
    }
}
