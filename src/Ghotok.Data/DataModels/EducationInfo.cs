using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ghotok.Data.DataModels
{
    public class EducationInfo
    {
        [Key]
        public Guid Id { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual CurrentProfession CurrentJob { get; set; }

    }

    public class Education
    {

        [Key]
        public Guid Id { get; set; }
        public string Degree { get; set; }
        public string InstituteName { get; set; }
        public string PassingYear { get; set; }
        public string Result { get; set; }
    }

    public enum DegreeName
    {
        SSC,
        HSC,
        BA,
        BCom,
        BSc,
        MA,
        MCom,
        MSc,
        PhD,
        PostDoc,
        Other
    }

    public class CurrentProfession
    {
        [Key]
        public Guid Id { get; set; }
        public string JobDesignation { get; set; }
        public string OfficeName { get; set; }
        public string SalaryRange { get; set; }
    }

    public enum JobSalaryRange
    {
        lower = 1000,


    }
}
