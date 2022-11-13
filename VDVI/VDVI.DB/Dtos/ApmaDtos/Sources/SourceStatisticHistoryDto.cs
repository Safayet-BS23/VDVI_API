using System;
using System.Collections.Generic;
using System.Text;
using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class SourceStatisticHistoryDto :Audit
    {
        public string PropertyCode { get; set; }
        public DateTime? BusinessDate { get; set; }
        public string SourceCode { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalRevenueExcl { get; set; }
        public decimal RevenueStatCodeA { get; set; }
        public decimal RevenueStatCodeAExcl { get; set; }
        public decimal RevenueStatCodeB { get; set; }
        public decimal RevenueStatCodeBExcl { get; set; }
        public decimal RevenueStatCodeC { get; set; }
        public decimal RevenueStatCodeCExcl { get; set; }
        public decimal RevenueStatCodeD { get; set; }
        public decimal RevenueStatCodeDExcl { get; set; }
        public decimal RevenueStatCodeE { get; set; }
        public decimal RevenueStatCodeEExcl { get; set; }
        public decimal RevenueStatCodeF { get; set; }
        public decimal RevenueStatCodeFExcl { get; set; }
        public decimal RevenueStatCodeUndefined { get; set; }
        public decimal RevenueStatCodeUndefinedExcl { get; set; }
    }
}
