using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using Ghotok.Data.UnitOfWork;
using GhotokApi.MediatR.Handlers;
using GhotokApi.MediatR.NotificationHandlers;
using GhotokApi.Models.RequestModels;
using GhotokApi.Utils.FilterBuilder;
using MediatR;

namespace GhotokApi.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IFilterBuilder _filterBuilder;



        public UserService(IUnitOfWork unitOfWork, IMediator mediator, IFilterBuilder filterBuilder)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _filterBuilder = filterBuilder;
        }


        public async Task<List<User>> GetUsers(UserInfosRequestModel model)
        {
            return await _mediator.Send(new GetUsersRequest
            {
                model = model
            });

            
        }

        public async Task<User> GetUser(UserInfoRequestModel model)
        {
          return await _mediator.Send(new GetUserRequest
            {
                model = model
            });
        }

        public async Task<dynamic> GetRecentUsers(UserInfosRequestModel model)
        {
            var users = await Task.Run(() => _unitOfWork.UserRepository.GetRecent(
                _filterBuilder.GetUserFilter(model.Filters),
                IncludeProperties.UserIncludingAllProperties));
            return users;
        }

        public async Task InsertUser(User user)
        {
            var result = await _mediator.Send(new AddUserInfoRequest
            {
                UserToAdd = user
            });

            if (result == "Done")
            {
                await _mediator.Publish(new ComitDatabaseNotification());
            }
        }

        public async Task InsertUsers(List<User> users)
        {
            var result = await _mediator.Send(new AddUserInfosRequest
            {
                UsersToAdd = users
            });

            if (result == "Done")
            {
                await _mediator.Publish(new ComitDatabaseNotification());
            }
        }

        public async Task UpdateUser(User user)
        {
            var result = await _mediator.Send(new UpdateUserInfoRequest
            {
                UserTobeUpdated = user
            });

            if (result == "Done")
            {
                await _mediator.Publish(new ComitDatabaseNotification());
            }
        }

        public async Task DeleteUser(User user)
        {
            var result = await _mediator.Send(new DeleteUserInfoRequest
            {
                UserTobeDeleted = user
            });

            if (result == "Done")
            {
                await _mediator.Publish(new ComitDatabaseNotification());
            }
        }

        public async Task SaveDatabse()
        {
            await Task.Run(() => _unitOfWork.Commit());
        }
    }
}
