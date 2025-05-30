using System.ComponentModel.DataAnnotations;
using Core.Entities.DTOs;

namespace Entities.DTOs.AuthDtos
{
    public class Register : IDto
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Mail { get; set; }
        public string PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public bool Gender { get; set; }
    }
}