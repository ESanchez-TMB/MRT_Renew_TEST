using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mrtRenew
{
    public class locks
    {
        public static void add(Dictionary<string, string> dictionary, string key, string value)
        {
            lock (dictionary)
            {
                dictionary.Add(key, value);
            }
        }
        public static void remove(Dictionary<string, string> dictionary, string key)
        {
            lock (dictionary)
            {
                dictionary.Remove(key);
            }
        }
    }
}