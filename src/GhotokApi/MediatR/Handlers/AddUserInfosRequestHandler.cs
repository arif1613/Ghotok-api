using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using MediatR;
using QQuery.UnitOfWork;

namespace GhotokApi.MediatR.Handlers
{
    public class AddUserInfosRequestHandler : IRequestHandler<AddUserInfosRequest, string>
    {
        private readonly IQqService<User> _unitOfWork;


        public AddUserInfosRequestHandler(IQqService<User> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddUserInfosRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.QqRepository.Insert(request.UsersToAdd);
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
