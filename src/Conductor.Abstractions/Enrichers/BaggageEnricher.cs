﻿using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Conductor.Abstractions.Enrichers
{
    public class BaggageEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (Activity.Current == null)
            {
                return;
            }

            foreach ((string key, string value) in Activity.Current.Baggage)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(key, value));
            }
        }
    }
}
