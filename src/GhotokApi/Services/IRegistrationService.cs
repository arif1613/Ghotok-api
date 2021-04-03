using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;

namespace GhotokApi.Services
{
    public interface IRegistrationService
    {
        Task<bool> IsUserRegisteredAsync(OtpRequestModel model);
        Task RegisterUserAsync(AppUser user);
        Task UnregisterUserAsync(OtpRequestModel model);
    }
}