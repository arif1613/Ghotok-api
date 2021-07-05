using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using Ghotok.Data.UnitOfWork;
using GhotokApi.Common;
using GhotokApi.MediatR.Handlers;
using GhotokApi.MediatR.NotificationHandlers;
using GhotokApi.Models.RequestModels;
using GhotokApi.Utils.FilterBuilder;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            IEnumerable<User> users;
            if (model.HasInclude && model.HasOrderBy)
            {
                users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                    _filterBuilder.GetUserFilter(model.Filters),
                    orderBy: source => source.OrderByDescending(r => r.ValidFrom),
                    include: s => s
                        .Include(a => a.BasicInfo)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(a => a.FamilyInfo).ThenInclude(d => d.FamilyMembers),
                     model.StartIndex, model.ChunkSize, true));
                return users.ToList();
            }

            if (model.HasInclude)
            {
                users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                    _filterBuilder.GetUserFilter(model.Filters),
                    null, include: s => s
                        .Include(a => a.BasicInfo)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(a => a.FamilyInfo).ThenInclude(d => d.FamilyMembers),
                     model.StartIndex, model.ChunkSize, true));
                return users.ToList();


            }

            if (model.HasOrderBy)
            {
                users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                    _filterBuilder.GetUserFilter(model.Filters),
                    orderBy: source => source.OrderBy(r => r.BasicInfo.Name),
                    null, model.StartIndex, model.ChunkSize, true));
                return users.ToList();

            }


            users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                _filterBuilder.GetUserFilter(model.Filters), 
                null, null,  model.StartIndex, model.ChunkSize, true));
            return users.ToList();
        }

        public async Task<User> GetUser(UserInfoRequestModel model)
        {
            var result = await _mediator.Send(new GetAppUserRequest
            {
                model = model
            });
            return result;
        }

        public async Task<List<User>> GetRecentUsers(UserInfosRequestModel model)
        {
            var users = await Task.Run(() => _unitOfWork.UserRepository.GetRecent(
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
