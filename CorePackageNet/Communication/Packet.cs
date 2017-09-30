using ProtoBuf;

namespace CorePackageNet.Communication
{
    /// <summary>
    /// Packet class contract for Protobuf usage.
    /// </summary>
    [ProtoContract]
    public class PacketClientAuth
    {
        // 0x44756c79
        [ProtoMember(1)]
        public int MagicNumber { get; set; }

        [ProtoMember(2)]
        public uint PacketSize { get; set; }

        [ProtoMember(3)]
        public uint Id { get; set; }

        [ProtoMember(4)]
        public string ClientName { get; set; }
    }

    /// <summary>
    /// Packet class contract for Protobuf usage.
    /// </summary>
    [ProtoContract]
    public class PacketRegisterEventRequest
    {
        // 0x44756c79
        [ProtoMember(1)]
        public int MagicNumber { get; set; }

        [ProtoMember(2)]
        public uint PacketSize { get; set; }

        [ProtoMember(3)]
        public uint Id { get; set; }

        [ProtoMember(4)]
        public string EventName { get; set; }

        [ProtoMember(5)]
        public uint EventSize { get; set; }

        [ProtoMember(6)]
        public bool Enable { get; set; }
    }


    /// <summary>
    /// Packet class contract for Protobuf usage.
    /// </summary>
    [ProtoContract]
    public class PacketRegisterEventResponse
    {
        // 0x44756c79
        [ProtoMember(1)]
        public int MagicNumber { get; set; }

        [ProtoMember(2)]
        public uint PacketSize { get; set; }

        [ProtoMember(3)]
        public uint Id { get; set; }

        [ProtoMember(4)]
        public string EventName { get; set; }

        [ProtoMember(5)]
        public uint EventId { get; set; }
    }

    /// <summary>
    /// Packet class contract for Protobuf usage.
    /// </summary>
    [ProtoContract]
    public class PacketSendEventRequest
    {
        // 0x44756c79
        [ProtoMember(1)]
        public int MagicNumber { get; set; }

        [ProtoMember(2)]
        public uint PacketSize { get; set; }

        [ProtoMember(3)]
        public uint Id { get; set; }

        [ProtoMember(4)]
        public uint EventId { get; set; }

        [ProtoMember(4)]
        public byte[] Data { get; set; }
    }

    /// <summary>
    /// Packet class contract for Protobuf usage.
    /// </summary>
    [ProtoContract]
    public class PacketSendEventResponse
    {
        // 0x44756c79
        [ProtoMember(1)]
        public int MagicNumber { get; set; }

        [ProtoMember(2)]
        public uint PacketSize { get; set; }

        [ProtoMember(3)]
        public uint Id { get; set; }

        [ProtoMember(4)]
        public uint EventId { get; set; }

        [ProtoMember(4)]
        public byte[] Data { get; set; }
    }
}