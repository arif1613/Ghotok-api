using System;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.MediatR.Handlers;
using GhotokApi.MediatR.NotificationHandlers;
using GhotokApi.Models;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.SharedModels;
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
        private readonly ILoginFlow _loginFlow;
        private readonly IMediator _mediator;


        public AuthenticationController(ILoginFlow loginFlow, IMediator mediator)
        {
            _loginFlow = loginFlow;
            _mediator = mediator;
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
                if (await _loginFlow.IsUserRegisteredAsync(inputModel))
                {
                    return BadRequest(ErrorCodes.UserAlreadyRegistered.ToString());
                }

                return Ok(JsonConvert.SerializeObject(await _loginFlow.GetOtpAsync(inputModel)));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }


        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestModel inputModel,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }

            try
            {
                if (!await _loginFlow.IsOtpValidAsync(inputModel))
                {
                    return BadRequest(ErrorCodes.InvalidOtp.ToString());
                }

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

                if (!await _loginFlow.IsUserRegisteredAsync(inputModel.OtpRequestModel))
                {
                    var res = await _mediator.Send(new RegisterUserRequest
                    {
                        UserToRegister = userToRegister
                    });
                    if (res == "Done")
                    {
                        await _mediator.Publish(new ComitDatabaseNotification(), cancellationToken);
                    }
                    else
                    {
                        return BadRequest(ErrorCodes.GenericError.ToString());
                    }

                }
                else
                {
                    var user = await _mediator.Send(new GetAppUserRequest
                    {
                        OtpRequestModel = inputModel.OtpRequestModel
                    });

                    if (user != null && user.LoggedInDevices < 3)
                    {
                        await _loginFlow.LogInUserAsync(inputModel.OtpRequestModel);
                    }
                    else
                    {
                        return BadRequest(ErrorCodes.UserAlreadyLoggedin.ToString());
                    }
                }

                var tokenresponse = _loginFlow.GetToken(userToRegister, inputModel.OtpRequestModel);

                return Ok(JsonConvert.SerializeObject(tokenresponse));
            }
            catch (Exception e)
            {
                return BadRequest(ErrorCodes.GenericError.ToString());
            }
        }

        private DateTime GetValidTill(string role)
        {
            if (role == AppRole.PremiumUser.ToString())
            {
                return DateTime.UtcNow + TimeSpan.FromDays(90);
            }
            return DateTime.UtcNow + TimeSpan.FromDays(3650);
        }




        [Route("logout")]
        [HttpPost]
        public async Task<IActionResult> LogoutUser([FromBody] OtpRequestModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput);
            }

            var user = await _mediator.Send(new GetAppUserRequest
            {
                OtpRequestModel = inputModel
            });
            if (user == null)
            {
                return BadRequest(ErrorCodes.UserIsNotRegistered.ToString());
            }
            if (user.Password != inputModel.Password)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }

            try
            {

                await _loginFlow.LogOutUserAsync(inputModel);
                return Ok(JsonConvert.SerializeObject(user));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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

            var user = await _mediator.Send(new GetAppUserRequest
            {
                OtpRequestModel = inputModel
            });
            if (user == null)
            {
                return BadRequest(ErrorCodes.UserIsNotRegistered.ToString());
            }
            if (user.Password != inputModel.Password)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }
            if (user.LoggedInDevices >= 3)
            {
                return BadRequest(ErrorCodes.UserAlreadyLoggedin.ToString());
            }
            try
            {
                var tokenresponse = await _mediator.Send(new LoginUserRequest
                {
                    OtpRequestModel = inputModel
                });
                if (tokenresponse == null)
                {
                    return BadRequest(ErrorCodes.RecordNotFound.ToString());
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
                var user = await _mediator.Send(new GetAppUserRequest
                {
                    OtpRequestModel = inputModel
                });
                if (user != null)
                {
                    await _loginFlow.UnregisterUserAsync(inputModel);
                    user.IsVarified = false;
                    user.LoggedInDevices = 0;
                    return Ok(JsonConvert.SerializeObject(user));
                }

                return BadRequest(ErrorCodes.RecordNotFound.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Route("gettoken")]
        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] OtpRequestModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput);
            }
            try
            {
                var user = await _mediator.Send(new GetAppUserRequest
                {
                    OtpRequestModel = inputModel
                });
                if (user == null)
                {
                    return BadRequest(ErrorCodes.RecordNotFound.ToString());
                }
                var tokenresponse = _loginFlow.GetToken(new AppUser
                {
                    Id = user.Id,
                    MobileNumber = inputModel.MobileNumber,
                    CountryCode = inputModel.CountryCode,
                    LoggedInDevices = user.LoggedInDevices,
                    IsVarified = true,
                    UserRole = user.UserRole,
                    ValidFrom = DateTime.UtcNow,
                    ValidTill = user.ValidTill,
                    LookingForBride = user.LookingForBride,
                    LanguageChoice = Language.English
                }, inputModel);
                return Ok(JsonConvert.SerializeObject(tokenresponse.Token));
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [Route("newlogin")]
        [HttpPost]
        public async Task<IActionResult> NewLogin([FromBody] OtpRequestModel inputModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }

            var user = await _mediator.Send(new GetAppUserRequest
            {
                OtpRequestModel = inputModel
            });
            if (user == null)
            {
                return BadRequest(ErrorCodes.UserIsNotRegistered.ToString());
            }
            if (user.Password != inputModel.Password)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }
            if (user.LoggedInDevices >= 3)
            {
                return BadRequest(ErrorCodes.UserAlreadyLoggedin.ToString());
            }

            try
            {
                var tokenresponse = await _mediator.Send(new LoginUserRequest
                {
                    OtpRequestModel = inputModel
                });

                if (tokenresponse != null) return Ok(JsonConvert.SerializeObject(tokenresponse));
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

            return BadRequest(ErrorCodes.UserAlreadyLoggedin.ToString());
        }

    }
}
