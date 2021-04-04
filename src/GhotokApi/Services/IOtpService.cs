using System.Threading.Tasks;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.ResponseModels;

namespace GhotokApi.Services
{
    public interface IOtpService
    {
        Task<OtpResponseModel> GetOtpAsync(OtpRequestModel model);
        Task<bool> IsOtpValidAsync(RegisterRequestModel model);
    }
}