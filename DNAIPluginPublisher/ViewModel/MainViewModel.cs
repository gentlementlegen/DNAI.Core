using GalaSoft.MvvmLight;
using DNAIPluginPublisher.Model;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DNAIPluginPublisher.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        private ItemProvider _provider = new ItemProvider();

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        private ObservableCollection<Item> _items = new ObservableCollection<Item>();

        public IReadOnlyList<Item> Items
        {
            get
            {
                return _provider.Items;
            }
        }

        private string _userName = "";

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                Set(ref _userName, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    WelcomeTitle = item.Title;
                });
            Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;
            _provider.GetItems(Properties.Settings.Default.DirectoryPath);
            Logger.Log("DNAI Plugin Publisher version " + typeof(MainViewModel).Assembly.GetName().Version);
        }

        private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Properties.Settings.Default.DirectoryPath))
            {
                Logger.Log("Changing project location to path [" + Properties.Settings.Default.DirectoryPath + "]");
                _provider.GetItems(Properties.Settings.Default.DirectoryPath);
            }
        }

        public override void Cleanup()
        {
            // Clean up if needed

            base.Cleanup();
            Properties.Settings.Default.PropertyChanged -= Default_PropertyChanged;
            Properties.Settings.Default.Save();
        }

        private RelayCommand<PasswordBox> _sendCommand;

        /// <summary>
        /// Gets the SendCommand.
        /// </summary>
        public RelayCommand<PasswordBox> SendCommand
        {
            get
            {
                return _sendCommand
                    ?? (_sendCommand = new RelayCommand<PasswordBox>(
                    (pbox) =>
                    {
                        Logger.Log("Starting...");
                        var packer = new Packer();

                        packer.Pack(_provider.Items, Properties.Settings.Default.DirectoryPath, "DNAItest.unityPackage");
                    }, (e) => !string.IsNullOrEmpty(UserName)));
            }
        }

        private RelayCommand _cancelCommand;

        /// <summary>
        /// Gets the CancelCommand.
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return _cancelCommand
                    ?? (_cancelCommand = new RelayCommand(
                    () =>
                    {
                        Logger.Log("Cancelling current task...");
                    }));
            }
        }

        private RelayCommand _pickFolderCommand;

        /// <summary>
        /// Gets the PickFolderCommand.
        /// </summary>
        public RelayCommand PickFolderCommand
        {
            get
            {
                return _pickFolderCommand
                    ?? (_pickFolderCommand = new RelayCommand(
                    () =>
                    {
                        var dialog = new CommonOpenFileDialog
                        {
                            IsFolderPicker = true
                        };
                        var res = dialog.ShowDialog();
                        if (res == CommonFileDialogResult.Ok)
                            Properties.Settings.Default.DirectoryPath = dialog.FileName + "\\";
                    }));
            }
        }
    }
}