using CorePluginMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CorePluginMobile.Views
{
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