using System;
using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class RateTypesDto : Audit
    {
        public string PropertyCode { get; set; } 
        public string Code { get; set; } 
        public string Description { get; set; } 
        public string ChargeType { get; set; } 
        public string ChargeplanCode { get; set; } 
        public string Article { get; set; } 
        public bool IsVisible { get; set; } 
        public bool IncludesBreakfast1 { get; set; } 
        public bool IncludesBreakfast2 { get; set; } 
        public bool IncludesPackedLunch1 { get; set; } 
        public bool IncludesPackedLunch2 { get; set; } 
        public bool IncludesLunch1 { get; set; } 
        public bool IncludesLunch2 { get; set; } 
        public bool IncludesDinner1 { get; set; } 
        public bool IncludesDinner2 { get; set; } 
        public bool IsManualRate { get; set; } 
        public bool IsCrsManaged { get; set; } 
        public bool IsComplimentary { get; set; } 
        public bool IgnoreRoomsToSell { get; set; } 
        public int ListOrder { get; set; } 
        public bool NotInRateQuery { get; set; } 
        public string CrsConnector { get; set; } 
        public string DistributionMealplan { get; set; } 
        public string SelectableInGroupBlocks { get; set; }
        public bool IsPrepaid { get; set; }
        public string DefaultSource { get; set; }
        public string DefaultSubSource { get; set; }
        public string DerivedFrom { get; set; }
        public string DerivedType { get; set; }
        public Decimal DerivedValue { get; set; }
        public string RateRoundingType { get; set; }
        public string RateTypeGroup { get; set; }
        public bool IsPublished { get; set; }
        public bool IsCwiPackage { get; set; }
        public string Allotment { get; set; }
        public string RatePlanCategory { get; set; }  

    }
}
