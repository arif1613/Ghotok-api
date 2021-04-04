using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.MediatR.Handlers;
using GhotokApi.MediatR.NotificationHandlers;
using GhotokApi.Models;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.SharedModels;
using GhotokApi.Services;
using GhotokApi.Utils.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GhotokApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;
        private readonly ILoginService _loginService;

        private readonly IOtpService _otpService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IRegistrationService registrationService, IOtpService otpService, ITokenService tokenService, ILoginService loginService)
        {
            _registrationService = registrationService;
            _otpService = otpService;
            _tokenService = tokenService;
            _loginService = loginService;
        }

        [Route("getotp")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetOtp([FromBody] OtpRequestModel inputModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput);
            }
            try
            {
                if (await _registrationService.IsUserRegisteredAsync(inputModel))
                {
                    return BadRequest(ErrorCodes.UserAlreadyRegistered.ToString());
                }

                return Ok(JsonConvert.SerializeObject(await _otpService.GetOtpAsync(inputModel)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }


        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestModel inputModel)
        {
            AppUser appUser;
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }

            try
            {
                if (!await _otpService.IsOtpValidAsync(inputModel))
                {
                    return BadRequest(ErrorCodes.InvalidOtp.ToString());
                }

                if (!await _registrationService.IsUserRegisteredAsync(inputModel.OtpRequestModel))
                {
                    appUser = await _registrationService.RegisterUserAsync(inputModel);
                    if (appUser == null)
                    {
                        return BadRequest(ErrorCodes.CouldNotCreateData.ToString());
                    }
                }
                else
                {
                    return BadRequest(ErrorCodes.UserAlreadyRegistered.ToString());
                }

                var tokenresponse = _tokenService.GetToken(appUser, inputModel.OtpRequestModel);

                return Ok(JsonConvert.SerializeObject(tokenresponse));
            }
            catch (Exception)
            {
                return BadRequest(ErrorCodes.GenericError.ToString());
            }
        }





        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> LogoutUser([FromBody] OtpRequestModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }

            if (!await _registrationService.IsUserRegisteredAsync(inputModel))
            {
                return BadRequest(ErrorCodes.UserIsNotRegistered.ToString());
            }

            if (await _loginService.IsUserLoggedOutAsync(inputModel))
            {
                return BadRequest(ErrorCodes.UserLoggedOut.ToString());
            }

            try
            {
                var user=await _loginService.LogOutUserAsync(inputModel);
                if (user == null)
                {
                    return BadRequest(ErrorCodes.CouldNotUpdateData.ToString());
                }
                return Ok(JsonConvert.SerializeObject(user));
            }
            catch (Exception)
            {
                return BadRequest(ErrorCodes.CouldNotUpdateData.ToString());

            }
        }


        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] OtpRequestModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput);
            }

            if (!await _registrationService.IsUserRegisteredAsync(inputModel))
            {
                return BadRequest(ErrorCodes.UserIsNotRegistered.ToString());
            }

            if (await _loginService.IsUserLoggedInAsync(inputModel))
            {
                return BadRequest(ErrorCodes.UserAlreadyLoggedinInMOreThanThreeDevices.ToString());
            }

            var user = await _loginService.LogInUserAsync(inputModel);
            if (user == null)
            {
                return BadRequest(ErrorCodes.CouldNotCreateData.ToString());
            }
           
            
            try
            {
                var tokenresponse = await _tokenService.GetToken(user, inputModel);
                if (tokenresponse == null)
                {
                    return BadRequest(ErrorCodes.CouldNotCreateData.ToString());
                }
                //Check token
                return Ok(JsonConvert.SerializeObject(tokenresponse));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [Route("unregister")]
        [HttpPost]
        public async Task<IActionResult> UnregisterUser([FromBody] OtpRequestModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput);
            }
            try
            {

                if (!await _registrationService.IsUserRegisteredAsync(inputModel))
                {
                    return BadRequest(ErrorCodes.UserIsNotRegistered.ToString());
                }

                await _registrationService.UnregisterUserAsync(inputModel);
                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(ErrorCodes.RecordNotFound.ToString());
            }
        }

        //[Route("gettoken")]
        //[HttpPost]
        //public async Task<IActionResult> GetToken([FromBody] OtpRequestModel inputModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ErrorCodes.InvalidInput);
        //    }
        //    try
        //    {
        //        var user = await _mediator.Send(new GetAppUserRequest
        //        {
        //            OtpRequestModel = inputModel
        //        });
        //        if (user == null)
        //        {
        //            return BadRequest(ErrorCodes.RecordNotFound.ToString());
        //        }
        //        var tokenresponse = _loginFlow.GetToken(new AppUser
        //        {
        //            Id = user.Id,
        //            MobileNumber = inputModel.MobileNumber,
        //            CountryCode = inputModel.CountryCode,
        //            LoggedInDevices = user.LoggedInDevices,
        //            IsVarified = true,
        //            UserRole = user.UserRole,
        //            ValidFrom = DateTime.UtcNow,
        //            ValidTill = user.ValidTill,
        //            LookingForBride = user.LookingForBride,
        //            LanguageChoice = Language.English
        //        }, inputModel);
        //        return Ok(JsonConvert.SerializeObject(tokenresponse.Token));
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(e.Message);
        //    }
        //}

        //[Route("newlogin")]
        //[HttpPost]
        //public async Task<IActionResult> NewLogin([FromBody] OtpRequestModel inputModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ErrorCodes.InvalidInput.ToString());
        //    }

        //    var user = await _mediator.Send(new GetAppUserRequest
        //    {
        //        OtpRequestModel = inputModel
        //    });
        //    if (user == null)
        //    {
        //        return BadRequest(ErrorCodes.UserIsNotRegistered.ToString());
        //    }
        //    if (user.Password != inputModel.Password)
        //    {
        //        return BadRequest(ErrorCodes.InvalidInput.ToString());
        //    }
        //    if (user.LoggedInDevices >= 3)
        //    {
        //        return BadRequest(ErrorCodes.UserAlreadyLoggedin.ToString());
        //    }

        //    try
        //    {
        //        var tokenresponse = await _mediator.Send(new LoginUserRequest
        //        {
        //            OtpRequestModel = inputModel
        //        });

        //        if (tokenresponse != null) return Ok(JsonConvert.SerializeObject(tokenresponse));
        //    }
        //    catch (Exception e)
        //    {
        //        return Ok(e.Message);
        //    }

        //    return BadRequest(ErrorCodes.UserAlreadyLoggedin.ToString());
        //}

    }
}
