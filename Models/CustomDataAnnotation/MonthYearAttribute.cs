using System.ComponentModel.DataAnnotations;

namespace StreamberryMoviesApi.Models.CustomDataAnnotation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class MonthYearAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || !(value is string))
            {
                return false;
            }

            string monthYearString = (string)value;
            return IsMonthYearFormatValid(monthYearString);
        }

        private bool IsMonthYearFormatValid(string monthYearString)
        {
            if (DateTime.TryParseExact(monthYearString, "MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _))
            {
                return true;
            }

            return false;
        }
    }
}
