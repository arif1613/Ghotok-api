using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using MediatR;
using QQuery.UnitOfWork;

namespace GhotokApi.MediatR.Handlers
{
    public class UpdateUserInfoRequestHandler:IRequestHandler<UpdateUserInfoRequest, string>
    {
        private readonly IQqService<User> _unitOfWork;

        public UpdateUserInfoRequestHandler(IQqService<User> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UpdateUserInfoRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.QqRepository.Update(request.UserTobeUpdated);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            }, cancellationToken);
        }
    }

    public class UpdateUserInfoRequest : IRequest<string>
    {
        public User UserTobeUpdated { get; set; }
    }
}
