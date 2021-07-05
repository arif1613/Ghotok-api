using System.Collections.Generic;

namespace GhotokApi.Models.RequestModels
{
    public class UserInfoRequestModel
    {
        public IEnumerable<IDictionary<string, string>> Filters { get; set; }
        public bool HasInclude { get; set; }
    }
}
