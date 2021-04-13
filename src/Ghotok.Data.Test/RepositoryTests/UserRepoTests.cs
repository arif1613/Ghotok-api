using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Ghotok.Data.Context;
using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using Ghotok.Data.Test.TestHelpers;
using Ghotok.Data.Utils.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ghotok.Data.Test.RepositoryTests
{
    [TestClass]
    public class UserRepoTests
    {

        private Mock<IGhotokDbContext> _mockDbContext;
        private IConfiguration Configuration;
        private CacheHelper CacheHelper;


        [TestInitialize]
        public void Setup()
        {
            CacheHelper = CommonTestHelpers.GetCacheHelper();
            Configuration = CommonTestHelpers.GetConfiguration();
            _mockDbContext = RepositoryTestHelper.MockContext<IGhotokDbContext>();

            var Users = Enumerable.Range(0, 105).Select(r => new User
            {
                CountryCode = $"CountryCode {r}",
                Email = $"Email {r}",
                Id = Guid.NewGuid(),
                LanguageChoice = Language.English,
                LookingForBride = true,
                MobileNumber = $"mobilenumber {r}",
                IsPublished = true,
                BasicInfo = new BasicInfo
                {
                    ContactNumber = $"contactnumber {r}",
                    Dob = DateTime.UtcNow,
                    email = $"basicinfoemail {r}",
                    Height_cm = r,
                    Id = Guid.NewGuid(),
                    MaritalStatus = "Unmarried",
                    Name = $"name{r}"
                },
                EducationInfo = new EducationInfo
                {
                    CurrentJob = new CurrentProfession
                    {
                        Id = Guid.NewGuid(),
                        JobDesignation = $"manager{r}",
                        OfficeName = $"Officename{r}",
                        SalaryRange = $"{r} - {r*10}"
                    },
                    Educations = new List<Education>
                    {
                        new Education
                        {
                            Degree = $"degree{r}",
                            Id = Guid.NewGuid(),
                            InstituteName = $"institutename{r}",
                            PassingYear = $"{r+1980}",
                            Result = $"result{r}"
                        }
                    }
                }
            });

            var UserDbSet = RepositoryTestHelper.CreateMockDbSet(Users.AsQueryable());
            _mockDbContext.Setup(r => r.GetDbSet<User>()).Returns(UserDbSet.Object);

        }

        [TestMethod]
        [TestCategory("User repo")]
        public void User_Repo_Will_Return_Valid_Users()
        {
            var UserRepo=new GenericRepository<User>(_mockDbContext.Object,CacheHelper,Configuration);
            var result = UserRepo.Get(new List<Expression<Func<User, bool>>>
            {
                r=>r.IsPublished
            });
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(),105);
        }

        [TestMethod]
        [TestCategory("User repo")]
        public void User_Repo_Will_Return_Valid_Ordered_Users()
        {
            var UserRepo = new GenericRepository<User>(_mockDbContext.Object, CacheHelper, Configuration);
            var result = UserRepo.Get(new List<Expression<Func<User, bool>>>
            {
                r=>r.IsPublished
            },orderBy:source=>source.OrderByDescending(r=>r.Email));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 105);
            Assert.AreEqual(result.ToList()[10].Email,"Email 9");
        }

        [TestMethod]
        [TestCategory("User repo")]
        public void User_Repo_Will_Return_Valid_Total_Users()
        {
            var UserRepo = new GenericRepository<User>(_mockDbContext.Object, CacheHelper, Configuration);
            var result = UserRepo.Get(new List<Expression<Func<User, bool>>>
            {
                r=>r.IsPublished
            }, orderBy: null,null,false,10,5);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual(result.ToList()[1].MobileNumber, "mobilenumber 11");
        }

        [TestMethod]
        [TestCategory("User repo")]
        public void User_Repo_Will_Return_Valid_Users_With_Included_Properties()
        {
            var UserRepo = new GenericRepository<User>(_mockDbContext.Object, CacheHelper, Configuration);
            var result = UserRepo.Get(new List<Expression<Func<User, bool>>>
                {
                    r=>r.IsPublished
                }, orderBy: null,
                source => source.Include(r => r.BasicInfo).Include(r => r.EducationInfo).ThenInclude(a => a.Educations)
                    .Include(r=>r.EducationInfo).ThenInclude(b=>b.CurrentJob),
                false, 0, 5);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 5);
            Assert.IsNotNull(result.ToList()[0].BasicInfo);
            Assert.IsNotNull(result.ToList()[0].EducationInfo);
            Assert.IsNotNull(result.ToList()[0].EducationInfo.CurrentJob);
            Assert.IsNotNull(result.ToList()[0].EducationInfo.Educations);

            Assert.AreEqual(result.ToList()[0].BasicInfo.Name, "name0");
            Assert.AreEqual(result.ToList()[0].EducationInfo.CurrentJob.OfficeName, "Officename0");
            Assert.AreEqual(result.ToList()[0].EducationInfo.Educations.ToList()[0].InstituteName, "institutename0");
        }

        [TestMethod]
        [TestCategory("User repo")]
        public void User_Repo_Will_Return_Valid_Recent_Users()
        {
            var UserRepo = new GenericRepository<User>(_mockDbContext.Object, CacheHelper, Configuration);
            var result = UserRepo.GetRecent(new List<Expression<Func<User, bool>>>
            {
                r=>r.IsPublished
            }, null,true);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 5);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _mockDbContext.Reset();
        }
    }
}
