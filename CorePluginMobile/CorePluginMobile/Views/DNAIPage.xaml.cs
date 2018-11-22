//using Android.Widget;
using CorePluginMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CorePluginMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DNAIPage : ContentPage
    {
        private DNAIViewModel _dnaiViewModel;

        public DNAIPage()
        {
            InitializeComponent();

            BindingContext = _dnaiViewModel = new DNAIViewModel();
            _dnaiViewModel.OnConnection += DnaiViewModel_OnConnection;
        }

        private void DnaiViewModel_OnConnection(object sender, ConnectionEventArgs e)
        {
            DependencyService.Get<Services.IToaster>().MakeText(e.Success ? "You are now connected." : "Failed to connect.");
        }
    }
}