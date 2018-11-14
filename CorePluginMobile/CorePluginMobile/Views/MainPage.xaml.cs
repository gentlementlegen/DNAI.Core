using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CorePluginMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<ViewModels.DNAIViewModel, int>(this, "SwitchPage", (sender, arg) =>
            {
                CurrentPage = Children[arg];
            });
        }
    }
}