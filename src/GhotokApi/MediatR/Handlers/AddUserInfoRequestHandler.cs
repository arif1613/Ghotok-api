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
    public class AddUserInfoRequestHandler : IRequestHandler<AddUserInfoRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddUserInfoRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddUserInfoRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                try
                {
                    _unitOfWork.UserRepository.Insert(request.UserToAdd);
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

    public class AddUserInfoRequest : IRequest<string>
    {
        public User UserToAdd{ get; set; }
    }
}
