using Core.Entities.DTOs;

namespace Entities.DTOs.CuponDtos
{
    public class Update : IDto
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime Start { get; set; }
        public DateTime Expired { get; set; }
        public byte Discount { get; set; }
        //Bu totalda nece istifade olacagin deyir 
        public int UseLimit { get; set; }
        //bu ise sadece melumatlandirmaq ucun yaza bilersen ki hazirda necedene qalib ki istifade oluna biler
        //burda istifadeci eger UseLimiti update elese bunnan cox ola bilmez 
        public int UseableCount { get; set; }
    }
}