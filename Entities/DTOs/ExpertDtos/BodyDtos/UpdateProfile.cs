namespace Entities.DTOs.ExpertDtos.BodyDtos
{
    public class UpdateProfile
    {
        public string Specality { get; set; }
        public string Resume { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public string PhotoUrl { get; set; }
        public string FullName { get; set; }
    }
}