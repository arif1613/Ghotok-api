using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;

namespace GhotokApi.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsers(UserInfosRequestModel model, bool hasOrderBy=false,bool hasInclude=false,
           int startIndex = 0, int chunkSize = 0);
        Task<User> GetUser(UserInfoRequestModel model, bool hasInclude = false);
        Task<List<User>> GetRecentUsers(UserInfosRequestModel model);
        Task InsertUser(User user);
        Task InsertUsers(List<User> users);
        Task UpdateUser(User user);
        Task DeleteUser(User user);
        Task SaveDatabse();
    }
}