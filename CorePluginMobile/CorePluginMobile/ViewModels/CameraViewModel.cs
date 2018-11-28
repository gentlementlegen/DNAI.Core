using Xamarin.Forms;
using CorePluginMobile.Services;

// https://docs.microsoft.com/en-us/xamarin/xamarin-forms/platform/native-views/xaml
// https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/custom-renderer/contentpage
namespace CorePluginMobile.ViewModels
{
    public class CameraViewModel : BaseViewModel
    {
        public Xamarin.Forms.StackLayout Contents
        {
            get;
            set;
        }

        public CameraViewModel()
        {
            //Contents = new Xamarin.Forms.StackLayout();
            //Contents.Children.Add(DependencyService.Get<ICamera>().GetView());
        }
    }
}