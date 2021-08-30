using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using MediatR;
using QQuery.UnitOfWork;

namespace GhotokApi.MediatR.Handlers
{
    public class UpdateAppUserRequestHandler : IRequestHandler<UpdateAppUserRequest, string>
    {
        private readonly IQqService<AppUser> _unitOfWork;

        public UpdateAppUserRequestHandler(IQqService<AppUser> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(UpdateAppUserRequest request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                try
                {
                    _unitOfWork.QqRepository.Update(request.AppUserTobeUpdated);
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
