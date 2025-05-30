namespace Entities.Common.Payment
{
    public class PaymentOrder
    {
        public int Id { get; set; }
        public string HppUrl { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public string Cvv2AuthStatus { get; set; }
        public string Secret { get; set; }
    }
}