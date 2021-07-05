using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ghotok.Data.DataModels;
using GhotokApi.Common;

namespace GhotokApi.Utils.FilterBuilder
{
    public interface IFilterBuilder
    {

        IEnumerable<Expression<Func<AppUser, bool>>> GetAppUserFilter(IEnumerable<IDictionary<string, string>> filters);
        IEnumerable<KeyValuePair<AppUserFilter, string>> ConstructAppUserFilterCriteria(
            IEnumerable<IDictionary<string, string>> filters);
        IEnumerable<Expression<Func<User, bool>>> GetUserFilter(IEnumerable<IDictionary<string, string>> filters);
        IEnumerable<KeyValuePair<UserFilter, string>> ConstructUserFilterCriteria(
            IEnumerable<IDictionary<string, string>> filters);
    }
}