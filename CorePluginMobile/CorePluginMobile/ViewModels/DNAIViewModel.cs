using Android.Widget;
using CorePluginMobile.Services.API;
using System;
using System.Collections.ObjectModel;
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
        public ObservableCollection<File> Items { get; set; } = new ObservableCollection<File>();

        public ICommand UrlCommand { get; }

        public event EventHandler<ConnectionEventArgs> OnConnection;

        public ICommand ConnectCommand
        {
            get => _urlCommand
                ?? (_urlCommand = new Command(async () =>
                                              {
                                                  var token = await Accessor.GetToken(Name, Password);
                                                  Accessor.SetAuthorization(token);
                                                  OnConnection?.Invoke(this, new ConnectionEventArgs { Success = !token.IsEmpty() });
                                                  RefreshScriptsCommand.Execute(null);
                                              }));
        }

        public ICommand RefreshScriptsCommand
        {
            get => _refreshScriptCommand
                ?? (_refreshScriptCommand = new Command(async () =>
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
                }));
        }

        private Command _urlCommand;

        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _password;
        private ICommand _refreshScriptCommand;

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public DNAIViewModel()
        {
            UrlCommand = new Command(() =>
            {
                MessagingCenter.Send(this, "SwitchPage", 1);
                });
            //Items.Add(new File { Title = "toto " });
        }
    }
}