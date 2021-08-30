using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ghotok.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Moq;
using QQuery.Context;
using QQuery.Repo;

namespace Ghotok.Api.Test.TestHelpers.TestHelpers
{
    public class RepoTestHelpers
    {
        public static Mock<IQqContext> GhotokContextMock;
        public static Mock<DbSet<AppUser>> AppUserMockSet;
        public static Mock<IQuickQueryRepository<AppUser>> AppUserRepo;
        public static Mock<IQuickQueryRepository<User>> UserRepo;



        public static Mock<IQuickQueryRepository<AppUser>> AppuserMockRepo()
        {
            var brides = Enumerable.Range(0, 50).Select(r => new AppUser
            {
                CountryCode = $"CountryCode {r}",
                Email = $"Email {r}",
                Id = Guid.NewGuid(),
                IsLoggedin = true,
                IsVarified = true,
                LanguageChoice = Language.English,
                LoggedInDevices = 0,
                LookingForBride = false,
                MobileNumber = $"mobilenumber {r}",
                Password = $"12345{r}",
                UserRole = $"role{r}"
            });

            var grooms = Enumerable.Range(0, 30).Select(r => new AppUser
            {
                CountryCode = $"CountryCode {r}",
                Email = $"Email {r}",
                Id = Guid.NewGuid(),
                IsLoggedin = true,
                IsVarified = true,
                LanguageChoice = Language.English,
                LoggedInDevices = 0,
                LookingForBride = true,
                MobileNumber = $"mobilenumber {r}",
                Password = $"12345{r}",
                UserRole = $"role{r}"
            });


            GhotokContextMock = ContextTestHelpers.MockContext<IQqContext>();
            AppUserMockSet = ContextTestHelpers.CreateMockDbSet(brides.AsQueryable());
            GhotokContextMock.Setup(r => r.GetDbSet<AppUser>()).Returns(AppUserMockSet.Object);
            AppUserRepo = new Mock<IQuickQueryRepository<AppUser>>();
            AppUserRepo.Setup(r => r.Get(It.IsAny<IEnumerable<Expression<Func<AppUser,bool>>>>(),  null,null,0,0,true))
                .Returns(brides.ToList());

            AppUserRepo.Setup(r => r.Get(It.IsAny<IEnumerable<Expression<Func<AppUser, bool>>>>(), null, null,  0, 0, true))
                .Returns(grooms.ToList());

            return AppUserRepo;
        }

        public static Mock<IQuickQueryRepository<User>> UserMockRepo()
        {
            var users = Enumerable.Range(0, 100).Select(r => new User
            {
                Id = Guid.NewGuid(),
                BasicInfo = new BasicInfo
                {
                    ContactNumber = $"basicinfo_contactnumber",
                    Dob = DateTime.UtcNow-TimeSpan.FromDays(20*365),
                    email = $"basicinfo_email",
                    Height_cm = 10+r,
                    Id = Guid.NewGuid(),
                    MaritalStatus = "unmarried",
                    MaternalHomeDistrict = $"maternaldistrict{r}",
                    Name = $"basicinfo_name{r}",
                    OtherBasicInfo = $"otherbasicinfo{r}",
                    PaternalHomeDistrict = $"paternaldistrict{r}"
                },
                CountryCode = $"countrycode{r}",
                EducationInfo = new EducationInfo
                {
                    CurrentJob = new CurrentProfession
                    {
                        Id = Guid.NewGuid(),
                        OfficeName = $"officename{r}",
                        JobDesignation = $"jobdescription{r}",
                        SalaryRange = $"salaryrange{r}"
                    },
                    Educations = new List<Education>
                    {
                        new Education
                        {
                            Id = Guid.NewGuid(),
                            InstituteName = $"institutename{r}",
                            Degree = $"degree{r}",
                            PassingYear = $"passingyear{r}",
                            Result = $"result{r}"
                        }
                    }
                },
                Email = $"email{r}",
                FamilyInfo = new FamilyInfo
                {
                    Id = Guid.NewGuid(),
                    FamilyMembers = new List<FamilyMember>
                    {
                        new FamilyMember
                        {
                            Id = Guid.NewGuid(),
                            FamilyMemberName = $"familymemebername{r}",
                            FamilyMemberOccupation = $"familymemeberoccupation{r}",
                            Relationship = $"familymemebrrelationship{r}"
                        }
                    }
                },
                IsPictureUploaded = false,
                IsPublished = true,
                LanguageChoice = Language.English,
                LookingForBride = false
                
            });

            var userContextMock = ContextTestHelpers.MockContext<IQqContext>();
            var userMockSet = ContextTestHelpers.CreateMockDbSet(users.AsQueryable());
            userContextMock.Setup(r => r.GetDbSet<User>()).Returns(userMockSet.Object);
            UserRepo = new Mock<IQuickQueryRepository<User>>();
            UserRepo.Setup(r => r.Get(null, null,null,0,0,true)).Returns(users.ToList());

            return UserRepo;
        }
    }
}
