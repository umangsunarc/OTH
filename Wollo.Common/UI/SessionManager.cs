using Wollo.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Wollo.Common.UI
{
    public static class SessionManager
    {
        const string UserSessionKey = "User";

        public static UserInformation UserInformation
        {
            get
            {
                return GetSessionValue<UserInformation>(UserSessionKey, true);
            }
            set
            {
                SetSessionValue<UserInformation>(UserSessionKey, value);
            }
        }


        private static T GetSessionValue<T>(string key, bool throwErrorIfNotPresent = false)
        {
            T value = default(T);

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[key] != null)
                {
                    try
                    {
                        value = (T)HttpContext.Current.Session[key];
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine(error.Message);
                        throw;
                    }
                }
                else if (throwErrorIfNotPresent)
                {
                    throw new Exception(string.Format("Invalid session for key '{0}'", key));
                }
            }

            return value;
        }

        private static void SetSessionValue<T>(string key, T value)
        {

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session[key] = value;
            }
        }

    }
}
