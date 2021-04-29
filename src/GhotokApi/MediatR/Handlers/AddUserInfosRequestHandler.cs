using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.UnitOfWork;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class AddUserInfosRequestHandler : IRequestHandler<AddUserInfosRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork;


        public AddUserInfosRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddUserInfosRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.UserRepository.Insert(request.UsersToAdd);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            }, cancellationToken);
        }
    }

    public class AddUserInfosRequest : IRequest<string>
    {
        public List<User> UsersToAdd{ get; set; }
    }
}
