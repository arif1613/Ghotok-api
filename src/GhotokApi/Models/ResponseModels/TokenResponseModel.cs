using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;

namespace GhotokApi.Models.ResponseModels
{
    public class TokenResponseModel
    {
        public AppUser AppUser { get; set; }
        public OtpRequestModel OtpRequestModel { get; set; }
        public string Token { get; set; }
     
    }
}
