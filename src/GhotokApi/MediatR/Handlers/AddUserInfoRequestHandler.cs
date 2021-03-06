﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.UnitOfWork;
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
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.UserRepository.Insert(request.UserToAdd);
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
