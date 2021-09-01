using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using GhotokApi.MediatR.Handlers;
using GhotokApi.MediatR.NotificationHandlers;
using GhotokApi.Models.RequestModels;
using GhotokApi.Utils.FilterBuilder;
using MediatR;
using QQuery.UnitOfWork;

namespace GhotokApi.Services
{
    public class UserService : IUserService
    {

        private readonly IQqService<User> _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IFilterBuilder _filterBuilder;



        public UserService(IQqService<User> unitOfWork, IMediator mediator, IFilterBuilder filterBuilder)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _filterBuilder = filterBuilder;
        }


        public async Task<IQueryable<User>> GetUsers(UserInfosRequestModel model)
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

        public async Task<List<User>> GetRecentUsers(UserInfosRequestModel model)
        {
            var users = await Task.Run(() => _unitOfWork.QqRepository.GetRecent(
                _filterBuilder.GetUserFilter(model.Filters),
                IncludeProperties.UserIncludingAllProperties));
            return users.ToList();
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
