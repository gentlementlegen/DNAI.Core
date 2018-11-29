using System;
using CorePluginMobile.Droid;
using CorePluginMobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(Camera))]
namespace CorePluginMobile.Droid
{
    public class Camera : ICamera
    {
        private MathNet.Numerics.LinearAlgebra.Matrix<double> _image;

        public event EventHandler<EventArgs> OnImageChange;

        public MathNet.Numerics.LinearAlgebra.Matrix<double> GetImage()
        {
            return _image;
        }

        public void SetImage(MathNet.Numerics.LinearAlgebra.Matrix<double> image)
        {
            _image = image;
            OnImageChange?.Invoke(this, EventArgs.Empty);
        }

        //public Xamarin.Forms.View GetView()
        //{
        //    return null;
        //    //return new Android.Views.TextureView(Android.App.Application.Context);
        //}
    }
}