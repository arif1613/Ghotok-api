using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;

namespace GhotokApi.Services
{
    public interface IRegistrationService
    {
        Task<bool> IsUserRegisteredAsync(OtpRequestModel model);
        Task<AppUser> RegisterUserAsync(RegisterRequestModel model);
        Task UnregisterUserAsync(OtpRequestModel model);
    }
}