using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkinAppBackend.Services
{
    public class DateSwapper
    {
        public DateTime SwapDates(DateTime date)
        {
            string[] dateSplit = date.ToShortDateString().Split("-");
            string temp = dateSplit[0];
            dateSplit[0] = dateSplit[2];
            dateSplit[2] = temp;

            
            string dateStr = dateSplit[0] + "-" + dateSplit[1] + "-" + dateSplit[2];
            date = Convert.ToDateTime(dateStr);

            return date;
        }
    }
}
