namespace Entities.DTOs.CuponDtos
{
    public class Detail : Get
    {
        public DateTime StartAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public int UseLimitForPerUser { get; set; }
        //Default 10 dene gelir geri qalani pagenation ile /UsedCupons pathinden
        public ICollection<UsedCupon> UsedCupons { get; set; } = new List<UsedCupon>();
        //Hamisi gelir
        public ICollection<CuponUser> CuponUsers { get; set; } = new List<CuponUser>();
        //Hamisin qaytaracam
        public List<object> Services { get; set; } = new List<object>();
    }
}