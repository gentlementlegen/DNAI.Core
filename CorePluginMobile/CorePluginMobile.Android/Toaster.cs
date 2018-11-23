using Android.Widget;
using CorePluginMobile.Droid;
using CorePluginMobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(Toaster))]
namespace CorePluginMobile.Droid
{
    public class Toaster : IToaster
    {
        public void MakeText(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
    }
}