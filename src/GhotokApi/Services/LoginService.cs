using System;
using System.Linq;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.UnitOfWork;
using GhotokApi.MediatR.Handlers;
using GhotokApi.MediatR.NotificationHandlers;
using GhotokApi.Models.RequestModels;
using MediatR;

namespace GhotokApi.Services
{
    public class LoginService : ILoginService
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;




        public LoginService(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsUserLoggedInAsync(OtpRequestModel model)
        {

            return await Task.Run(() =>
            {
                var user = GetAppUser(model);
                var userLoggedinDevices = user.LoggedInDevices;
                return userLoggedinDevices >=3;
            });

        }

        public async Task<bool> IsUserLoggedOutAsync(OtpRequestModel model)
        {
            return await Task.Run(() =>
            {
                var user = GetAppUser(model);
                var userLoggedinDevices = user.LoggedInDevices;
                return userLoggedinDevices == 0;
            });

        }

        public async Task<AppUser> LogInUserAsync(OtpRequestModel model)
        {
            var user = GetAppUser(model);
            if (user == null) return null;
            user.LoggedInDevices = user.LoggedInDevices + 1;
            user.IsLoggedin = user.LoggedInDevices != 0;
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
            catch (Exception)
            {
                return null;
            }
            return user;
        }

        public async Task<AppUser> LogOutUserAsync(OtpRequestModel model)
        {
            var user = GetAppUser(model);
            if (user == null) return null;
            user.LoggedInDevices = user.LoggedInDevices <= 0 ? 0 : user.LoggedInDevices - 1;
            user.IsLoggedin = user.LoggedInDevices == 0;
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
            catch (Exception)
            {
                return null;
            }
            return user;
        }

        private string CreateCacheKey(Type type, in int startIndex, in int chunkSize, in string isLookingForBride, string includeProperties)
        {
            var cacheKey = $"{type.Name}_{startIndex}_{chunkSize}_{isLookingForBride}";
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    cacheKey = $"{cacheKey}_{includeProperty}";
                }
            }

            return cacheKey;
        }

        private AppUser GetAppUser(OtpRequestModel model)
        {
            var user = model.RegisterByMobileNumber
                ? _unitOfWork.AppUseRepository.Get(
                    r => r.MobileNumber == model.MobileNumber &&
                         r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                         r.Password == model.Password).FirstOrDefault()
                : _unitOfWork.AppUseRepository.Get(
                    r => r.Email == model.Email && r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                         r.Password == model.Password).FirstOrDefault();

            return user;
        }
    }
}