using CorePluginMobile.Services.API;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace CorePluginMobile.ViewModels
{
    public class DNAIViewModel : BaseViewModel
    {
        public ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>();

        public ICommand UrlCommand { get; }

        private ApiAccess _access = new ApiAccess();

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _password;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public DNAIViewModel()
        {
            UrlCommand = new Command(() => _access.DownloadSolution());

            Items.Add("AI 1");
            Items.Add("AI 2");
            Items.Add("AI 3");
        }
    }
}