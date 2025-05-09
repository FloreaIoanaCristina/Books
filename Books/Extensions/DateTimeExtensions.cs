using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var age = today.Year - dob.Year;
            if(dob> today.AddDays(-age)) age--;

            return age;
        }
    }
}