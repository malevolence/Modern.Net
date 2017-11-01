using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modern.Utils
{
    public static class StringUtils
    {
        public static string Base64Encode(string s)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(s);
            return System.Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(string s)
        {
            var bytes = UrlBase64.Decode(s);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}