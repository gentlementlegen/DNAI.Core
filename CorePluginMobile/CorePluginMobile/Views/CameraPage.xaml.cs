using CorePluginMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CorePluginMobile.Views
{
    //https://github.com/xamarin/xamarin-forms-samples/blob/master/CustomRenderers/ContentPage/Droid/CameraPageRenderer.cs
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        private readonly CameraViewModel _cameraViewModel;

        public CameraPage()
        {
            InitializeComponent();

            BindingContext = _cameraViewModel = new CameraViewModel();
        }
    }
}