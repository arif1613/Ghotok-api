using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using Ghotok.Data.UnitOfWork;
using Moq;

namespace Ghotok.Api.Test.TestHelpers.TestHelpers
{
    public class UnitOfWorkTestHelpers
    {
        public static Mock<IRepository<AppUser>> appuserMockRepo;
        public static Mock<IRepository<User>> userMockRepo;

        public static Mock<IUnitOfWork> MockAppuserUnitOfWork()
        {
            appuserMockRepo = RepoTestHelpers.AppuserMockRepo();
            var mockUnitofwork=new Mock<IUnitOfWork>();
            mockUnitofwork.Setup(r => r.AppUseRepository).Returns(appuserMockRepo.Object);
            mockUnitofwork.Setup(r => r.AppUseRepository.Insert(It.IsAny<AppUser>())).Verifiable();
            return mockUnitofwork;
        }

        public static Mock<IUnitOfWork> MockUserUnitOfWork()
        {
            userMockRepo = RepoTestHelpers.UserMockRepo();
            var mockUnitofwork = new Mock<IUnitOfWork>();
            mockUnitofwork.Setup(r => r.UserRepository).Returns(userMockRepo.Object);
            mockUnitofwork.Setup(r => r.UserRepository.Insert(It.IsAny<User>())).Verifiable();
            return mockUnitofwork;
        }
    }
}
