using MathNet.Numerics.LinearAlgebra;
using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace CorePackage.Global
{
    public static class KerasService
    {
        private static char EOT { get; } = (char)4;

        private static char LF { get; } = '\n';

        private static string AssemblyPath { get; } = Path.GetDirectoryName(typeof(KerasService).Assembly.Location);

        private static bool IsProcessRunning { get; set; } = false;

        private static string PythonProgram { get; } = $"{AssemblyPath}/.Keras_loaded_model/python/Scripts/python";
        private static string KerasScript { get; } = $"{AssemblyPath}/.Keras_loaded_model/keras_restore_machine_learning.py";

        private static Process PythonProcess { get; } = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = PythonProgram,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            }
        };

        private static TcpListener Server { get; set; }

        private static TcpClient Client { get; set; }

        private static StreamReader Output { get; set; }

        private static StreamWriter Input { get; set; }

        private static Task ProcessThread { get; set; }

        private static Queue<string> OutputLines { get; } = new Queue<string>();

        private static Queue<string> ErrorLines { get; } = new Queue<string>();

        private static string LastModelLoaded { get; set; }

        private static string LastWeightsLoaded { get; set; }

        public static void Init()
        {
            if (!IsProcessRunning && File.Exists(KerasScript))
            {
                IsProcessRunning = true;

                Server = new TcpListener(IPAddress.Loopback, 0);
                Server.Start();

                int port = ((IPEndPoint)Server.LocalEndpoint).Port;

                Debug.WriteLine($"Running server on port {port}");

                PythonProcess.StartInfo.Arguments = $"\"{KerasScript}\" -p {port}";
                PythonProcess.Start();

                Client = Server.AcceptTcpClient();
                Input = new StreamWriter(Client.GetStream());
                Input.AutoFlush = false;
                Output = new StreamReader(Client.GetStream());

                ProcessThread = Task
                    .Run(() =>
                    {
                        PythonProcess.WaitForExit();
                        Server.Stop();
                    })
                    .ContinueWith(t => IsProcessRunning = false);
            }
        }

        private static string GetData()
        {
            string line = Output.ReadLine();

            if (line.Contains("ERROR: "))
            {
                throw new InvalidOperationException(line);
            }

            return line;
        }

        private static string GetOutputLine()
        {
            return GetData();
        }

        private static string GetError()
        {
            return GetData();
        }

        private static void SendCommand(string command, params string[] args)
        {
            Debug.WriteLine(command);
            Input.WriteLine(command);
            foreach (string arg in args)
            {
                Debug.WriteLine(arg);
                Input.WriteLine(arg);
            }
            Input.Flush();
        }

        public static void LoadModel(string model)
        {
            string path = $"{Entity.Type.Ressource.Instance.Directory}/{model}";

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Model file not found for prediction: {path}");
            }

            if (!path.Equals(LastModelLoaded))
            {
                SendCommand("LOAD_MODEL", path);
            }
        }

        public static void LoadWeights(string weights)
        {
            string path = $"{Entity.Type.Ressource.Instance.Directory}/{weights}";

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Weights file not found for prediction: {path}");
            }

            if (!path.Equals(LastWeightsLoaded))
            {
                SendCommand("LOAD_WEIGHTS", path);
            }
        }

        public static Matrix<double> Predict(Matrix<double> inputs, string shape)
        {
            string csvData = Entity.Type.Matrix.Instance.toCSV(inputs);

            SendCommand("PREDICT", $"{inputs.RowCount}", $"{inputs.ColumnCount}", shape, csvData);

            int resultCount = Int32.Parse(GetOutputLine());
            StringBuilder matrixBuilder = new StringBuilder();

            for (int i = 0; i < resultCount; i++)
            {
                matrixBuilder.Append((i == 0 ? "" : "\n") + GetOutputLine());
            }

            return Entity.Type.Matrix.Instance.fromCSV(matrixBuilder.ToString());
        }

        public static void Quit()
        {
            SendCommand("QUIT");
        }
    }
}
