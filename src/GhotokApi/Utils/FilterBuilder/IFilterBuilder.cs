using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ghotok.Data.DataModels;
using GhotokApi.Common;

namespace GhotokApi.Utils.FilterBuilder
{
    public interface IFilterBuilder
    {
        IEnumerable<Expression<Func<User, bool>>> GetUserFilter(IEnumerable<KeyValuePair<string, string>> filters);
    }
}