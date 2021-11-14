using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NubankAuthorizer.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Violations
    {
        [EnumMember(Value = "insufficient-limit")]
        InsufficientLimit,
        [EnumMember(Value = "high-frequency-small-interval")]
        HighFrequencySmallInterval,
        [EnumMember(Value = "double-transaction")]
        DoubleTransaction,
        [EnumMember(Value = "card-not-active")]
        CardNotActive,
        [EnumMember(Value = "account-already-initialized")]
        AccountAlreadyInitialized,
        [EnumMember(Value = "account-not-initialized")]
        AccountNotInitialized
    }
}