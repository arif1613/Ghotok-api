using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Models.RequestModels;

namespace GhotokApi.Services
{
    public interface IUserService
    {
        Task<IQueryable<User>> GetUsers(UserInfosRequestModel model);
        Task<User> GetUser(UserInfoRequestModel model);
        Task<List<User>> GetRecentUsers(UserInfosRequestModel model);
        Task InsertUser(User user);
        Task InsertUsers(List<User> users);
        Task UpdateUser(User user);
        Task DeleteUser(User user);
        Task SaveDatabse();
    }
}