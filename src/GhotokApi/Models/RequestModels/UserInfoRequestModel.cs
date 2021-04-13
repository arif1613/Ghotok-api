using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GhotokApi.Models.RequestModels
{
    public class UserInfoRequestModel
    {
        public IEnumerable<KeyValuePair<string, string>> Filters { get; set; }
    }
}
