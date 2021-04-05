using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using Ghotok.Data.Repo;
using Ghotok.Data.UnitOfWork;
using GhotokApi.MediatR.Handlers;
using GhotokApi.MediatR.NotificationHandlers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhotokApi.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;


        public UserService(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }


        public async Task<List<User>> GetUsers(Expression<Func<User, bool>> filter, bool isPublished, bool hasOrderBy = false, bool hasInclude = false, bool isLookingForBride = false,
            int startIndex = 0, int chunkSize = 0)
        {
            IEnumerable<User> users;
            if (hasInclude && hasOrderBy)
            {
                users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                    r => r.LookingForBride == isLookingForBride && r.IsPublished == isPublished,
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
                    r => r.LookingForBride == isLookingForBride && r.IsPublished == isPublished,
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
                    r => r.LookingForBride == isLookingForBride && r.IsPublished == isPublished,
                    orderBy: source => source.OrderBy(r => r.BasicInfo.Name),
                    null,
                    isLookingForBride, startIndex, chunkSize, true));
                return users.ToList();

            }


            users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                r => r.LookingForBride == isLookingForBride && r.IsPublished == isPublished,
                null, null, isLookingForBride, startIndex, chunkSize, true));
            return users.ToList();
        }

        public async Task<User> GetUser(Expression<Func<User, bool>> filter, bool hasInclude = false, bool isLookingForBride = false)
        {
            User user = null;
            IEnumerable<User> users;


            if (hasInclude)
            {
                users = await Task.Run(() => _unitOfWork.UserRepository.Get(
                     filter, null,
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
                    r => r.LookingForBride == isLookingForBride, null, null, isLookingForBride));

                if (users.Any())
                {
                    user = users.FirstOrDefault();
                }
            }

            return user;
        }

        public async Task<List<User>> GetRecentUsers(Expression<Func<User, bool>> filter, bool isLookingForBride = false)
        {
            var users = await Task.Run(() => _unitOfWork.UserRepository.GetRecent(
                r => r.LookingForBride == isLookingForBride && r.IsPublished,
                IncludeProperties.UserIncludingAllProperties, isLookingForBride));
            return users.ToList();
        }

        public async Task InsertUser(User User)
        {
            var result = await _mediator.Send(new AddUserInfoRequest
            {
                UserToAdd = User
            });

            if (result == "Done")
            {
                await _mediator.Publish(new ComitDatabaseNotification());
            }
        }

        public async Task InsertUsers(List<User> Users)
        {
            var result = await _mediator.Send(new AddUserInfosRequest
            {
                UsersToAdd = Users
            });

            if (result == "Done")
            {
                await _mediator.Publish(new ComitDatabaseNotification());
            }
        }

        public async Task UpdateUser(User User)
        {
            var result = await _mediator.Send(new UpdateUserInfoRequest
            {
                UserTobeUpdated = User
            });

            if (result == "Done")
            {
                await _mediator.Publish(new ComitDatabaseNotification());
            }
        }

        public async Task DeleteUser(User User)
        {
            var result = await _mediator.Send(new DeleteUserInfoRequest
            {
                UserTobeDeleted = User
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
