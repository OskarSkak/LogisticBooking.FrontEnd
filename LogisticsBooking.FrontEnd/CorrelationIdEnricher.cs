
using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;
using Serilog;

namespace LogisticsBooking.FrontEnd
{
    public class CorrelationIdEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("CorrelationId" , Trace.CorrelationManager.ActivityId));
        }
    }
}