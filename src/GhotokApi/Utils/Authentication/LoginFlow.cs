using System;
using System.Linq;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.JwtTokenGenerator;
using GhotokApi.MediatR.Handlers;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.ResponseModels;
using GhotokApi.Models.SharedModels;
using GhotokApi.Repo;
using GhotokApi.Utils.Cache;
using GhotokApi.Utils.DbOperations;
using GhotokApi.Utils.Otp;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace GhotokApi.Utils.Authentication
{
    public class LoginFlow : ILoginFlow
    {
        private readonly ICacheHelper _cacheHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOtpSender _otpSender;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;





        private readonly Random _random = new Random();

        public LoginFlow(ICacheHelper cacheHelper, IUnitOfWork unitOfWork, IOtpSender otpSender, IConfiguration configuration, IMediator mediator)
        {
            _cacheHelper = cacheHelper;
            _unitOfWork = unitOfWork;
            _otpSender = otpSender;
            _configuration = configuration;
            _mediator = mediator;
        }


        public async Task<bool> IsUserRegisteredAsync(OtpRequestModel model)
        {

            return await Task.Run(() =>
            {
                if (model.RegisterByMobileNumber)
                {
                    return _unitOfWork.AppUseRepository.GetAsync(r => r.MobileNumber == model.MobileNumber, model.MobileNumber, null).GetAwaiter().GetResult() != null;
                }
                return _unitOfWork.AppUseRepository.GetAsync(r => r.Email == model.Email, model.Email, null).GetAwaiter().GetResult() != null;
            });
        }



        public async Task<OtpResponseModel> GetOtpAsync(OtpRequestModel model)
        {

            //Send Otp here
            var CacheKey = !model.RegisterByMobileNumber ? $"Otp_{ model.Email}" : $"Otp_ {model.MobileNumber}";
            if (await _cacheHelper.Exists(CacheKey))
            {
                return await _cacheHelper.Get<OtpResponseModel>(CacheKey);
            }
            var otp = _random.Next(1000, 9999);
            await _cacheHelper.Add(new OtpResponseModel
            {
                Otp = otp.ToString()
            }, CacheKey, Convert.ToInt32(_configuration["OtpCaheMinute"]));


            //Message sender
            //if (model.RegisterByMobileNumber)
            //{
            //    await _otpSender.SendOtpMobileMessage(model,otp.ToString());
            //}
            //else
            //{
            //    await _otpSender.SendOtpEmailMessage(model,otp.ToString());
            //}
            return new OtpResponseModel { Otp = otp.ToString() };
        }



        public async Task<bool> IsUserLoggedInAsync(OtpRequestModel model)
        {
            return await Task.Run(() =>
            {
                AppUser user;
                if (model.RegisterByMobileNumber)
                {
                    user = _unitOfWork.AppUseRepository
                        .GetAsync(r => r.MobileNumber == model.MobileNumber && r.RegisterByMobileNumber == model.RegisterByMobileNumber && r.Password == model.Password && r.LoggedInDevices <= 3,
                            model.MobileNumber).GetAwaiter().GetResult();
                    return user != null;
                }

                user = _unitOfWork.AppUseRepository
                    .GetAsync(r => r.Email == model.Email && r.RegisterByMobileNumber == model.RegisterByMobileNumber && r.Password == model.Password && r.LoggedInDevices <= 3,
                        model.Email).GetAwaiter().GetResult();
                return user != null;
            });

        }


        public async Task<bool> IsOtpValidAsync(RegisterRequestModel model)
        {
            var cachekey = !model.OtpRequestModel.RegisterByMobileNumber ? $"Otp_{ model.OtpRequestModel.Email}" : $"Otp_ {model.OtpRequestModel.MobileNumber}";
            if (!await _cacheHelper.Exists(cachekey))
            {
                return false;
            }


            return Convert.ToInt32(_cacheHelper.Get<OtpResponseModel>(cachekey).GetAwaiter().GetResult().Otp) == Convert.ToInt32(model.Otp.Trim());
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
                    _unitOfWork.Commit();

                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task UnregisterUserAsync(OtpRequestModel model)
        {
            await Task.Run(() =>
            {
                var user = model.RegisterByMobileNumber
                    ? _unitOfWork.AppUseRepository.GetAsync(r => r.MobileNumber == model.MobileNumber, model.MobileNumber).GetAwaiter().GetResult()
                    : _unitOfWork.AppUseRepository.GetAsync(r => r.Email == model.Email, model.Email).GetAwaiter().GetResult();
                if (user == null) return;

                var cachekey = model.RegisterByMobileNumber ? model.MobileNumber : model.Email;
                user.LoggedInDevices = 0;
                user.IsVarified = false;
                _unitOfWork.AppUseRepository.Update(user);
                _unitOfWork.Commit();
                if (_cacheHelper.Exists(cachekey).GetAwaiter().GetResult())
                {
                    _cacheHelper.Update(user, cachekey).GetAwaiter().GetResult();
                }
            });
        }

        public async Task<TokenResponseModel> LogInUserAsync(OtpRequestModel model)
        {
            AppUser user;
            TokenResponseModel tokenResponseModel;
            return await Task.Run(() =>
            {

                user = model.RegisterByMobileNumber
                    ? _unitOfWork.AppUseRepository
                        .GetAsync(
                            r => r.MobileNumber == model.MobileNumber &&
                                 r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                                 r.Password == model.Password, model.MobileNumber).GetAwaiter().GetResult()
                    : _unitOfWork.AppUseRepository
                        .GetAsync(
                            r => r.Email == model.Email && r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                                 r.Password == model.Password, model.Email).GetAwaiter().GetResult();
                if (user == null) return null;
                var cachekey = model.RegisterByMobileNumber ? model.MobileNumber : model.Email;

                try
                {
                    //Check token
                    if (user.UserRole == AppRole.PremiumUser.ToString())
                    {
                        if (user.ValidTill < DateTime.UtcNow)
                        {
                            user.UserRole = AppRole.User.ToString();
                            user.ValidTill = DateTime.UtcNow + TimeSpan.FromDays(3650);
                        }
                    }

                    try
                    {
                        if (user.LoggedInDevices >= 3)
                        {
                            return null;
                        }

                        user.LoggedInDevices = user.LoggedInDevices + 1;
                        user.IsLoggedin = true;
                        _unitOfWork.AppUseRepository.Update(user);
                        _unitOfWork.Commit();
                        tokenResponseModel = GetToken(user, model);

                        if (_cacheHelper.Exists(cachekey).GetAwaiter().GetResult())
                        {
                            _cacheHelper.Update(user, cachekey).GetAwaiter().GetResult();
                        }
                        return tokenResponseModel;

                    }
                    catch (Exception e)
                    {
                        return null;
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
            });
        }

        public async Task LogOutUserAsync(OtpRequestModel model)
        {
            await Task.Run(() =>
            {
                var user = model.RegisterByMobileNumber
                    ? _unitOfWork.AppUseRepository
                        .GetAsync(
                            r => r.MobileNumber == model.MobileNumber &&
                                 r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                                 r.Password == model.Password, model.MobileNumber).GetAwaiter().GetResult()
                    : _unitOfWork.AppUseRepository
                        .GetAsync(
                            r => r.Email == model.Email && r.RegisterByMobileNumber == model.RegisterByMobileNumber &&
                                 r.Password == model.Password, model.Email).GetAwaiter().GetResult();

                if (user == null) return;

                var cachekey = model.RegisterByMobileNumber ? model.MobileNumber : model.Email;

                user.LoggedInDevices = user.LoggedInDevices <= 0 ? 0 : user.LoggedInDevices - 1;
                user.IsLoggedin= user.LoggedInDevices == 0 ? false :true;
                _unitOfWork.AppUseRepository.Update(user);
                _unitOfWork.Commit();

                if (_cacheHelper.Exists(cachekey).GetAwaiter().GetResult())
                {
                    _cacheHelper.Update(user, cachekey).GetAwaiter().GetResult();
                }
            });
        }

        public async Task<AppUser> GetUserAsync(OtpRequestModel model)
        {
            AppUser user;
            return await Task.Run(() =>
            {
                try
                {
                    user = model.RegisterByMobileNumber
                        ? _unitOfWork.AppUseRepository
                            .GetAsync(r => r.MobileNumber == model.MobileNumber && r.RegisterByMobileNumber == model.RegisterByMobileNumber && r.IsVarified, model.MobileNumber)
                            .GetAwaiter().GetResult()
                        : _unitOfWork.AppUseRepository
                            .GetAsync(r => r.Email == model.Email && r.RegisterByMobileNumber == model.RegisterByMobileNumber && r.IsVarified, model.Email).GetAwaiter()
                            .GetResult();

                    return user ?? null;
                }
                catch (Exception e)
                {
                    throw e;
                }

            });
        }

        public TokenResponseModel GetToken(AppUser user, OtpRequestModel model)
        {
            user.IsLoggedin = user.LoggedInDevices < 3;
            var token = new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create("Ghotok-Secret-Key"))
                .AddSubject(model.MobileNumber)
                .AddIssuer("Ghotok.Security.Bearer")
                .AddAudience("Ghotok.App")
                .AddClaim(user.UserRole, user.Id)
                .AddRole(user.UserRole)
                .AddExpiry(user.ValidTill)
                .Build();
            return new TokenResponseModel
            {
                Token = token.Value,
                AppUser = user,
                OtpRequestModel = model
            };
        }
    }
}
