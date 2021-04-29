using System;
using System.Threading.Tasks;
using Ghotok.Data.Utils.Cache;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.ResponseModels;
using Microsoft.Extensions.Configuration;

namespace GhotokApi.Services
{
    public class OtpService:IOtpService
    {
        private readonly ICacheHelper _cacheHelper;
        private readonly IConfiguration _configuration;
        private readonly Random _random = new Random();


        public OtpService(ICacheHelper cacheHelper, IConfiguration configuration)
        {
            _cacheHelper = cacheHelper;
            _configuration = configuration;
        }

        public async Task<OtpResponseModel> GetOtpAsync(OtpRequestModel model)
        {
            return await Task.Run(() =>
            {
                //Send Otp here
                var CacheKey = !model.RegisterByMobileNumber ? $"Otp_{ model.Email}" : $"Otp_{model.MobileNumber}";
                if (_cacheHelper.Exists(CacheKey))
                {
                    return _cacheHelper.Get<OtpResponseModel>(CacheKey);
                }
                var otp = _random.Next(1000, 9999);
                _cacheHelper.Add(new OtpResponseModel
                {
                    Otp = otp.ToString()
                }, CacheKey, Convert.ToInt32(_configuration["OtpCaheMinute"]));

                return new OtpResponseModel { Otp = otp.ToString() };
            });
        }

        public async Task<bool> IsOtpValidAsync(RegisterRequestModel model)
        {
            return await Task.Run(() =>
            {
                var cachekey = !model.OtpRequestModel.RegisterByMobileNumber ? $"Otp_{model.OtpRequestModel.Email}" : $"Otp_{model.OtpRequestModel.MobileNumber}";
                if (!_cacheHelper.Exists(cachekey))
                {
                    return false;
                }

                return Convert.ToInt32(_cacheHelper.Get<OtpResponseModel>(cachekey).Otp) == Convert.ToInt32(model.Otp.Trim());
            });
        }
    }
}
