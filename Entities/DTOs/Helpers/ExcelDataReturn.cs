namespace Entities.DTOs.Helpers
{
    public class ExcelDataReturn<T>
        where T : class, new()
    {
        public List<Problem> Problems { get; set; }
        public List<T> Datas { get; set; }
    }
}