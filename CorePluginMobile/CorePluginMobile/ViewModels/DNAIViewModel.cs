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
                AI ai = new AI();
                ai.minDistance = 15;
                ai.speed = 100;
                ai.UpdateDirection(ai, 45, 45);
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

        public class AI
        {
            public float X;
            public float Y;
            public float Z;
            public float minDistance;
            public float speed;

            public void UpdateDirection(AI @this, float @leftDistance, float @rightDistance)
            {
                _binaryManager.Controller.CallFunction(8, new Dictionary<string, dynamic> { { "this", (AI)@this }, { "leftDistance", (float)@leftDistance }, { "rightDistance", (float)@rightDistance }, });
            }
        }
    }
}