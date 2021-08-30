using System;
using System.Collections.Generic;
using System.Linq;
using Ghotok.Api.Test.TestHelpers;
using Ghotok.Api.Test.TestHelpers.TestHelpers;
using Ghotok.Data.DataModels;
using Ghotok.Data.Utils.Cache;
using GhotokApi.Models.RequestModels;
using GhotokApi.Services;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QQuery.UnitOfWork;

namespace Ghotok.Api.Test.ServiceTests
{
    [TestClass]
    public class LoginServiceTests
    {

        private Mock<IMediator> _mediatorMock;
        private Mock<IQqService<AppUser>> _unitOfWorkMock;
        private CacheHelper CacheHelper;
        private IEnumerable<AppUser> appUsers1;
        private IEnumerable<AppUser> appUsers2;

        [TestInitialize]
        public void Setup()
        {

            _mediatorMock = CommonTestHelpers.MockComponent<IMediator>();
            CacheHelper = CommonTestHelpers.GetCacheHelper();
            _unitOfWorkMock = UnitOfWorkTestHelpers.MockAppuserUnitOfWork();

            appUsers1 = Enumerable.Range(0, 5).Select(r => new AppUser
            {
                CountryCode = $"CountryCode {r}",
                Email = $"Email {r}",
                RegisterByMobileNumber = true,
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

            appUsers2 = Enumerable.Range(0, 5).Select(r => new AppUser
            {
                CountryCode = $"CountryCode {r}",
                Email = $"Email {r}",
                RegisterByMobileNumber = false,
                Id = Guid.NewGuid(),
                IsLoggedin = true,
                IsVarified = true,
                LanguageChoice = Language.English,
                LoggedInDevices = 3,
                LookingForBride = false,
                MobileNumber = $"mobilenumber {r}",
                Password = $"12345{r}",
                UserRole = $"role{r}"
            });
        }


        [TestMethod]
        [Ignore]
        public void Will_Return_LoggedIn_User()
        {
            var loginservice = new LoginService(_mediatorMock.Object,_unitOfWorkMock.Object);

            var result = loginservice.IsUserLoggedInAsync(new OtpRequestModel
            {
                CountryCode = $"CountryCode 1",
                Email = "Email 1",
                MobileNumber = "mobilenumber 1",
                Password = "123451",
                RegisterByMobileNumber = true
            }).GetAwaiter().GetResult();

            Assert.IsNotNull(result);
            Assert.AreEqual(result, true);
        }


        [TestCleanup]
        public void Cleanup()
        {

        }
    }
}
