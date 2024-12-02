using ProtoBuf;

namespace Verisoft.Core.Contracts
{
    [ProtoContract]
    public class GenericResponse
    {
        [ProtoMember(1)]
        public string OperationId { get; set; }

        [ProtoMember(2)]
        public string Message { get; set; }

        public static GenericResponse Ok() => new GenericResponse { Message = "OK" };
    }
}
