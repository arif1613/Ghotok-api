using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using MediatR;
using QQuery.UnitOfWork;

namespace GhotokApi.MediatR.Handlers
{
    public class AddUserInfoRequestHandler : IRequestHandler<AddUserInfoRequest, string>
    {
        private readonly IQqService<User> _unitOfWork;


        public AddUserInfoRequestHandler(IQqService<User> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddUserInfoRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.QqRepository.Insert(request.UserToAdd);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            }, cancellationToken);
        }
    }

    public class AddUserInfoRequest : IRequest<string>
    {
        public User UserToAdd{ get; set; }
    }
}
