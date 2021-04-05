using System.ComponentModel.DataAnnotations;

namespace GhotokApi.Models.RequestModels
{
    public class RegisterRequestModel
    {

        [Required]
        public string Otp { get; set; }

        [Required]
        public bool IsLookingForBride { get; set; }
        public OtpRequestModel OtpRequestModel { get; set; }
    }
}
