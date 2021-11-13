namespace NubankAuthorizer.Models
{
    public enum Violations
    {
        INSUFFICIENT_LIMIT,
        HIGH_FREQUENCY_SMALL_INTERVAL,
        DOUBLE_TRANSACTION,
        CARD_NOT_ACTIVE,
        ACCOUNT_ALREADY_INITIALIZED,
        ACCOUNT_NOT_INITIALIZED
    }
}