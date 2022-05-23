using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQuery.DbOperation
{
    public abstract  class SqlView: CommonOperations
    {
        protected string CreateViewFromSqlString(string sqlQueryString)
        {
            return RemoveSpecialCharacters(sqlQueryString);
        }   


        private static string CreateView(string viewename,string sqlstring)
        {
            return $"CREATE View {viewename} as {sqlstring} GO";
        }
    }
}
