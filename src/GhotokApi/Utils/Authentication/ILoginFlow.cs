using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.ResponseModels;

namespace GhotokApi.Utils.Authentication
{
    public interface ILoginFlow
    {
        Task<bool> IsUserRegisteredAsync(OtpRequestModel model);
        Task<OtpResponseModel> GetOtpAsync(OtpRequestModel model);

        Task<bool> IsUserLoggedInAsync(OtpRequestModel model);
        
        Task<bool> IsOtpValidAsync(RegisterRequestModel model);

        Task RegisterUserAsync(AppUser user);
        Task UnregisterUserAsync(OtpRequestModel model);

        Task<TokenResponseModel> LogInUserAsync(OtpRequestModel model);

        Task LogOutUserAsync(OtpRequestModel model);
        Task<AppUser> GetUserAsync(OtpRequestModel model);
        TokenResponseModel GetToken(AppUser user, OtpRequestModel model);
    }
}