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
using GhotokApi.Models.RequestModels;
using GhotokApi.Services;
using GhotokApi.Utils.FilterBuilder;
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
        private Mock<IFilterBuilder> _filterBuilderMock;

        private IConfiguration Configuration;
        private CacheHelper CacheHelper;
        private IEnumerable<AppUser> appUsers;


        [TestInitialize]
        public void Setup()
        {
            _unitOfWorkMock = UnitOfWorkTestHelpers.MockAppuserUnitOfWork();
            _mediatorMock = CommonTestHelpers.MockComponent<IMediator>();
            _filterBuilderMock = CommonTestHelpers.MockComponent<IFilterBuilder>();
            CacheHelper = CommonTestHelpers.GetCacheHelper();
            Configuration = CommonTestHelpers.GetConfiguration();


        }

        [TestMethod]
        [TestCategory("AppUser service")]
        public void AppUser_Repo_Will_Return_Valid_AppUsers_Groom()
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
            Assert.AreEqual(result.Count,30);

            Assert.AreEqual(result.FirstOrDefault().MobileNumber, "mobilenumber 0");
        }

        [TestMethod]
        [TestCategory("AppUser service")]
        public void AppUser_Repo_Will_Return_Valid_AppUsers_Bride()
        {
            var dic = new Dictionary<string, string>();
            dic.Add("IsLookingForBride", "false");
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
            Assert.AreEqual(result.Count, 30);

            Assert.AreEqual(result.FirstOrDefault().MobileNumber, "mobilenumber 0");
        }

        [TestMethod]
        [TestCategory("AppUser service")]
        public void AppUser_Repo_Will_Return_Valid_AppUser()
        {
           var appUserService = new AppUserService(_unitOfWorkMock.Object,_mediatorMock.Object, _filterBuilderMock.Object);
            var result = appUserService.GetAppUser().GetAwaiter().GetResult();
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
