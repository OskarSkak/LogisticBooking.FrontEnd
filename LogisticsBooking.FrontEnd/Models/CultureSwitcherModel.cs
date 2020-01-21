using System.Collections.Generic;
using System.Globalization;

namespace LogisticsBooking.FrontEnd.Models
{
    public class CultureSwitcherModel
    {
        public CultureInfo CurrentUICulture { get; set; }
        public List<CultureInfo> SupportedCultures { get; set; }
    }
}