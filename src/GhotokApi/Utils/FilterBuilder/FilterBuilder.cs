using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ghotok.Data.DataModels;
using GhotokApi.Common;

namespace GhotokApi.Utils.FilterBuilder
{
    public class FilterBuilder : IFilterBuilder
    {
        public IEnumerable<Expression<Func<AppUser, bool>>> GetAppUserFilter(IEnumerable<IDictionary<string, string>> filters)
        {
            var constructedfilters = ConstructAppUserFilterCriteria(filters);
            var predicates = constructedfilters.Select<KeyValuePair<AppUserFilter, string>, Expression<Func<AppUser, bool>>>(s =>
                s.Key switch
                {
                    AppUserFilter.UserRole => (ap) => ap.UserRole == s.Value,
                    AppUserFilter.IsLoggedIn => (ap) => ap.IsLoggedin == Convert.ToBoolean(s.Value),
                    AppUserFilter.IsVerified => (ap) => ap.IsVarified == Convert.ToBoolean(s.Value),
                    AppUserFilter.IslookingForBride => (ap) => ap.LookingForBride == Convert.ToBoolean(s.Value),
                    _ => (ap) => true
                });

            return predicates;
        }

        public IEnumerable<KeyValuePair<AppUserFilter, string>> ConstructAppUserFilterCriteria(IEnumerable<IDictionary<string, string>> filters)
        {
            var keyValuePairs = filters?.SelectMany(f => f.Select(kvp => kvp)) ?? Enumerable.Empty<KeyValuePair<string, string>>();
            var filterCriteria = keyValuePairs.Select(kvp => new KeyValuePair<AppUserFilter, string>(Enum.Parse<AppUserFilter>(kvp.Key, true), kvp.Value));

            return filterCriteria.ToList();
        }

        public IEnumerable<Expression<Func<User, bool>>> GetUserFilter(IEnumerable<IDictionary<string, string>> filters)
        {
            var constructedfilters = ConstructUserFilterCriteria(filters);
            var predicates = constructedfilters.Select<KeyValuePair<UserFilter, string>, Expression<Func<User, bool>>>(s =>
                s.Key switch
                {
                    UserFilter.IslookingForBride => (ap) =>ap.LookingForBride== Convert.ToBoolean(s.Value),
                    UserFilter.IsPictureUploaded => (ap) => ap.IsPictureUploaded == Convert.ToBoolean(s.Value),
                    UserFilter.RegisterByMobileNumber => (ap) => ap.RegisterByMobileNumber == Convert.ToBoolean(s.Value),
                    UserFilter.Email => (ap) => ap.Email == s.Value,
                    UserFilter.IsPublished => (ap) => ap.IsPublished ==Convert.ToBoolean(s.Value),
                    UserFilter.MobileNumber => (ap) => ap.MobileNumber == s.Value,
                    _ => (ap) => true
                });

            return predicates;
        }


        public IEnumerable<KeyValuePair<UserFilter, string>> ConstructUserFilterCriteria(IEnumerable<IDictionary<string, string>> filters)
        {
            var keyValuePairs = filters?.SelectMany(f => f.Select(kvp => kvp)) ?? Enumerable.Empty<KeyValuePair<string, string>>();
            var filterCriteria = keyValuePairs.Select(kvp => new KeyValuePair<UserFilter, string>(Enum.Parse<UserFilter>(kvp.Key, true), kvp.Value));

            return filterCriteria.ToList();
        }


    }
}

