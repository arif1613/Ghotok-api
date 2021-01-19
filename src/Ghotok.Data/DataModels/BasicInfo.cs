using System;
using System.ComponentModel.DataAnnotations;

namespace Ghotok.Data.DataModels
{
    public class BasicInfo
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string ProfileCreatingFor { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string MaritalStatus { get; set; }
        [Required]
        public DateTime Dob { get; set; }
        [Required]
        [Phone]
        public string ContactNumber { get; set; }
        [Required]
        public int Height_cm { get; set; }
        [Required]
        public string PresentAddress { get; set; }
        [Required]
        public string Religion { get; set; }
        [Required]
        public string ReligionCast { get; set; }

        [EmailAddress]
        public string email { get; set; }

        public string PermanentAddress { get; set; }
        public string PaternalHomeDistrict { get; set; }
        public string MaternalHomeDistrict { get; set; }
        public string PropertyInfo { get; set; }
        public string OtherBasicInfo { get; set; }

    }

    public enum Relation
    {
        Myself,
        MySon,
        MyDaughter,
        MyRelative
    }

    public enum MarriedStatus
    {
        Single,
        Divorced,
        Widowed
    }

    public class Address
    {
        [Key]
        public long Id { get; set; }
        public string HouseNumber { get; set; }
        public string RoadNumber { get; set; }
        public string District { get; set; }
    }
}
