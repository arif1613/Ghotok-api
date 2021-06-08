using System.ComponentModel.DataAnnotations;

namespace GhotokApi.Models.RequestModels
{
    public class OtpRequestModel
    {
        private string _mobileNumber;
        private string _email;
        private string _countrycode;

        [Required]
        public bool RegisterByMobileNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email {
            get => _email;
            set { _email = !RegisterByMobileNumber ? value : "myemail@ghotok.com"; }
        }

        [Required]
        [Phone]
        public string MobileNumber
        {
            get => _mobileNumber;
            set { _mobileNumber = RegisterByMobileNumber ? value : "0000000000"; }
        }
        [Required]
        public string CountryCode
        {
            get => _countrycode;
            set { _countrycode = RegisterByMobileNumber ? value : "+00"; }
        }
        [Required]
        [MinLength(6, ErrorMessage = "Password minimum length is 6")]
        public string Password { get; set; }

    }
}
