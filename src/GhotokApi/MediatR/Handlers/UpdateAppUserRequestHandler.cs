using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.UnitOfWork;
using MediatR;

namespace GhotokApi.MediatR.Handlers
{
    public class UpdateAppUserRequestHandler : IRequestHandler<UpdateAppUserRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAppUserRequestHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UpdateAppUserRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.AppUseRepository.Update(request.AppUserTobeUpdated);
                    return "Done";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
               
            }, cancellationToken);
        }
    }

    public class UpdateAppUserRequest : IRequest<string>
    {
        public AppUser AppUserTobeUpdated { get; set; }
    }
}
