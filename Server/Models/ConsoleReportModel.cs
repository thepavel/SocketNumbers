using System;
namespace Server.Models
{
    public class ConsoleReportModel
    {
        public int TotalUniqueEntries { get; set; }
        public int TotalDuplicateEntries { get; set; }

        public ConsoleReportModel()
        {
        }
    }
}
