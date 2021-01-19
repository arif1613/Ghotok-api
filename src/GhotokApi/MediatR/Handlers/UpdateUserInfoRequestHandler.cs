using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Repo;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class UpdateUserInfoRequestHandler:IRequestHandler<UpdateUserInfoRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserInfoRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UpdateUserInfoRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    _unitOfWork.UserRepository.Update(request.UserTobeUpdated);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            });
        }
    }

    public class UpdateUserInfoRequest : IRequest<string>
    {
        public User UserTobeUpdated { get; set; }
    }
}
