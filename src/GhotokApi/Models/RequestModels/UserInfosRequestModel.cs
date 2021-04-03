using System;
using System.ComponentModel.DataAnnotations;

namespace GhotokApi.Models.RequestModels
{
    public class UserInfosRequestModel
    {
        [Required]
        public bool LookingForBride { get; set; }
        [Required]
        [Range(0,Int32.MaxValue)]
        public int StartIndex { get; set; }
        [Required]
        [Range(0, Int32.MaxValue)]
        public int ChunkSize { get; set; }

        [Required]
        public bool IsPublished { get; set; }

        [Required]
        public bool HasOrderBy { get; set; }
        [Required]
        public bool HasInclude { get; set; }

    }
}
