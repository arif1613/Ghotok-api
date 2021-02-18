using System;
using System.Collections.Generic;
using System.Text;

namespace Ghotok.Data.DataModels.Views
{
    public class UserShortInfo
    {
        public Guid Id { get; set; }
        public bool LookingForBride { get; set; }
        public bool IsPictureUploaded { get; set; }
        public string PictureName { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string ContactNumber { get; set; }
        public DateTime Dob { get; set; }
        public string MaritalStatus { get; set; }
    }
}
