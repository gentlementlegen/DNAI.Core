using Xamarin.Forms;
using CorePluginMobile.Services;
using CoreCommand;
using System.Collections.Generic;

// https://docs.microsoft.com/en-us/xamarin/xamarin-forms/platform/native-views/xaml
// https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/custom-renderer/contentpage
namespace CorePluginMobile.ViewModels
{
    public class CameraViewModel : BaseViewModel
    {
        public CameraViewModel()
        {
            //Contents = new Xamarin.Forms.StackLayout();
            //Contents.Children.Add(DependencyService.Get<ICamera>().GetView());
            DependencyService.Get<ICamera>().OnImageChange += CameraViewModel_OnImageChange;
        }

        private void CameraViewModel_OnImageChange(object sender, System.EventArgs e)
        {
            var image = (sender as ICamera).GetImage();
            var generated_script_execution_results = new Dictionary<string, dynamic>();

            generated_script_execution_results = _binaryManager.Controller.CallFunction(11, new Dictionary<string, dynamic> { { "pixels", (MathNet.Numerics.LinearAlgebra.Double.DenseMatrix)image }, });
            var result = generated_script_execution_results["result"];
            var maxOut = generated_script_execution_results["maxOut"];
            var results = generated_script_execution_results["results"];
            DependencyService.Get<IToaster>().MakeText($"Number found: [{result}] ({maxOut}%)");
        }
    }
}