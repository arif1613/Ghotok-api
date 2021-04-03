using System;
using System.Collections.Generic;
using System.Text;
using Ghotok.Data.Utils.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ghotok.Data.Test.UtilsTests.CacheTests
{
    [TestClass]
    public class CacheUtilityTests
    {
        private CacheHelper _cachehelper;
        private TestDataModel TestCacheData;

        [TestInitialize]
        public void Setup()
        {

            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var imemoryCache = serviceProvider.GetService<IMemoryCache>();
            _cachehelper = new CacheHelper(imemoryCache);
            TestCacheData = new TestDataModel
            {
                id = 1,
                name = "testname"
            };
        }

        [TestMethod]
        public void CacheData_Can_Add_Successfuly_For_FiveMinutes()
        {
            _cachehelper.Add(TestCacheData, "testdata", 5);
            var testDataInCache = _cachehelper.Get<TestDataModel>("testdata");
            Assert.IsNotNull(testDataInCache);
            Assert.AreEqual(testDataInCache.id, 1);
            Assert.AreEqual(testDataInCache.name, "testname");
        }

        [TestMethod]
        public void CacheData_Cleared_Successfuly()
        {
            _cachehelper.Add(TestCacheData, "testdata", 5);
            _cachehelper.Clear("testdata");
            var testDataInCache = _cachehelper.Get<TestDataModel>("testdata");
            Assert.IsNull(testDataInCache);
        }

        [TestCleanup]
        public void CleanUp()
        {

        }

        public class TestDataModel
        {
            public int id { get; set; }
            public string name { get; set; }

        }
    }
}
