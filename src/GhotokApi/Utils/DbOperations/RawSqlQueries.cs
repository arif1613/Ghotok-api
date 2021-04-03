using System;

namespace GhotokApi.Utils.DbOperations
{
    public static class RawSqlQueries
    {
        public static string GetAllUsersIncludingBasicInfos =
            $"SELECT * FROM Users u INNER JOIN BasicInfo b on b.Id=u.BasicInfoId INNER JOIN dbo.Address ad on ad.Id=b.PresentAddressId";
        public static string GetAllUsersIncludingEducationInfos =
            $"SELECT * FROM Users u INNER JOIN EducationInfo eduinfo on eduinfo.Id=u.EducationInfoId INNER JOIN CurrentProfession cp on cp.Id=eduinfo.CurrentJobId INNER JOIN Education edu on edu.EducationInfoId=eduinfo.Id";



        public static FormattableString GetAllAppUsers()
        {
            return $"SELECT * FROM AppUsers";
        }

        public static FormattableString GetAllBrideAppUsers()
        {
            return $"SELECT * FROM [GhotokApiDb].[dbo].[AppUsers] u WHERE u.LookingForBride=0";
        }

        public static FormattableString GetAllGroomAppUsers()
        {
            return $"SELECT * FROM [GhotokApiDb].[dbo].[AppUsers] u WHERE u.LookingForBride=1";
        }

        public static FormattableString GetAllUsers()
        {
            return $"SELECT * FROM Users";
        }

        public static FormattableString GetAllBrideUsers()
        {
            return $"SELECT * FROM [GhotokApiDb].[dbo].[Users] u WHERE u.LookingForBride=0";
        }
        public static FormattableString GetAllGroomUsers()
        {
            return $"SELECT * FROM [GhotokApiDb].[dbo].[Users] u WHERE u.LookingForBride=1";
        }
        public static FormattableString GetAllUsersIncludingAllProperties(bool isLookingForBride, bool ispublished)
        {
            var lookingForBride = Convert.ToInt32(isLookingForBride);
            var isPublished = Convert.ToInt32(ispublished);
            return $"SELECT * FROM [GhotokApiDb].[dbo].[Users] u INNER JOIN [GhotokApiDb].[dbo].[BasicInfo] b on b.Id = u.BasicInfoId INNER JOIN [GhotokApiDb].[dbo].[EducationInfo] eduinfo on eduinfo.Id = u.EducationInfoId INNER JOIN [GhotokApiDb].[dbo].[FamilyInfo] famiinfo on famiinfo.Id = u.FamilyInfoId INNER JOIN [GhotokApiDb].[dbo].[Address] ad on ad.Id = b.PresentAddressId INNER JOIN [GhotokApiDb].[dbo].[CurrentProfession] cp on cp.Id = eduinfo.CurrentJobId INNER JOIN [GhotokApiDb].[dbo].[Education] edu on edu.EducationInfoId = eduinfo.Id INNER JOIN [GhotokApiDb].[dbo].[FamilyMember] famimem on famimem.FamilyInfoId = famiinfo.Id ORDER BY u.Id";


        }

    }
}
