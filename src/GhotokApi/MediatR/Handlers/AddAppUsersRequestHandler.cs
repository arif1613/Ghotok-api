using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using MediatR;
using QQuery.UnitOfWork;

namespace GhotokApi.MediatR.Handlers
{
    public class AddAppUsersRequestHandler : IRequestHandler<AddAppUsersRequest, string>
    {
        private readonly IQqService<AppUser> _qqService;

        public AddAppUsersRequestHandler(IQqService<AppUser> unitOfWork)
        {
            _qqService = unitOfWork;
        }

        public async Task<string> Handle(AddAppUsersRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _qqService.QqRepository.Insert(request.AppUsersToAdd);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            });
        }

       
    }

    public class AddAppUsersRequest : IRequest<string>
    {
        public List<AppUser> AppUsersToAdd{ get; set; }
    }


}
