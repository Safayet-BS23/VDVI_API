using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[DailyHistory_Future]")]
    public class DailyHistoryFuture : Audit
    {
        public string PropertyCode { get; set; }
        public DateTime Date { get; set; }
        public string PmsSegmentNumber { get; set; }
        public string PmsSegmentType { get; set; }
        public string RoomType { get; set; }
        public string Source { get; set; }
        public string SubSource { get; set; }
        public string RateType { get; set; }
        public string Mealplan { get; set; }
        public string Package { get; set; }
        public string CountryIso2Code { get; set; }
        public decimal PaymentDebitor { get; set; }
        public decimal PaymentNonDebitor { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public int Infants { get; set; }
        public bool IsDayuse { get; set; }
        public decimal RevenueInclusiveA { get; set; }
        public decimal RevenueInclusiveB { get; set; }
        public decimal RevenueInclusiveC { get; set; }
        public decimal RevenueInclusiveD { get; set; }
        public decimal RevenueInclusiveE { get; set; }
        public decimal RevenueInclusiveF { get; set; }
        public decimal RevenueInclusiveUndefined { get; set; }
        public decimal RevenueExclusiveA { get; set; }
        public decimal RevenueExclusiveB { get; set; }
        public decimal RevenueExclusiveC { get; set; }
        public decimal RevenueExclusiveD { get; set; }
        public decimal RevenueExclusiveE { get; set; }
        public decimal RevenueExclusiveF { get; set; }
        public decimal RevenueExclusiveUndefined { get; set; }
    }
}
