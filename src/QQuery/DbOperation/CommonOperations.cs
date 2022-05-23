using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQuery.DbOperation
{
    public abstract class CommonOperations
    {
        public static string RemoveSpecialCharacters(string sqlQueryString)
        {
            if (sqlQueryString.Contains("\n"))
            {
                sqlQueryString = sqlQueryString.Replace("\n", " ");
            }

            if (sqlQueryString.Contains("\r"))
            {
                sqlQueryString = sqlQueryString.Replace("\r", " ");
            }

            if (sqlQueryString.Contains("  "))
            {
                sqlQueryString = sqlQueryString.Replace("  ", " ");
            }

            if (sqlQueryString.Contains("This LINQ query"))
            {
                sqlQueryString = sqlQueryString.Split("This LINQ query").FirstOrDefault();
            }
            return sqlQueryString;
        }
    }
}
