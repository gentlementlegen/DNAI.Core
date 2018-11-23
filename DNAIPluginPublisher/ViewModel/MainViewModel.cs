using DNAIPluginPublisher.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        private CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        private CancellationToken _cancelToken;

        private readonly ObservableCollection<Item> _items = new ObservableCollection<Item>();

        public IReadOnlyList<Item> Items
        {
            get
            {
                return _provider.Items;
            }
        }

        private string _userName = Properties.Settings.Default.Username;

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                Set(ref _userName, value);
                Properties.Settings.Default.Username = value;
            }
        }

        private string _version = Properties.Settings.Default.Version;

        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                Set(ref _version, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _cancelToken = _cancelTokenSource.Token;
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
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
                        var sender = new Sender();
                        var path = Path.GetTempPath() + "DNAI.unityPackage";
                        var zipPath = Path.GetTempPath() + "DNAI.zip";

                        Task.Run(() =>
                        {
                            if (!_cancelToken.IsCancellationRequested)
                            {
                                packer.Pack(_provider.Items, Properties.Settings.Default.DirectoryPath, path);
                                //System.Diagnostics.Process.Start("explorer.exe", "/select, \"" + Path.GetTempPath() + "\"");
                            }

                            if (!_cancelToken.IsCancellationRequested)
                            {
                                Logger.Log("Preparing to send package DNAI version " + Version);
                                sender.Send(zipPath, UserName, pbox.Password, new Version(Version), () =>
                                {
                                    using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Update))
                                    {
                                        ZipArchiveEntry oldEntry = zip.GetEntry("DNAI.unityPackage");
                                        if (oldEntry != null) oldEntry.Delete();
                                        zip.CreateEntryFromFile(path, "DNAI.unityPackage");
                                    }
                                });
                            }
                        }, _cancelToken).ContinueWith((e) =>
                        {
                            if (e.IsCanceled)
                                Logger.Log("Operation cancelled.");
                            if (e.IsFaulted)
                                Logger.Log("Operation could not be completed. Reason: " + e.Exception.Message);
                            if (e.IsCompleted && !e.IsCanceled)
                                Logger.Log("Operation completed.");

                            _cancelTokenSource.Dispose();
                            _cancelTokenSource = new CancellationTokenSource();
                            _cancelToken = _cancelTokenSource.Token;

                            try
                            {
                                packer.Dispose();
                                File.Delete(zipPath);
                            } catch { }
                        });
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
                        _cancelTokenSource.Cancel();
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