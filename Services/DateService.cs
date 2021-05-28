using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_NetCore_API.Services
{
    public class DateService
    {

        /*public string GetCompleteDate(string DateString)
        {
           string[] ArrayString = DateString.Split(" ");
           int Month = GetMonth(ArrayString[1]);
           string Date = ArrayString[2];
           string Year = ArrayString[3];
           string CompleteDate = Month.ToString() + " /" + Date + " /" + Year;

           return CompleteDate;
        }*/

        public string GetCompleteFromDate(string FromDate)
        {
            string[] ArrayString = FromDate.Split(" ");
            int Month = GetMonth(ArrayString[1]);
            string Date = ArrayString[2];
            string Year = ArrayString[3];
            string CompleteDate = Month.ToString() + " /" + Date + " /" + Year;

            return CompleteDate+" 00:00";

        }

        public string GetCompleteEndDate(string EndDate)
        {
            string[] ArrayString = EndDate.Split(" ");
            int Month = GetMonth(ArrayString[1]);
            string Date = ArrayString[2];
            string Year = ArrayString[3];
            string CompleteDate = Month.ToString() + " /" + Date + " /" + Year;

            return CompleteDate + " 23:59";

        }

        public int GetMonth(string Month)
        {
            switch(Month.ToUpper())
            {
                case "JAN":
                    return 1;
                case "FEB":
                    return 2;
                case "MAR":
                    return 3;
                case "APR":
                    return 4;
                case "MAY":
                    return 5;
                case "JUN":
                    return 6;
                case "JUL":
                    return 7;
                case "AUG":
                    return 8;
                case "SEP":
                    return 9;
                case "OCT":
                    return 10;
                case "NOV":
                    return 11;
                case "DEC":
                    return 12;
                default:
                    return 0;
            }
           
        }


        
    }
}
