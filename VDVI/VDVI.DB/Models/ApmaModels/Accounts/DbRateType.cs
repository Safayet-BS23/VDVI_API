using System;
using System.ComponentModel.DataAnnotations.Schema;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[RateType]")]
    public class DbRateType : Audit
    {
        public string PropertyCode { get; set; } 
        public string Code { get; set; } 
        public string Description { get; set; } 
        public string ChargeType { get; set; } 
        public string ChargeplanCode { get; set; } 
        public string Article { get; set; } 
        public Boolean IsVisible { get; set; } 
        public Boolean IncludesBreakfast1 { get; set; } 
        public Boolean IncludesBreakfast2 { get; set; } 
        public Boolean IncludesPackedLunch1 { get; set; } 
        public Boolean IncludesLunch1 { get; set; } 
        public Boolean IncludesLunch2 { get; set; } 
        public Boolean IncludesDinner1 { get; set; } 
        public Boolean IncludesDinner2 { get; set; } 
        public Boolean IsManualRate { get; set; } 
        public Boolean IsCrsManaged { get; set; } 
        public Boolean IsComplimentary { get; set; } 
        public Boolean IgnoreRoomsToSell { get; set; } 
        public int ListOrder { get; set; } 
        public int NotInRateQuery { get; set; } 
        public string CrsConnector { get; set; } 
        public string DistributionMealplan { get; set; } 
        public string SelectableInGroupBlocks { get; set; }
        public Boolean IsPrepaid { get; set; }
        public string DefaultSource { get; set; }
        public string DefaultSubSource { get; set; }
        public string DerivedFrom { get; set; }
        public string DerivedType { get; set; }
        public Decimal DerivedValue { get; set; }
        public string RateRoundingType { get; set; }
        public string RateTypeGroup { get; set; }
        public Boolean IsPublished { get; set; }
        public Boolean IsCwiPackage { get; set; }
        public string Allotment { get; set; }
        public string RatePlanCategory { get; set; }  

    }
}
