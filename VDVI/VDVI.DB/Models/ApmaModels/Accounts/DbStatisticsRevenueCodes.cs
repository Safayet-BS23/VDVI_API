using System.ComponentModel.DataAnnotations.Schema;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[StatisticsRevenueCodes] ")]
    public class DbStatisticsRevenueCodes : Audit
    {
        public string PropertyCode { get; set; }
        public string  Type { get; set; }
        public string Description { get; set; }
    }
}
