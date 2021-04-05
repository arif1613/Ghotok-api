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
    #region Interface

    #endregion
    public class AppUserService : IAppUserService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;


        public AppUserService(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }


        public async Task<List<AppUser>> GetAppUsers(Expression<Func<AppUser, bool>> filter, bool isPublished, bool hasOrderBy = false, bool hasInclude = false, bool isLookingForBride = false,
            int startIndex = 0, int chunkSize = 0)
        {
            IEnumerable<AppUser> appUsers = new List<AppUser>();
            if (hasInclude && hasOrderBy)
            {
                appUsers = await Task.Run(() => _unitOfWork.AppUseRepository.Get(
                    r => r.LookingForBride == isLookingForBride && r.User.IsPublished == isPublished,
                    orderBy: source => source.OrderBy(r => r.User.BasicInfo.Name),
                    include: s => s
                        .Include(r => r.User)
                        .ThenInclude(a => a.BasicInfo)
                        .Include(r => r.User)
                        .ThenInclude(b => b.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(r => r.User)
                        .ThenInclude(b => b.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(r => r.User)
                        .ThenInclude(c => c.FamilyInfo).ThenInclude(c => c.FamilyMembers),
                    isLookingForBride, startIndex, chunkSize, true));
                return appUsers.ToList();

            }

            if (hasInclude)
            {
                appUsers = await Task.Run(() => _unitOfWork.AppUseRepository.Get(
                    r => r.LookingForBride == isLookingForBride && r.User.IsPublished == isPublished,
                    null,
                    include: s => s
                        .Include(a => a.User.BasicInfo)
                        .Include(a => a.User.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(a => a.User.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(a => a.User.FamilyInfo).ThenInclude(d => d.FamilyMembers),
                    isLookingForBride, startIndex, chunkSize, true));
                return appUsers.ToList();


            }

            if (hasOrderBy)
            {
                appUsers = await Task.Run(() => _unitOfWork.AppUseRepository.Get(
                    r => r.LookingForBride == isLookingForBride && r.User.IsPublished == isPublished,
                    orderBy: source => source.OrderBy(r => r.User.BasicInfo.Name),
                    null,
                    isLookingForBride, startIndex, chunkSize, true));
                return appUsers.ToList();

            }

            appUsers = await Task.Run(() => _unitOfWork.AppUseRepository.Get(
                r => r.LookingForBride == isLookingForBride && r.User.IsPublished == isPublished,
                null, null, isLookingForBride, startIndex, chunkSize, true));
            return appUsers.ToList();

        }

        public async Task<AppUser> GetAppUser(Expression<Func<AppUser, bool>> filter, bool hasInclude = false, bool isLookingForBride = false)
        {

            AppUser appUser = new AppUser();
            IEnumerable<AppUser> appUsers = new List<AppUser>();


            if (hasInclude)
            {
                appUsers = await Task.Run(() => _unitOfWork.AppUseRepository.Get(
                    r => r.LookingForBride == isLookingForBride,
                    null,
                    include: s => s
                        .Include(r => r.User)
                        .ThenInclude(a => a.BasicInfo)
                        .Include(r => r.User)
                        .ThenInclude(b => b.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(r => r.User)
                        .ThenInclude(b => b.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(r => r.User)
                        .ThenInclude(c => c.FamilyInfo).ThenInclude(c => c.FamilyMembers),
                    isLookingForBride));

                if (appUsers.Any())
                {
                    appUser = appUsers.FirstOrDefault();
                }

            }
            else
            {
                appUsers = await Task.Run(() => _unitOfWork.AppUseRepository.Get(
                    r => r.LookingForBride == isLookingForBride, null, null, isLookingForBride));

                if (appUsers.Any())
                {
                    appUser = appUsers.FirstOrDefault();
                }
            }

            return appUser;
        }

        public async Task<List<AppUser>> GetRecentAppUsers(Expression<Func<AppUser, bool>> filter, bool hasOrderBy = false, bool hasInclude = false, bool isLookingForBride = false)
        {
            IEnumerable<AppUser> appUsers = new List<AppUser>();
            appUsers = await Task.Run(() => _unitOfWork.AppUseRepository.GetRecent(
                 r => r.LookingForBride == isLookingForBride && r.User.IsPublished,
                 IncludeProperties.UserIncludingAllProperties, isLookingForBride));
            return appUsers.ToList();
        }

        public async Task InsertAppUser(AppUser appUser)
        {
            var result = await _mediator.Send(new AddAppUserRequest
            {
                AppUserToAdd = appUser
            });

            if (result == "Done")
            {
                await _mediator.Publish(new ComitDatabaseNotification());
            }
        }

        public async Task InsertAppUsers(List<AppUser> appUsers)
        {
            var result = await _mediator.Send(new AddAppUsersRequest
            {
                 AppUsersToAdd = appUsers
            });

            if (result == "Done")
            {
                await _mediator.Publish(new ComitDatabaseNotification());
            }
        }

        public async Task UpdateAppUser(AppUser appUser)
        {
            var result = await _mediator.Send(new UpdateAppUserRequest
            {
                AppUserTobeUpdated =  appUser
            });

            if (result == "Done")
            {
                await _mediator.Publish(new ComitDatabaseNotification());
            }
        }

        public async Task DeleteAppUser(AppUser appUser)
        {
            var result = await _mediator.Send(new DeleteAppUserRequest
            {
                AppUserTobeDeleted =  appUser
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
