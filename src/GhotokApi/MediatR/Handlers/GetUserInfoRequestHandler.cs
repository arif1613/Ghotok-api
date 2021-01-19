using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;
using GhotokApi.Repo;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class GetUserInfoRequestHandler : IRequestHandler<GetUserInfoRequest, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserInfoRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(GetUserInfoRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository
                .GetAsync(r => r.Id == request.UserInfoRequestModel.UserId && r.IsPublished,request.UserInfoRequestModel.UserId.ToString(),
                    IncludeProperties.UserIncludingAllProperties);
        }
    }

    public class GetUserInfoRequest : IRequest<User>
    {
        public UserInfoRequestModel UserInfoRequestModel { get; set; }
    }
}
