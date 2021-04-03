using System.Collections.Generic;
using System.Linq;
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
    public class GetUserInfosRequestHandler : IRequestHandler<GetUserInfosRequest, IEnumerable<User>>
    {
        private readonly IUserService _userService;

        public GetUserInfosRequestHandler(IUserService userService)
        {
            _userService = userService;
        }


        public async Task<IEnumerable<User>> Handle(GetUserInfosRequest request, CancellationToken cancellationToken)
        {
            return await _userService.GetUsers(r => r.LookingForBride == !request.UserInfosRequestModel.LookingForBride && r.IsPublished,
                    request.UserInfosRequestModel.IsPublished,
                    request.UserInfosRequestModel.HasOrderBy,request.UserInfosRequestModel.HasInclude,
                    request.UserInfosRequestModel.LookingForBride,request.UserInfosRequestModel.StartIndex, request.UserInfosRequestModel.ChunkSize);
        }
    }

    public class GetUserInfosRequest : IRequest<IEnumerable<User>>
    {
        public UserInfosRequestModel UserInfosRequestModel { get; set; }
    }
}
