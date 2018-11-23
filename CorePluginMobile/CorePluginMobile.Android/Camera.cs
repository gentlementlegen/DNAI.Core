using CorePluginMobile.Droid;
using CorePluginMobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(Camera))]
namespace CorePluginMobile.Droid
{
    public class Camera : ICamera
    {
        public Xamarin.Forms.View GetView()
        {
            return null;
            //return new Android.Views.TextureView(Android.App.Application.Context);
        }
    }
}