using System;
using System.Linq;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.UnitOfWork;
using GhotokApi.MediatR.Handlers;
using GhotokApi.Models.NotificationModels;
using GhotokApi.Models.RequestModels;
using MediatR;

namespace GhotokApi.Services
{
    public class RegistrationService:IRegistrationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;


        public RegistrationService(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<bool> IsUserRegisteredAsync(OtpRequestModel model)
        {
            return await Task.Run(()=>
            {
                if (model.RegisterByMobileNumber)
                {
                    return _unitOfWork.AppUseRepository.Get(r => r.MobileNumber == model.MobileNumber).FirstOrDefault()!=null;
                }
                return _unitOfWork.AppUseRepository.Get(r => r.Email == model.Email).FirstOrDefault() != null;
            });
        }

        public async Task RegisterUserAsync(AppUser user)
        {
            try
            {
                var response = await _mediator.Send(new AddAppUserRequest
                {
                    AppUserToAdd = user
                });

                if (response == "Done")
                {
                    await _mediator.Publish(new ComitDatabaseNotification());

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Task UnregisterUserAsync(OtpRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}
