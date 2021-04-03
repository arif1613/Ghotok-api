using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;

namespace GhotokApi.Services
{
    public interface IAppUserService
    {
        Task<List<AppUser>> GetAppUsers(Expression<Func<AppUser, bool>> filter, bool isPublished, bool hasOrderBy=false,bool hasInclude=false,
           bool isLookingForBride = false, int startIndex = 0, int chunkSize = 0);
        Task<AppUser> GetAppUser(Expression<Func<AppUser, bool>> filter, bool hasInclude = false, bool isLookingForBride=false);
        Task<List<AppUser>> GetRecentAppUsers(Expression<Func<AppUser, bool>> filter, bool hasOrderBy = false, bool hasInclude = false, bool isLookingForBride=false);
        Task InsertAppUser(AppUser appUser);
        Task InsertAppUsers(List<AppUser> appUsers);
        Task UpdateAppUser(AppUser appUser);
        Task DeleteAppUser(AppUser appUser);
        Task SaveDatabse();

    }
}