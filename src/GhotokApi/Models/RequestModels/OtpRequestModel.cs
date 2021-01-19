using System.ComponentModel.DataAnnotations;

namespace GhotokApi.Models.RequestModels
{
    public class OtpRequestModel
    {
        [Required]
        public bool RegisterByMobileNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string MobileNumber { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password minimum length is 6")]
        public string Password { get; set; }

    }
}
