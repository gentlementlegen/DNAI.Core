using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CorePluginMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModalNumberPage : ContentPage
    {
        public string TextAccuracy { get; private set; }
        public string TextNumber { get; private set; }

        public ModalNumberPage()
        {
            InitializeComponent();
        }

        public ModalNumberPage(string result, string maxOut)
        {
            TextNumber = result;
            TextAccuracy = maxOut;
            InitializeComponent();
            BindingContext = this;
        }

        public async void OnCloseModalAsync(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
    }
}