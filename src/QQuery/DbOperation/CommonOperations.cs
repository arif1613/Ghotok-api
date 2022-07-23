using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QQuery.DbOperation
{
    public abstract class CommonOperations
    {
        public static string Header = "SET ANSI_NULLS ON GO SET QUOTED_IDENTIFIER ON GO ";
        public static string Footer = " GO";
        public static string GoString = " GO";
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

        public static string AddHeader(string sqlQueryString)
        {
            
            return Header + sqlQueryString;
        }

        public static string AddFooter(string sqlQueryString)
        {
            return sqlQueryString + Footer;
        }
    }
}
