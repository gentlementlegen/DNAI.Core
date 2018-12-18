using System;
using CNTK;

namespace CorePackageCNTK
{
    public static class CNTKHelper
    {
        private static DeviceDescriptor _device = null;

        public static DeviceDescriptor Device()
        {
            if (_device == null)
            {
                try
                {
                    _device = DeviceDescriptor.GPUDevice(0);
                }
                catch (Exception)
                {
                    _device = DeviceDescriptor.CPUDevice;
                }
            }

            return _device;
        }

        public static CNTK.Function LoadModel(string modelPath)
        {
            return modelPath.EndsWith(".onnx") ? CNTK.Function.Load(modelPath, Device(), ModelFormat.ONNX) : CNTK.Function.Load(modelPath, Device());
        }
    }
}
