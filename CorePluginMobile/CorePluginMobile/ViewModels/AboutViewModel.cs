using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace CorePluginMobile.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://dnai.io")));
        }

        public ICommand OpenWebCommand { get; }
    }
}