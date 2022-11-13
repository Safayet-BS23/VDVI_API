using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[SourceStatistics_Future]")]
    public class DbSourceStatisticFuture :Audit
    {
        public string PropertyCode { get; set; }
        public DateTime? BusinessDate { get; set; }
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
