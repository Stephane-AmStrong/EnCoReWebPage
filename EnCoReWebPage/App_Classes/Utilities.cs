using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EnCoReWebPage.App_Classes
{
    public class Utilities
    {
        public static bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }

    public static class StringExtentions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (value.Length > maxLength) return value.Substring(0, maxLength) + " ...";
            return value;
        }

        public static bool Contains(this string source, string value, CompareOptions options, CultureInfo culture)
        {
            return culture.CompareInfo.IndexOf(source, value, options) >= 0;
        }

        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source?.IndexOf(value, comparisonType) >= 0;
        }
    }
}