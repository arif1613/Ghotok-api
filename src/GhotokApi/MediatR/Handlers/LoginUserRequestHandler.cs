using System.Threading;
using System.Threading.Tasks;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.ResponseModels;
using GhotokApi.Utils.Authentication;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class LoginUserRequestHandler:IRequestHandler<LoginUserRequest,TokenResponseModel>
    {
        private readonly ILoginFlow _loginFlow;

        public LoginUserRequestHandler(ILoginFlow loginFlow)
        {
            _loginFlow = loginFlow;
        }

        public async Task<TokenResponseModel> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            return await _loginFlow.LogInUserAsync(request.OtpRequestModel);
        }
    }

    public class LoginUserRequest:IRequest<TokenResponseModel>
    {
        public OtpRequestModel OtpRequestModel { get; set; }
    }
}
