using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQuery.DbOperation
{
    public abstract class SqlStoreProcedure : CommonOperations
    {
        internal const string CreateProcedureHeader = "CREATE OR ALTER procedure ";
        protected static string CreateStoreProcedureFromSqlString(string procedureName,string sqlQueryString)
        {
            sqlQueryString = RemoveSpecialCharacters(sqlQueryString);
            sqlQueryString = CreateStoreProcedureBody(procedureName, sqlQueryString);
            //sqlQueryString = AddHeader(sqlQueryString);
            return AddFooter(sqlQueryString);
        }
        private static string CreateStoreProcedureBody(string procedurename, string sqlstring)
        {
            return CreateProcedureHeader+procedurename+ " as "+ sqlstring;
        }
    }
}
