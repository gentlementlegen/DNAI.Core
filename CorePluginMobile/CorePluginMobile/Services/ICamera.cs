using System;

namespace CorePluginMobile.Services
{
    public interface ICamera
    {
        MathNet.Numerics.LinearAlgebra.Matrix<double> GetImage();

        event EventHandler<EventArgs> OnImageChange;
    }
}