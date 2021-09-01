using System.Collections.Generic;
using Ghotok.Data.DataModels;

namespace GhotokApi.Models.ResponseModels
{
    public class UserInfosResponseModel: BaseResponseModel
    {
        public IEnumerable<User> Users { get; set; }
    }
}
