namespace Entities.DTOs.AuthDtos
{
    public class UserInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string ImageUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string UnicalKey { get; set; }
        public bool Gender { get; set; }
        public DateTime? DateOfBrith { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}