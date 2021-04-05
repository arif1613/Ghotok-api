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
            get
            {
                return _email;
            }
            set
            {
                if (!RegisterByMobileNumber)
                {
                    _email = value;
                }
                else
                {
                    _email = "myemail@ghotok.com";
                }
            }
        }

        [Required]
        [Phone]
        public string MobileNumber
        {
            get
            {
                return _mobileNumber;
            }
            set
            {
                if (RegisterByMobileNumber)
                {
                    _mobileNumber = value;
                }
                else
                {
                    _mobileNumber = "0000000000";
                }
            }
        }
        [Required]
        public string CountryCode
        {
            get
            {
                return _countrycode;
            }
            set
            {
                if (RegisterByMobileNumber)
                {
                    _countrycode = value;
                }
                else
                {
                    _countrycode = "+00";
                }
            }
        }
        [Required]
        [MinLength(6, ErrorMessage = "Password minimum length is 6")]
        public string Password { get; set; }

    }
}
