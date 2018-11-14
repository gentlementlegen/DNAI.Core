using Android.Widget;
using CorePluginMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CorePluginMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DNAIPage : ContentPage
	{
        DNAIViewModel _dnaiViewModel;

		public DNAIPage ()
		{
			InitializeComponent ();

            BindingContext = _dnaiViewModel = new DNAIViewModel();
            _dnaiViewModel.OnConnection += DnaiViewModel_OnConnection;
		}

        private void DnaiViewModel_OnConnection(object sender, ConnectionEventArgs e)
        {
            // Hack : this will break the iOS compilation. Move that to android folders
            Toast.MakeText(Android.App.Application.Context, e.Success ? "You are now connected." : "Failed to connect.", ToastLength.Long).Show();
        }
    }
}