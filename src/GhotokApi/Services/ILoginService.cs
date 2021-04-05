using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;

namespace GhotokApi.Services
{
    public interface ILoginService
    {
        Task<bool> IsUserLoggedInAsync(OtpRequestModel model);
        Task<bool> IsUserLoggedOutAsync(OtpRequestModel model);
        Task<AppUser> LogInUserAsync(OtpRequestModel model);
        Task<AppUser> LogOutUserAsync(OtpRequestModel model);
    }
}