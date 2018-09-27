using Lego.Ev3.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace CorePluginLego.Model
{
    public class BrickController : IDisposable
    {
        public bool IsConnected { get; private set; }
        public float Distance { get; private set; } = 100;

        public float Velocity = 40;
        public float MinDistance = 10;

        private readonly IConnection _connection;
        private Brick _brick;
        private readonly BackgroundWorker _backgroundWorker;
        private bool _isAutoPilot;
        private static readonly CoreCommand.BinaryManager _manager = new CoreCommand.BinaryManager();
        private readonly AI _ai = new AI();

        public BrickController(IConnection connection)
        {
            _connection = connection;

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += Bw_DoWork;
            _backgroundWorker.RunWorkerCompleted += Bw_RunWorkerCompleted;
            _backgroundWorker.ProgressChanged += Bw_ProgressChanged;
        }

        public async System.Threading.Tasks.Task ConnectAsync()
        {
            _brick = await _connection.Connect();
            _brick.BrickChanged += Brick_BrickChanged;
            IsConnected = true;
        }

        private void Brick_BrickChanged(object sender, BrickChangedEventArgs e)
        {
            Console.WriteLine("brick changed");
            Distance = e.Ports[InputPort.Four].SIValue;
        }

        public void SendCommand(Action<Brick> action)
        {
            action?.Invoke(_brick);
        }

        public void Dispose()
        {
            _connection?.Dispose();
            IsConnected = false;
            _backgroundWorker.Dispose();
        }

        public void StartAutoPilot(string aiPath)
        {
            if (_isAutoPilot || _backgroundWorker?.CancellationPending == true)
                return;

            _isAutoPilot = true;
            _manager.Reset();
            _manager.LoadCommandsFrom(aiPath);
            _backgroundWorker.RunWorkerAsync();
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine("progress changed");
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("completed");
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            while (_isAutoPilot)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }
                _ai.minDistance = MinDistance;
                _ai.speed = Velocity;
                _ai.UpdateDirection(_ai, Distance, Distance);
                Console.WriteLine(_ai.X + " Y" + _ai.Y + " Z" + _ai.Z + " dist" + _ai.minDistance + " speed" + _ai.speed);
                Thread.Sleep(1);
            }
        }

        public void StopAutopilot()
        {
            _isAutoPilot = false;
            _backgroundWorker.CancelAsync();
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
                _manager.Controller.CallFunction(8, new Dictionary<string, dynamic> { { "this", (AI)@this }, { "leftDistance", (float)@leftDistance }, { "rightDistance", (float)@rightDistance }, });
            }
        }
    }
}