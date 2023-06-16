using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.AnalyticsReporting.v4.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

namespace GA4
{
    class Program
    {
        static void Main(string[] args)
        {
            // Nastavte si cestu k souboru credentials.json, který jste stáhli při povolení API
            string credentialsPath = @"d:\Projekty\Aplikace\GA4\local-concord-383708-e147e26b4de2.json";

            var credential = GoogleCredential.FromFile(credentialsPath)
                .CreateScoped(new[] { AnalyticsReportingService.Scope.AnalyticsReadonly });
            var service = new AnalyticsReportingService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Analytics Reporting API v4"
            });

            var dateRange = new DateRange()
            {
                StartDate = "2023-06-01",
                EndDate = "2023-06-30"
            };

            var sessions = new Metric()
            {
                Expression = "ga:sessions",
                Alias = "Sessions"
            };

            var visitors = new Metric()
            {
                Expression = "ga:users",
                Alias = "Visitors"
            };

            var dimensions = new List<Dimension>()
            {
                new Dimension() { Name = "ga:date" }
            };

            var request = new ReportRequest()
            {
                ViewId = "2616231498",
                DateRanges = new List<DateRange>() { dateRange },
                Metrics = new List<Metric>() { sessions, visitors },
                Dimensions = dimensions
            };

            var getReportsRequest = new GetReportsRequest() { ReportRequests = new List<ReportRequest>() { request } };
            var response = service.Reports.BatchGet(getReportsRequest).Execute();

            int visitorsCount = 0;
            foreach (var row in response.Reports[0].Data.Rows)
            {
                visitorsCount += Convert.ToInt32(row.Metrics[0].Values[0]);
            }

            Console.WriteLine("Počet návštěvníků za měsíc: " + visitorsCount);
        }
    }
}
