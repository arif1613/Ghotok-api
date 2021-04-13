using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ghotok.Data.DataModels;
using GhotokApi.Common;

namespace GhotokApi.Utils.FilterBuilder
{
    public class FilterBuilder : IFilterBuilder
    {
        public IEnumerable<Expression<Func<User, bool>>> GetUserFilter(IEnumerable<KeyValuePair<string, string>> filters)
        {
            
            var predicates = filters.Select<KeyValuePair<string, string>, Expression<Func<User, bool>>>(s =>
                s.Key switch
                {

                    "IslookingForBride" => (ap) =>ap.LookingForBride.ToString()==s.Value,
                    "Email" => (ap) => ap.Email == s.Value,
                    _ => (ap) => true
                });

            return predicates;
        }
    }
}

