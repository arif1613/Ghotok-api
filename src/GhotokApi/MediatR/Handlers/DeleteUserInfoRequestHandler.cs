using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.UnitOfWork;
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
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.UserRepository.Delete(request.UserTobeDeleted);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            }, cancellationToken);
        }
    }

    public class DeleteUserInfoRequest : IRequest<string>
    {
        public User UserTobeDeleted { get; set; }
    }
}
