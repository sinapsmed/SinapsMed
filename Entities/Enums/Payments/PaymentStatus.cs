namespace Entities.Enums.Payments
{
    public enum PaymentStatus : byte
    {
        BeingPrepared,
        Cancelled,
        Rejected,
        Refused,
        Expired,
        Authorized,
        PartiallyPaid,
        FullyPaid,
        Funded,
        Declined,
        Voided,
        Refunded,
        Closed,
        Pending,
    }
}