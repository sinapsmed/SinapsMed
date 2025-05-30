using Entities.Enums;

namespace Entities.DTOs.CuponDtos.Body
{
    public class CuponServiceBody
    {
        public CuponServiceBody()
        {
            if (Page <= 0)
            {
                Page = 1;
            }
        }
        public CuponType Type { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; }
    }
}