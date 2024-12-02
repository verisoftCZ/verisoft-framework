using ProtoBuf;

namespace Verisoft.Core.Contracts
{
    [ProtoContract]
    public class FailureDetail
    {
        [ProtoMember(1)]
        public string OperationId { get; set; }

        [ProtoMember(2)]
        public string ErrorMessage { get; set; }

        [ProtoMember(3)]
        public bool IsBusinessException { get; set; }

        public static FailureDetail AsInternalServerError(string id)
            => new FailureDetail { OperationId = id, ErrorMessage = "Unexpected internal error", IsBusinessException = false };

        public static FailureDetail AsBusinessException(string id, string businessMessage)
            => new FailureDetail { OperationId = id, ErrorMessage = businessMessage, IsBusinessException = true };
    }
}
