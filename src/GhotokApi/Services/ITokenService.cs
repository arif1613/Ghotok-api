using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.ResponseModels;

namespace GhotokApi.Services
{
    public interface ITokenService
    {
        Task<TokenResponseModel> GetToken(AppUser user, OtpRequestModel model);
    }
}