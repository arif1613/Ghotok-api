using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Ghotok.Api.Test.TestHelpers;
using Ghotok.Api.Test.TestHelpers.TestHelpers;
using Ghotok.Data.Context;
using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using Ghotok.Data.UnitOfWork;
using Ghotok.Data.Utils.Cache;
using GhotokApi.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Ghotok.Api.Test.ServiceTests
{
    [TestClass]
    public class AppUserServiceTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMediator> _mediatorMock;
        private IConfiguration Configuration;
        private CacheHelper CacheHelper;
        private IEnumerable<AppUser> appUsers;


        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = UnitOfWorkTestHelpers.MockAppuserUnitOfWork();
            _mediatorMock = CommonTestHelpers.MockComponent<IMediator>();
            CacheHelper = CommonTestHelpers.GetCacheHelper();
            Configuration = CommonTestHelpers.GetConfiguration();

            appUsers = Enumerable.Range(0, 100).Select(r => new AppUser
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

            //var AppUserDbSet = RepositoryTestHelper.CreateMockDbSet(AppUsers.AsQueryable());
            //var DbContext = RepositoryTestHelper.MockContext<IGhotokDbContext>();
            //DbContext.Setup(r => r.GetDbSet<AppUser>()).Returns(AppUserDbSet.Object);


            //var appUserRepo = new GenericRepository<AppUser>(DbContext.Object,CacheHelper,Configuration);

            //appUserRepo.Setup(expression: r => r.Get(r=>r.IsLoggedin)).Returns(new List<AppUser>());
            //_unitOfWorkMock
            //    .Setup(r => r.AppUseRepository.Get(It.IsAny<Expression<Func<AppUser, bool>>>(), null, null, false, 0,
            //        10,
            //        true)).Returns(appUsers);

        }

        [TestMethod]
        [TestCategory("AppUser service")]
        public void AppUser_Repo_Will_Return_Valid_AppUsers()
        {
            //_unitOfWorkMock
            //    .Setup(r => r.AppUseRepository.Get(
            //        true)).Returns(appUsers.Where(p => p.LookingForBride == false));

            var appUserService = new AppUserService(_unitOfWorkMock.Object,_mediatorMock.Object);
            var result = appUserService.GetAppUser(null,false,false).GetAwaiter().GetResult();
            Assert.IsNotNull(result);
            //Assert.AreEqual(result.IsLoggedin,true);
            Assert.AreEqual(result.MobileNumber, "mobilenumber 10");

        }


    }
}
