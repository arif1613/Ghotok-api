using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.UnitOfWork;
using GhotokApi.MediatR.Handlers;
using GhotokApi.MediatR.NotificationHandlers;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.SharedModels;
using MediatR;

namespace GhotokApi.Services
{
    public class RegistrationService : IRegistrationService
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
            return await Task.Run(() =>
            {
                if (model.RegisterByMobileNumber)
                {
                   return _unitOfWork.AppUseRepository.Get(new List<Expression<Func<AppUser, bool>>>
                    {
                        r=>r.MobileNumber==model.MobileNumber && r.Password==model.Password,
                    },null).FirstOrDefault() != null;
                }

                return _unitOfWork.AppUseRepository.Get(new List<Expression<Func<AppUser, bool>>>
                {
                    r=>r.Email==model.Email && r.Password==model.Password
                },null).FirstOrDefault() != null;
            });
        }
        public async Task<AppUser> RegisterUserAsync(RegisterRequestModel inputModel)
        {
            var role = AppRole.User.ToString();

            if (inputModel.OtpRequestModel.MobileNumber == "0729958708" && inputModel.OtpRequestModel.CountryCode == "+46")
            {
                role = AppRole.Admin.ToString();
            }
            var validtill = GetValidTill(role);
            var userToRegister = new AppUser
            {
                Id = Guid.NewGuid(),
                MobileNumber = inputModel.OtpRequestModel.MobileNumber,
                CountryCode = inputModel.OtpRequestModel.CountryCode,
                LoggedInDevices = 1,
                IsLoggedin = true,
                IsVarified = true,
                UserRole = role,
                ValidFrom = DateTime.UtcNow,
                ValidTill = validtill,
                RegisterByMobileNumber = inputModel.OtpRequestModel.RegisterByMobileNumber,
                Email = inputModel.OtpRequestModel.Email,
                LookingForBride = inputModel.IsLookingForBride,
                Password = inputModel.OtpRequestModel.Password,
                LanguageChoice = Language.English,
                User = new User
                {
                    Id = Guid.NewGuid(),
                    MobileNumber = inputModel.OtpRequestModel.MobileNumber,
                    CountryCode = inputModel.OtpRequestModel.CountryCode,
                    Email = inputModel.OtpRequestModel.Email,
                    ValidFrom = DateTime.UtcNow,
                    ValidTill = validtill,
                    RegisterByMobileNumber = inputModel.OtpRequestModel.RegisterByMobileNumber,
                    LanguageChoice = Language.English,
                    PictureName = Guid.NewGuid().ToString()
                }
            };
            try
            {
                var response = await _mediator.Send(new AddAppUserRequest
                {
                    AppUserToAdd = userToRegister
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
            return userToRegister;
        }
        public async Task UnregisterUserAsync(OtpRequestModel model)
        {
            await Task.Run(async () =>
            {
               
                var user = model.RegisterByMobileNumber
                    ? _unitOfWork.AppUseRepository.Get(new List<Expression<Func<AppUser, bool>>>
                    {
                        r=>r.MobileNumber==model.MobileNumber
                    },null).FirstOrDefault()
                    : _unitOfWork.AppUseRepository.Get(new List<Expression<Func<AppUser, bool>>>
                    {
                        r=>r.Email==model.Email
                    },null).FirstOrDefault();
                if (user == null) return;

                user.LoggedInDevices = 0;
                user.IsVarified = false;


                try
                {
                    var response = await _mediator.Send(new UpdateAppUserRequest
                    {
                        AppUserTobeUpdated = user

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

            });

        }
        private DateTime GetValidTill(string role)
        {
            if (role == AppRole.PremiumUser.ToString())
            {
                return DateTime.UtcNow + TimeSpan.FromDays(90);
            }

            if (role == AppRole.Admin.ToString())
            {
                return DateTime.UtcNow + TimeSpan.FromDays(3650);
            }
            return DateTime.UtcNow + TimeSpan.FromDays(365);
        }

    }
}
