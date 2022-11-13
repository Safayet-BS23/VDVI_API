using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class GetFullReservationDetailsDto : Audit
    {
        public string PropertyCode { get; set; } 
        public string PMSReservationNumber { get; set; }
        public string PMSSegmentNumber { get; set; }
        public string ExternalReservationNumber { get; set; }
        public string Language { get; set; }
        public string UniqueID { get; set; }  
    }
}
