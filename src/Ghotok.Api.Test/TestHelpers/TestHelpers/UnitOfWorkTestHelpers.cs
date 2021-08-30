using Ghotok.Data.DataModels;
using Moq;
using QQuery.Repo;
using QQuery.UnitOfWork;

namespace Ghotok.Api.Test.TestHelpers.TestHelpers
{
    public class UnitOfWorkTestHelpers
    {
        public static Mock<IQuickQueryRepository<AppUser>> appuserMockRepo;
        public static Mock<IQuickQueryRepository<User>> userMockRepo;

        public static Mock<IQqService<AppUser>> MockAppuserUnitOfWork()
        {
            appuserMockRepo = RepoTestHelpers.AppuserMockRepo();
            var mockUnitofwork=new Mock<IQqService<AppUser>>();
            mockUnitofwork.Setup(r => r.QqRepository).Returns(appuserMockRepo.Object);
            mockUnitofwork.Setup(r => r.QqRepository.Insert(It.IsAny<AppUser>())).Verifiable();
            return mockUnitofwork;
        }

        public static Mock<IQqService<User>> MockUserUnitOfWork()
        {
            userMockRepo = RepoTestHelpers.UserMockRepo();
            var mockUnitofwork = new Mock<IQqService<User>>();
            mockUnitofwork.Setup(r => r.QqRepository).Returns(userMockRepo.Object);
            mockUnitofwork.Setup(r => r.QqRepository.Insert(It.IsAny<User>())).Verifiable();
            return mockUnitofwork;
        }
    }
}
