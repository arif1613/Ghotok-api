using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;

namespace GhotokApi.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsers(Expression<Func<User, bool>> filter, bool isPublished, bool hasOrderBy=false,bool hasInclude=false,
           bool isLookingForBride = false, int startIndex = 0, int chunkSize = 0);
        Task<User> GetUser(Expression<Func<User, bool>> filter, bool hasInclude = false, bool isLookingForBride=false);
        Task<List<User>> GetRecentUsers(Expression<Func<User, bool>> filter, bool isLookingForBride=false);
        Task InsertUser(User User);
        Task InsertUsers(List<User> Users);
        Task UpdateUser(User User);
        Task DeleteUser(User User);
        Task SaveDatabse();
    }
}