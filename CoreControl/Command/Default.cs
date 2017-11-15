namespace CoreControl.Command
{
    [ProtoBuf.ProtoContract]
    public class Default
    {
        [ProtoBuf.ProtoMember(1)]
        public Declare Declare { get { return new Declare(); } }

        [ProtoBuf.ProtoMember(2)]
        public Remove Remove { get { return new Remove(); } }
    }

    [ProtoBuf.ProtoContract]
    [ProtoBuf.ProtoInclude(500, typeof(Declare))]
    [ProtoBuf.ProtoInclude(501, typeof(Remove))]
    public class BaseAction
    {
        [ProtoBuf.ProtoMember(1)]
        public string Name { get; set; }
    }
}