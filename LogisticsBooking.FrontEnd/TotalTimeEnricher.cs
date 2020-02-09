using Serilog.Core;
using Serilog.Events;

namespace LogisticsBooking.FrontEnd
{
    public class TotalTimeEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            
        }
    }
}