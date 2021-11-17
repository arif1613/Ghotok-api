using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models;
using GhotokApi.Models.RequestModels;
using GhotokApi.Models.ResponseModels;
using GhotokApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace GhotokApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserInfoController(IUserService userService)
        {
            _userService = userService;
        }


        [Route("getuserinfo")]
        [HttpPost]
        public async Task<IActionResult> GetUserinfo([FromBody] UserInfoRequestModel requestmodel,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }
            try
            {
                var user = await _userService.GetUser(requestmodel);
                return Ok(JsonConvert.SerializeObject(user));


            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }



        [Route("adduserinfo")]
        [HttpPost]
        public async Task<IActionResult> AddUserinfo([FromBody] User model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await _userService.InsertUser(model);
                return Ok();

            }
            catch (Exception)
            {
                return BadRequest(ErrorCodes.CouldNotCreateData.ToString());
            }
        }

        [Route("updateuserinfo")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserinfo([FromBody] User model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                 await _userService.UpdateUser(model);
                 return Ok("User is updated");
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }

        [Route("deleteuserinfo")]
        [HttpPost]
        public async Task<IActionResult> DeleteUserinfo([FromBody] User model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await _userService.DeleteUser(model);

                return Ok("User is deleted");
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }



        [Route("getuserinfos")]
        [HttpPost]
        public async Task<IActionResult> GetUserInfos([FromBody] UserInfosRequestModel model,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }

            var users =await _userService.GetUsers(model);

            var linkedlist = new LinkedList<User>();
            using var e = users.AsNoTracking().AsSplitQuery().GetEnumerator();
            while (e.MoveNext())
            {
                linkedlist.AddLast(e.Current);
            }

            if (!linkedlist.Any())
            {
                return NotFound(ErrorCodes.RecordNotFound.ToString());
            }

            return Ok(JsonConvert.SerializeObject(new RecentUserInfosResponseModel
            {
                Count = linkedlist.Count(),
                RecentUsers = linkedlist
            }));
        }

        [Route("getrecentuserinfos")]
        [HttpPost]
        public async Task<IActionResult> GetRecentUserInfos([FromBody] UserInfosRequestModel model,CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ErrorCodes.InvalidInput.ToString());
            }

            var users = await _userService.GetRecentUsers(model);
            var linkedlist = new LinkedList<User>();
            var e = users.GetEnumerator();
            while (e.MoveNext())
            {
                linkedlist.AddLast(e.Current);
            }

            if (!linkedlist.Any())
            {
                return NotFound(ErrorCodes.RecordNotFound.ToString());
            }
           

            return Ok(JsonConvert.SerializeObject(new RecentUserInfosResponseModel
            {
                Count = linkedlist.Count(),
                RecentUsers = linkedlist
            }));
        }
    }
}
