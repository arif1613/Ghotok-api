using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using Ghotok.Data.UnitOfWork;
using GhotokApi.Models.RequestModels;
using GhotokApi.Services;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class GetRecentUserInfosRequestHandler : IRequestHandler<GetRecentUserInfosRequest, IEnumerable<User>>
    {
        private readonly IUserService _userService;

        public GetRecentUserInfosRequestHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IEnumerable<User>> Handle(GetRecentUserInfosRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                return _userService.GetRecentUsers(
                    r => r.LookingForBride == !request.UserInfosRequestModel.LookingForBride,
                    request.UserInfosRequestModel.LookingForBride);
            }, cancellationToken);
        
        }
    }

    public class GetRecentUserInfosRequest : IRequest<IEnumerable<User>>
    {
        public UserInfosRequestModel UserInfosRequestModel { get; set; }
    }
}
