using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using MediatR;
using QQuery.UnitOfWork;

namespace GhotokApi.MediatR.Handlers
{
    public class DeleteUserInfoRequestHandler : IRequestHandler<DeleteUserInfoRequest, string>
    {
        private readonly IQqService<User> _unitOfWork;

        public DeleteUserInfoRequestHandler(IQqService<User> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(DeleteUserInfoRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.QqRepository.Delete(request.UserTobeDeleted);
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
