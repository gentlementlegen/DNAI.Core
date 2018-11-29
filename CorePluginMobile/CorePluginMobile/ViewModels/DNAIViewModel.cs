using CorePluginMobile.Services.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace CorePluginMobile.ViewModels
{
    public class ConnectionEventArgs : EventArgs
    {
        public bool Success = true;
    }

    public class DNAIViewModel : BaseViewModel
    {
        public ObservableCollection<Services.API.File> Items { get; set; } = new ObservableCollection<Services.API.File>();

        public ICommand UrlCommand { get; }

        public event EventHandler<ConnectionEventArgs> OnConnection;

        public ICommand ConnectCommand { get; }

        public ICommand RefreshScriptsCommand { get; }

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _password;
        private Token _token;
        private Services.API.File _selectedItem;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public Services.API.File SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public DNAIViewModel()
        {
            UrlCommand = new Command(async () =>
            {
                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), SelectedItem.Title);
                var file = await Accessor.GetFileContent(_token.user_id, SelectedItem._id);
                System.IO.File.WriteAllBytes(fileName, file);
                _binaryManager.LoadCommandsFrom(fileName);
                MessagingCenter.Send(this, "SwitchPage", 1);
            });

            ConnectCommand = new Command(async () =>
            {
                _token = await Accessor.GetToken(Name, Password);
                Accessor.SetAuthorization(_token);
                OnConnection?.Invoke(this, new ConnectionEventArgs { Success = !_token.IsEmpty() });
                RefreshScriptsCommand.Execute(null);
            });

            RefreshScriptsCommand = new Command(async () =>
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                Items.Clear();
                var files = await Accessor.GetFiles(Accessor.Token.user_id);
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        Items.Add(file);
                    }
                }
                IsBusy = false;
            });
        }
    }
}