using System;
namespace EventClient.Communication
{
    public class ValidatePackage
    {
        private readonly int _headerLength = 12;
        private readonly int _magicNumber = 0x44756c79;

        public class Header {
            public readonly int magicNumber;
            public readonly uint size;
            public readonly uint id;

            public Header(int magicNumber, uint size, uint id) {
                this.magicNumber = magicNumber;
                this.size = size;
                this.id = id;
            }
        }

        public class ReceiveEvent
        {
            public readonly string eventName;
            public readonly byte[] data;
            static public int Size = 100;

            public ReceiveEvent(string eventName, byte[] data)
            {
                this.eventName = eventName;
                this.data = data;
            }
        }

        public ValidatePackage()
        {
        }

        public bool IsHeaderPackage(byte[] data) {
            if (data.Length >= _headerLength) {
                int magicNumber = BitConverter.ToInt32(data, 0);
                uint size = BitConverter.ToUInt32(data, 4);
                uint id = BitConverter.ToUInt32(data, 8);

                if (magicNumber == _magicNumber
                    && (id >= 1 && id <= 3)) {
                    return (true);
                }
            }

            return (false);
        }

        public Header GetHeader(byte[] data)
        {
            if (data.Length >= _headerLength)
            {
                int magicNumber = BitConverter.ToInt32(data, 0);
                uint size = BitConverter.ToUInt32(data, 4);
                uint id = BitConverter.ToUInt32(data, 8);
                Header head = new Header(magicNumber, size, id);

                if (magicNumber == _magicNumber
                    && (id >= 1 && id <= 3))
                {
                    return (head);
                }
            }

            return (null);
        }

        public ReceiveEvent GetReceivePackage(Header head, byte[] data)
        {
            if (data.Length >= (head.size + _headerLength))
            {
                string name = System.Text.Encoding.UTF8.GetString(data, _headerLength, ReceiveEvent.Size).TrimEnd('\0');
                int dataSize = (int)head.size - ReceiveEvent.Size;
                if (dataSize >= 0)
                {
                    byte[] content = new byte[dataSize];
                    Buffer.BlockCopy(data, _headerLength + ReceiveEvent.Size, content, 0, dataSize);

                    ReceiveEvent receiveEvent = new ReceiveEvent(name, content);

                    return (receiveEvent);
                }
            }

            return (null);
        }

    }
}
