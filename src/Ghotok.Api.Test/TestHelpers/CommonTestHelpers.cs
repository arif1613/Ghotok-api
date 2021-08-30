using System.Collections.Generic;
using Ghotok.Data.Utils.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Ghotok.Api.Test.TestHelpers
{
    public class CommonTestHelpers
    {
        public static IConfiguration GetConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string> {

                {"GhotokDbConnectionString", "Server=.\\SQLEXPRESS;Database=GhotokApiDb_Guid;Trusted_Connection=True;MultipleActiveResultSets=true"},
                {"RepoCacheKeyAppUser", "AppUserRepo"},
                {"RepoCacheKeyUser", "UserRepo"},
                {"RepoCacheMinute", "15"},
                {"OtpValidMinute", "15"},
                {"AppUserCacheMinute", "300"},
                {"RecentUserCacheMinute", "15"},
                {"UserInfoCacheChunkSize", "20"},
                {"UserCacheChunkSize", "60"},
                {"OtpCaheMinute", "30"}
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build() as IConfiguration;
        }


        public static CacheHelper GetCacheHelper()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var imemoryCache = serviceProvider.GetService<IMemoryCache>();
            return new CacheHelper(imemoryCache);
        }

        public static Mock<T> MockComponent<T>() where T : class
        {
            return new Mock<T>();
        }

        public static Mock<T> MockContext<T>() where T : class
        {
            return new Mock<T>();
        }

        //public static Mock<IUnitOfWork> MockAppUserUnitOfWork()
        //{
        //    var appUserMockRepo = CommonTestHelpers.OrderMockRepo();
        //    var mockUnitofwork = new Mock<IUnitOfWork>();
        //    mockUnitofwork.Setup(r => r.OrderRepository).Returns(orderMockRepo.Object);
        //    mockUnitofwork.Setup(r => r.OrderRepository.Insert(It.IsAny<Order>())).Verifiable();
        //    return mockUnitofwork;
        //}
    }
}
