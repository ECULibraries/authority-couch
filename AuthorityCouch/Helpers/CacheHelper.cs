using System;
using System.Web;

namespace AuthorityCouch.Helpers
{
    public static class CacheHelper
    {
        public static void Add<T>(T o, string key, DateTime expiration)
        {
            HttpContext.Current.Cache.Insert(key, o, null, expiration, System.Web.Caching.Cache.NoSlidingExpiration);
        }

        public static void Add<T>(T o, string key, TimeSpan span)
        {
            HttpContext.Current.Cache.Insert(key, o, null, System.Web.Caching.Cache.NoAbsoluteExpiration, span);
        }

        public static void Clear(string key)
        {
            HttpContext.Current.Cache.Remove(key);
        }

        public static bool Exists(string key)
        {
            return HttpContext.Current.Cache[key] != null;
        }

        public static bool Get<T>(string key, out T value)
        {
            try
            {
                if (!Exists(key))
                {
                    value = default(T);
                    return false;
                }

                value = (T)HttpContext.Current.Cache[key];
            }
            catch
            {
                value = default(T);
                return false;
            }

            return true;
        }
    }
}