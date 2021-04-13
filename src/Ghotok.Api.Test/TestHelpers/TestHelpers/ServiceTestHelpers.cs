using Moq;

namespace Ghotok.Api.Test.TestHelpers.TestHelpers
{
    public class ServiceTestHelpers
    {
        public static Mock<T> MockService<T>() where T : class
        {
            return new Mock<T>();
        }
    }
}
