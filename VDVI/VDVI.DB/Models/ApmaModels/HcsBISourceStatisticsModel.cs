using System;
using System.Collections.Generic;
using System.Text;

namespace VDVI.DB.Models.ApmaModels
{
    public class HcsBISourceStatisticsModel
    {
        public class HcsBISourceStatisticsResult
        {
            public HcsBISourceStatistics hcsBISourceStatistics;
        }
        public class HcsBISourceStatistics
        {
            public string PropertyCode { get; set; }
            public List<SourceStatistic> SourceStatistics { get; set; }
            public string UniqueID { get; set; }
            public bool Success { get; set; }
            public object ErrorInfo { get; set; }
            public object WarningInfo { get; set; }
        }

        public class SourceStatistic
        {
            public DateTime BusinessDate { get; set; }
            public string SourceCode { get; set; }
            public int NumberOfRooms { get; set; }
            public double TotalRevenue { get; set; }
            public double TotalRevenueExcl { get; set; }
            public double RevenueStatCodeA { get; set; }
            public double RevenueStatCodeAExcl { get; set; }
            public double RevenueStatCodeB { get; set; }
            public double RevenueStatCodeBExcl { get; set; }
            public double RevenueStatCodeC { get; set; }
            public double RevenueStatCodeCExcl { get; set; }
            public double RevenueStatCodeD { get; set; }
            public double RevenueStatCodeDExcl { get; set; }
            public double RevenueStatCodeE { get; set; }
            public double RevenueStatCodeEExcl { get; set; }
            public double RevenueStatCodeF { get; set; }
            public double RevenueStatCodeFExcl { get; set; }
            public double RevenueStatCodeUndefined { get; set; }
            public double RevenueStatCodeUndefinedExcl { get; set; }
        }

    }
}
