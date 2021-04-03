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
    public class GetUserInfoRequestHandler : IRequestHandler<GetUserInfoRequest, User>
    {
        private readonly IUserService _userService;

        public GetUserInfoRequestHandler(IUserService userService)
        {
            _userService = userService;
        }


        public async Task<User> Handle(GetUserInfoRequest request, CancellationToken cancellationToken)
        {
            return await _userService.GetUser(r => r.Id == request.UserInfoRequestModel.UserId && r.IsPublished,request.UserInfoRequestModel.HasInclude);
        }
    }

    public class GetUserInfoRequest : IRequest<User>
    {
        public UserInfoRequestModel UserInfoRequestModel { get; set; }
    }
}
