using System;
using System.Collections.Generic;
using System.Text;
using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class StatisticsRevenueCodesDto : Audit
    {
        public string PropertyCode { get; set; }
        public string  Type { get; set; }
        public string Description { get; set; }
    }
}
