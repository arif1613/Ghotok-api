using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using Ghotok.Data.UnitOfWork;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class AddAppUserRequestHandler : IRequestHandler<AddAppUserRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddAppUserRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(AddAppUserRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                try
                {

                    _unitOfWork.AppUseRepository.Insert(request.AppUserToAdd);
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

    public class AddAppUserRequest : IRequest<string>
    {
        public AppUser AppUserToAdd{ get; set; }
    }


}
