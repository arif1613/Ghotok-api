using System;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.Utils.Cache;
using GhotokApi.MediatR.Handlers;
using GhotokApi.MediatR.NotificationHandlers;
using GhotokApi.Models.RequestModels;
using MediatR;

namespace GhotokApi.Services
{
    public class LoginService : ILoginService
    {
        private readonly IAppUserService _appUserService;
        private readonly IMediator _mediator;
        private readonly ICacheHelper _cacheHelper;



        public LoginService(IAppUserService appUserService, IMediator mediator, ICacheHelper cacheHelper)
        {
            _appUserService = appUserService;
            _mediator = mediator;
            _cacheHelper = cacheHelper;
        }

        public async Task<bool> IsUserLoggedInAsync(OtpRequestModel model)
        {
            AppUser user;
            if (model.RegisterByMobileNumber)
            {
                user = await _appUserService.GetAppUser(r => r.MobileNumber == model.MobileNumber &&
                                                             r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                                                             r.Password == model.Password && r.LoggedInDevices == 3);
                return user != null;
            }

            user = await _appUserService.GetAppUser(r => r.Email == model.Email &&
                                                         r.RegisterByMobileNumber == false && r.Password == model.Password &&
                                                         r.LoggedInDevices == 3);
            return user != null;
        }

        public async Task<bool> IsUserLoggedOutAsync(OtpRequestModel model)
        {
            AppUser user;
            if (model.RegisterByMobileNumber)
            {
                user = await _appUserService.GetAppUser(r => r.MobileNumber == model.MobileNumber &&
                                                             r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                                                             r.Password == model.Password && r.LoggedInDevices == 0);
                return user != null;
            }

            user = await _appUserService.GetAppUser(r => r.Email == model.Email &&
                                                         r.RegisterByMobileNumber == false && r.Password == model.Password &&
                                                         r.LoggedInDevices == 0);
            return user != null;
        }

        public async Task<AppUser> LogInUserAsync(OtpRequestModel model)
        {
            var user = model.RegisterByMobileNumber
                ? await _appUserService.GetAppUser(
                    r => r.MobileNumber == model.MobileNumber &&
                         r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                         r.Password == model.Password)
                : await _appUserService.GetAppUser(
                    r => r.Email == model.Email && r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                         r.Password == model.Password);

            if (user == null) return null;

            var cachekey = model.RegisterByMobileNumber ? model.MobileNumber : model.Email;

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
                if (_cacheHelper.Exists(cachekey))
                {
                    _cacheHelper.Update(user, cachekey);
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
            var user = model.RegisterByMobileNumber
                ? await _appUserService.GetAppUser(
                    r => r.MobileNumber == model.MobileNumber &&
                         r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                         r.Password == model.Password)
                : await _appUserService.GetAppUser(
                    r => r.Email == model.Email && r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                         r.Password == model.Password);

            if (user == null) return null;

            var cachekey = model.RegisterByMobileNumber ? model.MobileNumber : model.Email;

            user.LoggedInDevices = user.LoggedInDevices <= 0 ? 0 : user.LoggedInDevices - 1;
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
                if (_cacheHelper.Exists(cachekey))
                {
                    _cacheHelper.Update(user, cachekey);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return user;
        }
    }
}