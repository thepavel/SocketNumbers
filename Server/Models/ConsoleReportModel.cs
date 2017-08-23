using System;
namespace Server.Models
{
    public class ConsoleReportModel
    {

        public int IncrementalDuplicateCount { get; set; }
        public int TotalDuplicateCount { get; set; }
        public int IncrementalUniqueCount { get; set; }
        public int TotalUniqueCount { get; set;}


        public ConsoleReportModel()
        {
            IncrementalDuplicateCount = 0;
            TotalDuplicateCount = 0;
            IncrementalUniqueCount = 0;
            TotalUniqueCount = 0;
        }

        public ConsoleReportModel(ConsoleReportModel model)
        {
            IncrementalUniqueCount = model.IncrementalUniqueCount;
            IncrementalDuplicateCount = model.IncrementalDuplicateCount;
            TotalDuplicateCount = model.TotalDuplicateCount;
            TotalUniqueCount = model.TotalUniqueCount;
        }
    }
}
