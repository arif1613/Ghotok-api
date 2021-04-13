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


        public async Task<List<User>> GetUsers(UserInfosRequestModel model, bool hasOrderBy = false, bool hasInclude = false,
            int startIndex = 0, int chunkSize = 0)
        {
            var isLookingForBride = false;
            foreach (var filter in model.Filters)
            {
                if (filter.Key == UserFilter.IslookingForBride.ToString())
                {
                    isLookingForBride = Convert.ToBoolean(filter.Value);
                }
            }
            IEnumerable<User> users;
            if (hasInclude && hasOrderBy)
            {
                users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                    _filterBuilder.GetUserFilter(model.Filters),
                    orderBy: source => source.OrderByDescending(r => r.ValidFrom),
                    include: s => s
                        .Include(a => a.BasicInfo)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(a => a.FamilyInfo).ThenInclude(d => d.FamilyMembers),
                    isLookingForBride, startIndex, chunkSize, true));
                return users.ToList();
            }

            if (hasInclude)
            {
                users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                    _filterBuilder.GetUserFilter(model.Filters),
                    null,
                    include: s => s
                        .Include(a => a.BasicInfo)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(a => a.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(a => a.FamilyInfo).ThenInclude(d => d.FamilyMembers),
                    isLookingForBride, startIndex, chunkSize, true));
                return users.ToList();


            }

            if (hasOrderBy)
            {
                users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                    _filterBuilder.GetUserFilter(model.Filters),
                    orderBy: source => source.OrderBy(r => r.BasicInfo.Name),
                    null,
                    isLookingForBride, startIndex, chunkSize, true));
                return users.ToList();

            }


            users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                _filterBuilder.GetUserFilter(model.Filters),
                null, null, isLookingForBride, startIndex, chunkSize, true));
            return users.ToList();
        }

        public async Task<User> GetUser(UserInfoRequestModel model, bool hasInclude = false)
        {
            User user = null;
            IEnumerable<User> users;
            var isLookingForBride = false;
            foreach (var filter in model.Filters)
            {
                if (filter.Key == UserFilter.IslookingForBride.ToString())
                {
                    isLookingForBride = Convert.ToBoolean(filter.Value);
                }
            }

            if (hasInclude)
            {
                users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                    _filterBuilder.GetUserFilter(model.Filters),
                     null,
                     include: s => s
                         .Include(a => a.BasicInfo)
                         .Include(a => a.EducationInfo).ThenInclude(b => b.Educations)
                         .Include(a => a.EducationInfo).ThenInclude(b => b.CurrentJob)
                         .Include(a => a.FamilyInfo).ThenInclude(d => d.FamilyMembers),
                     isLookingForBride));

                if (users.Any())
                {
                    user = users.FirstOrDefault();
                }

            }
            else
            {
                users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                    _filterBuilder.GetUserFilter(model.Filters), null, null, isLookingForBride));

                if (users.Any())
                {
                    user = users.FirstOrDefault();
                }
            }

            return user;
        }

        public async Task<List<User>> GetRecentUsers(UserInfosRequestModel model)
        {
            var isLookingForBride = false;
            foreach (var filter in model.Filters)
            {
                if (filter.Key == UserFilter.IslookingForBride.ToString())
                {
                    isLookingForBride = Convert.ToBoolean(filter.Value);
                }
            }
            var users = await Task.Run(() => _unitOfWork.UserRepository.GetRecent(
                _filterBuilder.GetUserFilter(model.Filters),
                IncludeProperties.UserIncludingAllProperties, isLookingForBride));
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
