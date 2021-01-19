using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models;
using GhotokApi.Models.RequestModels;
using GhotokApi.Repo;
using MediatR;
using Newtonsoft.Json;

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
            return _unitOfWork.UserRepository.GetRecent(
                r => r.LookingForBride == !request.UserInfosRequestModel.LookingForBride,
                IncludeProperties.UserIncludingAllProperties,request.UserInfosRequestModel.LookingForBride);
        }
    }

    public class GetRecentUserInfosRequest : IRequest<IEnumerable<User>>
    {
        public UserInfosRequestModel UserInfosRequestModel { get; set; }
    }
}
