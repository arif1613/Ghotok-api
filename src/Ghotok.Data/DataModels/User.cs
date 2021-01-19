using System;
using System.ComponentModel.DataAnnotations;

namespace Ghotok.Data.DataModels
{
    public class User:BaseDbModel
    {
        public string PictureName { get; set; }
        public bool IsPictureUploaded { get; set; }
        public bool IsPublished { get; set; }
        public virtual BasicInfo BasicInfo { get; set; }
        public virtual FamilyInfo FamilyInfo { get; set; }
        public virtual EducationInfo EducationInfo { get; set; }
    }
}
