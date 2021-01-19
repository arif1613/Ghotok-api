using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Ghotok.Data.DataModels
{
    public class BaseDbModel
    {
        [Key]
        public Guid Id { get; set; }
        public string CountryCode { get; set; }
        [Required]
        [Phone]
        public string MobileNumber { get; set; }
        public bool LookingForBride { get; set; }
        public bool RegisterByMobileNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public Language LanguageChoice { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }
    }

    public enum Language
    {
        English,
        Bengali
    }
}
