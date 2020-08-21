using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public static class Logger
    {
        private static TelemetryClient telemetryClient;

        public static void Initialize()
        {
            var config = TelemetryConfiguration.CreateDefault();
            config.InstrumentationKey = "4db899c8-043a-4a36-b28a-169e6024e0a7";
            telemetryClient = new TelemetryClient(config);
            telemetryClient.Context.Cloud.RoleInstance = Environment.MachineName;
            InstanceName = Environment.MachineName;
        }

        public static string InstanceName;

        public static void TraceInformation(string s)
        {
            telemetryClient.TrackTrace(s);
        }

        public static void Flush()
        {
            telemetryClient.Flush();
        }
    }
}
