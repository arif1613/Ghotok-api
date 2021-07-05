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
using GhotokApi.Models.RequestModels;
using GhotokApi.Utils.FilterBuilder;
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
        private readonly IFilterBuilder _filterBuilder;


        public AppUserService(IUnitOfWork unitOfWork, IMediator mediator, IFilterBuilder filterBuilder)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _filterBuilder = filterBuilder;
        }


        public async Task<List<AppUser>> GetAppUsers(AppUserInfosRequestModel model)
        {
            var result = await _mediator.Send(new GetAppUsersRequest
            {
                model = model
            });
            return result;

        }

        public async Task<AppUser> GetAppUser(Expression<Func<AppUser, bool>> filter=null, bool hasInclude = false, bool isLookingForBride = false)
        {

            AppUser appUser = null;
            IEnumerable<AppUser> appUsers = new List<AppUser>();

            if (hasInclude)
            {
                appUsers = await Task.Run(() => _unitOfWork.AppUseRepository.Get(
                    new List<Expression<Func<AppUser, bool>>>
                    {
                       filter
                    },
                    
                    include: s => s
                        .Include(r => r.User)
                        .ThenInclude(a => a.BasicInfo)
                        .Include(r => r.User)
                        .ThenInclude(b => b.EducationInfo).ThenInclude(b => b.Educations)
                        .Include(r => r.User)
                        .ThenInclude(b => b.EducationInfo).ThenInclude(b => b.CurrentJob)
                        .Include(r => r.User)
                        .ThenInclude(c => c.FamilyInfo).ThenInclude(c => c.FamilyMembers)));

                if (appUsers.Any())
                {
                    appUser = appUsers.FirstOrDefault();
                }

            }
            else
            {
                appUsers = await Task.Run(() => _unitOfWork.AppUseRepository.Get(
                    new List<Expression<Func<AppUser, bool>>>
                    {
                        filter
                    },  null, null));

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
                new List<Expression<Func<AppUser, bool>>>
                {
                    filter
                },
                 IncludeProperties.UserIncludingAllProperties));
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
