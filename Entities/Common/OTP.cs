namespace Entities.Common
{
    public class OTP
    {
        public DateTime Expired { get; set; }
        public string PhoneNumber { get; set; }
        public long Code { get; set; }
        public bool IsVerificated { get; set; }
    }
}