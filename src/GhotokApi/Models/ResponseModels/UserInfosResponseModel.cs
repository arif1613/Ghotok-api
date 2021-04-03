using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;

namespace GhotokApi.Models.ResponseModels
{
    public class UserInfosResponseModel: BaseResponseModel
    {
        public List<User> Users { get; set; }
    }
}
