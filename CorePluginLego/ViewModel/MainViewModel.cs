using GalaSoft.MvvmLight;
using CorePluginLego.Model;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;

namespace CorePluginLego.ViewModel
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
        private readonly IConnection _connection;

        private BrickController _controller;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _connection = new ConnectionBluetooth();

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
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
        ///

        private RelayCommand _connectCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand ConnectCommand
        {
            get
            {
                return _connectCommand
                    ?? (_connectCommand = new RelayCommand(
                    async () =>
                    {
                        if (_controller == null || !_controller.IsConnected)
                        {
                            Status = "Connecting...";
                            _controller = new BrickController(new ConnectionBluetooth(ComPort));
                            await _controller.ConnectAsync();
                            Status = "Disconnect";
                        }
                        else
                        {
                            _controller.Dispose();
                            Status = "Connect";
                        }
                    }));
            }
        }

        private RelayCommand<int> _moveCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand<int> MoveCommand
        {
            get
            {
                return _moveCommand
                    ?? (_moveCommand = new RelayCommand<int>(
                    (dir) =>
                    {
                        AutoPilot = false;
                        _controller.SendCommand((brick) =>
                        {
                            brick.DirectCommand.TurnMotorAtPowerForTimeAsync(Lego.Ev3.Core.OutputPort.A | Lego.Ev3.Core.OutputPort.B, Velocity * dir, 100, false);
                        });
                    }, (dir) => _controller?.IsConnected == true));
            }
        }

        private RelayCommand<int> _turnCommand;

        /// <summary>
        /// Gets the TurnCommand.
        /// </summary>
        public RelayCommand<int> TurnCommand
        {
            get
            {
                return _turnCommand
                    ?? (_turnCommand = new RelayCommand<int>(
                    (dir) =>
                    {
                        AutoPilot = false;
                        _controller.SendCommand((brick) =>
                        {
                            brick.BatchCommand.TurnMotorAtPowerForTime(Lego.Ev3.Core.OutputPort.A | Lego.Ev3.Core.OutputPort.B, Velocity * dir, 100, false);
                            brick.BatchCommand.TurnMotorAtPowerForTime(Lego.Ev3.Core.OutputPort.A | Lego.Ev3.Core.OutputPort.B, Velocity * -dir, 100, false);
                        });
                    },
                    (dir) => _controller?.IsConnected == true));
            }
        }

        private RelayCommand _pickFileCommand;

        /// <summary>
        /// Gets the PickFileCommand.
        /// </summary>
        public RelayCommand PickFileCommand
        {
            get
            {
                return _pickFileCommand
                    ?? (_pickFileCommand = new RelayCommand(
                    () =>
                    {
                        var dialog = new System.Windows.Forms.OpenFileDialog();
                        switch (dialog.ShowDialog())
                        {
                            case System.Windows.Forms.DialogResult.OK:
                                Path = dialog.FileName;
                                break;
                            case System.Windows.Forms.DialogResult.Cancel:
                                break;
                            default:
                                break;
                        }
                    }));
            }
        }

        private string _status = "Connect";

        /// <summary>
        /// Sets and gets the Status property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                Set(ref _status, value);
            }
        }

        private bool _autoPilot;

        public bool AutoPilot
        {
            get => _autoPilot;
            set
            {
                Set(ref _autoPilot, value);
                if (value)
                    _controller.StartAutoPilot(Path);
                else
                    _controller.StopAutopilot();
            }
        }

        private string _comPort = "COM3";

        public string ComPort
        { get => _comPort; set => Set(ref _comPort, value); }

        private int _velocity = 40;

        public int Velocity
        {
            get => _velocity;
            set
            {
                if (Set(ref _velocity, value) && _controller != null)
                    _controller.Velocity = value;
            }
        }

        private string _path;

        public string Path
        { get => _path; set => Set(ref _path, value); }

        private float _minDistance = 10f;

        public float MinDistance
        {
            get => _minDistance;
            set
            {
                if (Set(ref _minDistance, value) && _controller != null)
                    _controller.MinDistance = value;
            }
        }

        public string Distance => _controller?.Distance.ToString();
    }
}