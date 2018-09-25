using GalaSoft.MvvmLight;
using CorePluginLego.Model;
using GalaSoft.MvvmLight.CommandWpf;

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
                        Status = "Connecting";
                        _controller = new BrickController(new ConnectionBluetooth("COM3"));
                        await _controller.ConnectAsync();
                        Status = "Connected";
                    }));
            }
        }

        private RelayCommand _forwardCommand;

        /// <summary>
        /// Gets the MyCommand.
        /// </summary>
        public RelayCommand ForwardCommand
        {
            get
            {
                return _forwardCommand
                    ?? (_forwardCommand = new RelayCommand(
                    () =>
                    {
                        _controller.SendCommand((brick) =>
                        {
                            brick.DirectCommand.TurnMotorAtPowerForTimeAsync(Lego.Ev3.Core.OutputPort.A | Lego.Ev3.Core.OutputPort.B, 40, 100, false);
                        });
                    },
                    () => true));
            }
        }

        private string _status = "Disconnected";

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
    }
}