using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ghotok.Api.Test.TestHelpers;
using Ghotok.Api.Test.TestHelpers.TestHelpers;
using Ghotok.Data.DataModels;
using Ghotok.Data.Utils.Cache;
using GhotokApi.MediatR.Handlers;
using GhotokApi.Models.RequestModels;
using GhotokApi.Services;
using GhotokApi.Utils.FilterBuilder;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QQuery.UnitOfWork;

namespace Ghotok.Api.Test.ServiceTests
{
    [TestClass]
    public class AppUserServiceTests
    {
        private Mock<IQqService<AppUser>> _unitOfWorkMock;
        private Mock<IMediator> _mediatorMock;
        private Mock<IFilterBuilder> _filterBuilderMock;
        private IEnumerable<AppUser> appUsers;


        [TestInitialize]
        public void Setup()
        {
            appUsers = Enumerable.Range(0, 50).Select(r => new AppUser
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
            }).AsQueryable();

            _unitOfWorkMock = UnitOfWorkTestHelpers.MockAppuserUnitOfWork();
            _mediatorMock = CommonTestHelpers.MockComponent<IMediator>();
            _filterBuilderMock = CommonTestHelpers.MockComponent<IFilterBuilder>();

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAppUsersRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(appUsers.ToList());

        }

        [TestMethod]
        [TestCategory("AppUser service")]
        public void AppUser_Repo_Will_Return_Valid_AppUsers()
        {
            var dic=new Dictionary<string,string>();
            dic.Add("IsLookingForBride", "true");
            var appUserService = new AppUserService(_unitOfWorkMock.Object, _mediatorMock.Object, _filterBuilderMock.Object);
            var result = appUserService.GetAppUsers(new AppUserInfosRequestModel
            {
                ChunkSize = 0,
                HasInclude = false,
                HasOrderBy = false,
                Filters = new List<IDictionary<string, string>>
                {
                    dic
                }
            }).GetAwaiter().GetResult();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count,50);

            Assert.AreEqual(result.FirstOrDefault().MobileNumber, "mobilenumber 0");
        }


        [TestMethod]
        [TestCategory("AppUser service")]
        public void AppUser_Repo_Will_Return_Valid_AppUser()
        {
            var dic = new Dictionary<string, string>();
            dic.Add("IsLookingForBride", "false");
            var appUserService = new AppUserService(_unitOfWorkMock.Object,_mediatorMock.Object, _filterBuilderMock.Object);
            var result = appUserService.GetAppUser(r=>r.LookingForBride).GetAwaiter().GetResult();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.MobileNumber, "mobilenumber 0");
        }

        [TestCleanup]
        public void ClenUp()
        {
            _unitOfWorkMock.Reset();
            _mediatorMock.Reset();
            _filterBuilderMock.Reset();
        }


    }
}
