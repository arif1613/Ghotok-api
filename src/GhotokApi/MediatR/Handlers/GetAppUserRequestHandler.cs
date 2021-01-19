using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;
using GhotokApi.Utils.Authentication;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class GetAppUserRequestHandler : IRequestHandler<GetAppUserRequest, AppUser>
    {
        private readonly ILoginFlow _loginFlow;

        public GetAppUserRequestHandler(ILoginFlow loginFlow)
        {
            _loginFlow = loginFlow;
        }

        public async Task<AppUser> Handle(GetAppUserRequest request, CancellationToken cancellationToken)
        {
            return await _loginFlow.GetUserAsync(request.OtpRequestModel);
        }
    }

    public class GetAppUserRequest : IRequest<AppUser>
    {
        public OtpRequestModel OtpRequestModel { get; set; }
    }
}
