using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GhotokApi.Models.RequestModels
{
    public class UserInfoRequestModel
    {
        public IEnumerable<IDictionary<string, string>> Filters { get; set; }
        public bool HasInclude { get; set; }
    }
}
