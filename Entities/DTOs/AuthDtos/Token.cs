using Core.Entities.DTOs;

namespace Entities.DTOs.AuthDtos
{
    public class Token : IDto
    {
        public string JWT { get; set; }
    }

    public class TokenPair : IDto
    {
        public string AccessToken { get; set; }
        public string RefresToken { get; set; }
    }
}