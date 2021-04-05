using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.JwtTokenGenerator;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.ResponseModels;

namespace GhotokApi.Services
{
    public class TokenService : ITokenService
    {
        public async Task<TokenResponseModel> GetToken(AppUser user, OtpRequestModel model)
        {
            return await Task.Run(() =>
            {
                var token = new JwtTokenBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Ghotok-Secret-Key"))
                    .AddSubject(model.MobileNumber)
                    .AddIssuer("Ghotok.Security.Bearer")
                    .AddAudience("Ghotok.App")
                    .AddClaim(user.UserRole, user.Id)
                    .AddRole(user.UserRole)
                    .AddExpiry(user.ValidTill)
                    .Build();

                var t= new TokenResponseModel
                {
                    Token = token.Value,
                    AppUser = user,
                    OtpRequestModel = model
                };
                return new TokenResponseModel
                {
                    Token = token.Value,
                    AppUser = user,
                    OtpRequestModel = model
                };
            });
        }
    }
}
