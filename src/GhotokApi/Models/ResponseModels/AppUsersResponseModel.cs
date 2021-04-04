using System.Collections.Generic;
using Ghotok.Data.DataModels;

namespace GhotokApi.Models.ResponseModels
{
    public class AppUsersResponseModel : BaseResponseModel
    {
        public List<AppUser> AppUsers { get; set; }
    }
}
