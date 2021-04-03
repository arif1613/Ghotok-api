using System;
using System.Linq;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.MediatR.Handlers;
using GhotokApi.Models;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GhotokApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserInfoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("getuserinfo")]
        [HttpPost]
        public async Task<IActionResult> GetUserinfo([FromBody] UserInfoRequestModel requestmodel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }
            try
            {
                var user = await _mediator.Send(new GetUserInfoRequest
                {
                    UserInfoRequestModel = requestmodel
                });
                return Ok(JsonConvert.SerializeObject(user));


            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }



        [Route("adduserinfo")]
        [HttpPost]
        public async Task<IActionResult> AddUserinfo([FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var response = await _mediator.Send(new AddUserInfoRequest
                {
                    UserToAdd = model
                });

                if (response == "Done")
                {
                    await _mediator.Send(new ComitDatabaseRequest());
                }


                return BadRequest(ErrorCodes.CouldNotCreateData.ToString());

            }
            catch (Exception e)
            {
                return BadRequest(ErrorCodes.CouldNotCreateData.ToString());
            }
        }

        [Route("updateuserinfo")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserinfo([FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var response = await _mediator.Send(new UpdateUserInfoRequest
                {
                    UserTobeUpdated = model
                });

                if (response == "Done")
                {
                    await _mediator.Send(new ComitDatabaseRequest());
                }

               

                return BadRequest(ErrorCodes.CouldNotUpdateData.ToString());
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [Route("deleteuserinfo")]
        [HttpPost]
        public async Task<IActionResult> DeleteUserinfo([FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var response = await _mediator.Send(new DeleteUserInfoRequest
                {
                    UserTobeDeleted = model
                });

                if (response == "Done")
                {
                    await _mediator.Send(new ComitDatabaseRequest());
                }

              

                return BadRequest(ErrorCodes.CouldNotUpdateData.ToString());
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }



        [Route("getuserinfos")]
        [HttpPost]
        public async Task<IActionResult> GetUserInfos([FromBody] UserInfosRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }

            var users = await _mediator.Send(new GetUserInfosRequest
            {
                UserInfosRequestModel = model
            });
            var usersResponse = users.ToList();
            return Ok(JsonConvert.SerializeObject(new UserInfosResponseModel
            {
                Count = usersResponse.Count(),
                Users = usersResponse.ToList()
            }));
        }

        [Route("getrecentuserinfos")]
        [HttpPost]
        public async Task<IActionResult> GetRecentUserInfos([FromBody] UserInfosRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }

            var users = await _mediator.Send(new GetRecentUserInfosRequest
            {
                UserInfosRequestModel = model
            });


            var usersResponse = users.ToList();
            return Ok(JsonConvert.SerializeObject(new RecentUserInfosResponseModel
            {
                Count = usersResponse.Count(),
                RecentUsers = usersResponse.ToList()
            }));
        }
    }
}
