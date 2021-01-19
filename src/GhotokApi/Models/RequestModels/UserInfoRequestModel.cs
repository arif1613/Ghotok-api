using System;
using System.ComponentModel.DataAnnotations;

namespace GhotokApi.Models.RequestModels
{
    public class UserInfoRequestModel
    {
        [Required]
        public Guid UserId { get; set; }

    }
}
