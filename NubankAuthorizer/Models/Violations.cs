using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NubankAuthorizer.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Violations
    {
        [EnumMember(Value = "insufficient-limit")]
        INSUFFICIENT_LIMIT,
        [EnumMember(Value = "high-frequency-small-interval")]
        HIGH_FREQUENCY_SMALL_INTERVAL,
        [EnumMember(Value = "double-transaction")]
        DOUBLE_TRANSACTION,
        [EnumMember(Value = "card-not-active")]
        CARD_NOT_ACTIVE,
        [EnumMember(Value = "account-already-initialized")]
        ACCOUNT_ALREADY_INITIALIZED,
        [EnumMember(Value = "account-not-initialized")]
        ACCOUNT_NOT_INITIALIZED
    }
}