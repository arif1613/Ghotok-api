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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ghotok.Data.Test.RepositoryTests
{
    [TestClass]
    public class AppUserRepoTests
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
            });

            var AppUserDbSet = RepositoryTestHelper.CreateMockDbSet(AppUsers.AsQueryable());
            _mockDbContext.Setup(r => r.GetDbSet<AppUser>()).Returns(AppUserDbSet.Object);

        }

        [TestMethod]
        [TestCategory("AppUser repo")]
        public void AppUser_Repo_Will_Return_Valid_AppUsers()
        {
            var appUserRepo=new GenericRepository<AppUser>(_mockDbContext.Object,CacheHelper,Configuration);
            var result = appUserRepo.Get(new List<Expression<Func<AppUser, bool>>>
            {
                r=>r.IsLoggedin
            });
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(),100);
        }

        [TestMethod]
        [TestCategory("AppUser repo")]
        public void AppUser_Repo_Will_Return_Valid_Ordered_AppUsers()
        {
            var appUserRepo = new GenericRepository<AppUser>(_mockDbContext.Object, CacheHelper, Configuration);
            var result = appUserRepo.Get(new List<Expression<Func<AppUser, bool>>>
            {
                r=>r.IsLoggedin
            }, orderBy:source=>source.OrderByDescending(r=>r.Email));
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 100);
            Assert.AreEqual(result.ToList()[10].Email,"Email 9");
        }

        [TestMethod]
        [TestCategory("AppUser repo")]
        public void AppUser_Repo_Will_Return_Valid_Total_AppUsers()
        {
            var appUserRepo = new GenericRepository<AppUser>(_mockDbContext.Object, CacheHelper, Configuration);
            var result = appUserRepo.Get(new List<Expression<Func<AppUser, bool>>>
            {
                r=>r.IsLoggedin
            }, orderBy: null,null,false,10,5);
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
