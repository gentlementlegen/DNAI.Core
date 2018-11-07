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
		}
	}
}