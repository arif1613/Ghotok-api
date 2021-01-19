using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GhotokApi.Repo
{
    public static class IncludeProperties
    {

        public static string AppUserIncludingAllProperties =
            $"User,BasicInfo,FamilyInfo,FamilyInfo.FamilyMembers,EducationInfo,EducationInfo.Educations,EducationInfo.CurrentJob";

        public static string UserIncludingAllProperties =
            $"BasicInfo,FamilyInfo,FamilyInfo.FamilyMembers,EducationInfo,EducationInfo.Educations,EducationInfo.CurrentJob";
        public static string UserIncludingBasicInfos = $"BasicInfo";
        public static string UserIncludingEducationAndJobInfos = $"EducationInfo,EducationInfo.Educations,EducationInfo.CurrentJob";
        public static string UserIncludingFamilyInfos = $"FamilyInfo,FamilyInfo.FamilyMembers";


    }
}
