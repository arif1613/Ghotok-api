using System.Collections.Generic;
using Ghotok.Data.DataModels;

namespace GhotokApi.Models.ResponseModels
{
    public class RecentUserInfosResponseModel:BaseResponseModel
    {
        public List<User> RecentUsers { get; set; }

    }
}
