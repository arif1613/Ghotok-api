using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GhotokApi.Common;
using static System.Int32;

namespace GhotokApi.Models.RequestModels
{
    public class UserInfosRequestModel
    {
        public IEnumerable<IDictionary<string, string>> Filters { get; set; }
        [Required]
        [Range(0,MaxValue)]
        public int StartIndex { get; set; }
        [Required]
        [Range(0, MaxValue)]
        public int ChunkSize { get; set; }

        [Required]
        public bool HasOrderBy { get; set; }
        [Required]
        public bool HasInclude { get; set; }

    }
}
