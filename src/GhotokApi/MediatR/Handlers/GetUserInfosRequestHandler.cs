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
    public class GetUserInfosRequestHandler : IRequestHandler<GetUserInfosRequest, IEnumerable<User>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserInfosRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> Handle(GetUserInfosRequest request, CancellationToken cancellationToken)
        {
            return _unitOfWork.UserRepository
                .Get(r => r.LookingForBride == !request.UserInfosRequestModel.LookingForBride && r.IsPublished,
                    r => r.OrderBy(q => q.Id).ThenBy(q => q.BasicInfo.Name),
                    IncludeProperties.UserIncludingAllProperties,
                    request.UserInfosRequestModel.LookingForBride,request.UserInfosRequestModel.StartIndex, request.UserInfosRequestModel.ChunkSize);
        }
    }

    public class GetUserInfosRequest : IRequest<IEnumerable<User>>
    {
        public UserInfosRequestModel UserInfosRequestModel { get; set; }
    }
}
