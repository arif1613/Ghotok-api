using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using Ghotok.Data.UnitOfWork;
using GhotokApi.Models.RequestModels;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class GetRecentUserInfosRequestHandler : IRequestHandler<GetRecentUserInfosRequest, IEnumerable<User>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRecentUserInfosRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> Handle(GetRecentUserInfosRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                return _unitOfWork.UserRepository.GetRecent(
                    r => r.LookingForBride == !request.UserInfosRequestModel.LookingForBride,
                    IncludeProperties.UserIncludingAllProperties, request.UserInfosRequestModel.LookingForBride);
            }, cancellationToken);
        
        }
    }

    public class GetRecentUserInfosRequest : IRequest<IEnumerable<User>>
    {
        public UserInfosRequestModel UserInfosRequestModel { get; set; }
    }
}
