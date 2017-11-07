using System;

namespace EventServerClient.Communication
{
    public class CreatePackage
    {
        public CreatePackage()
        {
        }

        private void initHeaderPackage(byte[] data, uint id, uint size) {
            int magicNumber = 0x44756c79;
            byte[] magicNumberBytes = BitConverter.GetBytes(magicNumber);
            byte[] idBytes = BitConverter.GetBytes(id);
            byte[] sizeBytes = BitConverter.GetBytes(size);

            Buffer.BlockCopy(magicNumberBytes, 0, data, 0, magicNumberBytes.Length);
            Buffer.BlockCopy(sizeBytes, 0, data, 4, sizeBytes.Length);
            Buffer.BlockCopy(idBytes, 0, data, 8, idBytes.Length);

        }

        public byte[] AuthenticatePackage(string clientName) {
            byte[] data = new byte[268];

            initHeaderPackage(data, 1, 256);

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(clientName);

            Buffer.BlockCopy(bytes, 0, data, 12, bytes.Length);

            return data;
        }

        public byte[] EventRegisterPackage(string eventName, uint size, bool enable)
        {
            byte[] data = new byte[273];

            initHeaderPackage(data, 2, 261);

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(eventName);
            Buffer.BlockCopy(bytes, 0, data, 12, bytes.Length);

            byte[] sizeBytes = BitConverter.GetBytes(size);
            Buffer.BlockCopy(sizeBytes, 0, data, 12 + 256, sizeBytes.Length);

            byte[] enableBytes = BitConverter.GetBytes(enable == true ? (char)1 : (char)0);
            Buffer.BlockCopy(enableBytes, 0, data, 12 + 256 + 4, 1);

            return data;
        }

        public byte[] EventSendPackage(string eventName, byte[] buffer)
        {
            byte[] data = new byte[12 + 100 + buffer.Length];

            initHeaderPackage(data, 3, (uint)(100 + buffer.Length));

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(eventName);
            Buffer.BlockCopy(bytes, 0, data, 12, bytes.Length);

            Buffer.BlockCopy(buffer, 0, data, 12 + 100, buffer.Length);

            return data;
        }

    }
}
