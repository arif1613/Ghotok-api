using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQuery.DbOperation
{
    internal class SqlStoreProcedure: CommonOperations
    {
        internal static string CreateViewFromSqlString(string sqlQueryString)
        {
            return RemoveSpecialCharacters(sqlQueryString);
        }

        private static string CreateProcedure(string procedurename,string sqlstring)
        {
            return $"CREATE OR ALTER procedure {procedurename} as {sqlstring} GO";
        }
    }
}
