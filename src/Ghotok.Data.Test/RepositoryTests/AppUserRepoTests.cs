using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ghotok.Data.DataModels;
using Ghotok.Data.Test.TestHelpers;
using Ghotok.Data.Utils.Cache;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QQuery.Context;
using QQuery.Repo;

namespace Ghotok.Data.Test.RepositoryTests
{
    [TestClass]
    public class AppUserRepoTests
    {

        private Mock<IQqContext> _mockDbContext;


        [TestInitialize]
        public void Setup()
        {
            _mockDbContext = RepositoryTestHelper.MockContext<IQqContext>();

            var AppUsers = Enumerable.Range(0, 100).Select(r => new AppUser
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
            }).AsQueryable();

            var AppUserDbSet = RepositoryTestHelper.CreateMockDbSet(AppUsers);
            _mockDbContext.Setup(r => r.GetDbSet<AppUser>()).Returns(AppUserDbSet.Object);

        }

        [TestMethod]
        [TestCategory("AppUser repo")]
        public void AppUser_Repo_Will_Return_Valid_AppUsers()
        {
            var appUserRepo=new QuickQueryRepository<AppUser>(_mockDbContext.Object);
            var result = appUserRepo.Get(new List<Expression<Func<AppUser, bool>>>
            {
                r=>r.IsLoggedin==true
            }, null, null, 0, 0, true);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(),100);
        }

        [TestMethod]
        [TestCategory("AppUser repo")]
        public void AppUser_Repo_Will_Return_Valid_Ordered_AppUsers()
        {
            var appUserRepo = new QuickQueryRepository<AppUser>(_mockDbContext.Object);
            var result = appUserRepo.Get(new List<Expression<Func<AppUser, bool>>>
            {
                r=>r.IsLoggedin==true
            },  orderBy:source=>source.OrderByDescending(r=>r.Email));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 100);
            Assert.AreEqual(result.ToList()[10].Email,"Email 9");
        }

        [TestMethod]
        [TestCategory("AppUser repo")]
        public void AppUser_Repo_Will_Return_Valid_Total_AppUsers()
        {
            var appUserRepo = new QuickQueryRepository<AppUser>(_mockDbContext.Object);
            var result = appUserRepo.Get(new List<Expression<Func<AppUser, bool>>>
            {
                r=>r.IsLoggedin==true
            },  orderBy: null,null,10,5);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual(result.ToList()[1].MobileNumber, "mobilenumber 11");
        }

        [TestCleanup]
        public void CleanUp()
        {
            _mockDbContext.Reset();
        }
    }
}
