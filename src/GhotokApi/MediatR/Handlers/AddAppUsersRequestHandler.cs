using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.UnitOfWork;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class AddAppUsersRequestHandler : IRequestHandler<AddAppUsersRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddAppUsersRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddAppUsersRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.AppUseRepository.Insert(request.AppUsersToAdd);
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
