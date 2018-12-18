using MathNet.Numerics.LinearAlgebra;

namespace CorePackage.Global
{
    public  interface IPredictor
    {
        void LoadModel(string path);
        Matrix<double> Predict(Matrix<double> inputs);

    }
}
