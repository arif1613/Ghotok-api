using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GhotokApi.Common;

namespace GhotokApi.Models.RequestModels
{
    public class UserInfosRequestModel
    {
        //[Required]
        //public bool LookingForBride { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Filters { get; set; }
        [Required]
        [Range(0,Int32.MaxValue)]
        public int StartIndex { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int ChunkSize { get; set; }

        //[Required]
        //public bool IsPublished { get; set; }

        [Required]
        public bool HasOrderBy { get; set; }
        [Required]
        public bool HasInclude { get; set; }

    }
}
