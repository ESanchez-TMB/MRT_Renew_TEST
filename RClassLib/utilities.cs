using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace RClassLibrary
{
    public class utilities
    {
        public static bool IsValidUSZip2(string num)
        {
            if (num == null) return false;
            // Allows 5 digit, 5+4 digit and 9 digit zip codes
            string pattern = @"^(\d{5}-\d{4}|\d{5}|\d{9})$";
            Regex match = new Regex(pattern);
            return match.IsMatch(num);
        }
        //
        public static string getLogInfo()
        {
            StackFrame frame = new StackFrame(1);
            var method = frame.GetMethod();
            var type = method.DeclaringType;
            var name = method.Name;
            return type + "." + name;
        }
        //
        public static string logFileInfo()
        {
            StackTrace st = new StackTrace(new StackFrame(true));
            var file = st.GetFrame(0).GetFileName();
            var lineNumber = st.GetFrame(0).GetFileLineNumber().ToString();
            return "FileName: " + file + ", Line #: " + lineNumber;
        }

        public static string genTraceNum(string application, int OrderID)            
        {
            string month = "";
            string year = "";
            int mm = DateTime.Now.Month;
            int yy = DateTime.Now.Year;
            if (mm < 10)
                month = "0" + mm.ToString();
            else
                month = mm.ToString();
            year = (yy.ToString()).Substring(2,2);              
            return application + year +  
                (int.Parse("100000000") + OrderID).ToString().Substring(1, 8);               
        }
       




    }
}
