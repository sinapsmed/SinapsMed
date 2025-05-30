namespace Entities.DTOs.CuponDtos.Body
{
    public class CuponUserBody
    {
        public CuponUserBody()
        {
            if (Page <= 0)
            {
                Page = 1;
            }
        }
        public int Page { get; set; }
        public string? Search { get; set; }
    }
}