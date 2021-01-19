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
    public class DeleteUserInfoRequestHandler : IRequestHandler<DeleteUserInfoRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserInfoRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(DeleteUserInfoRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    _unitOfWork.UserRepository.Delete(request.UserTobeDeleted);
                    _unitOfWork.Commit();
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            });
        }
    }

    public class DeleteUserInfoRequest : IRequest<string>
    {
        public User UserTobeDeleted { get; set; }
    }
}
