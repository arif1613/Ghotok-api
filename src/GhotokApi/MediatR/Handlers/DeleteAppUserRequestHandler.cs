using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using MediatR;
using QQuery.UnitOfWork;

namespace GhotokApi.MediatR.Handlers
{
    public class DeleteAppUserRequestHandler : IRequestHandler<DeleteAppUserRequest, string>
    {
        private readonly IQqService<AppUser> _unitOfWork;

        public DeleteAppUserRequestHandler(IQqService<AppUser> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(DeleteAppUserRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.QqRepository.Delete(request.AppUserTobeDeleted);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            }, cancellationToken);
        }
    }

    public class DeleteAppUserRequest : IRequest<string>
    {
        public AppUser AppUserTobeDeleted { get; set; }
    }
}
