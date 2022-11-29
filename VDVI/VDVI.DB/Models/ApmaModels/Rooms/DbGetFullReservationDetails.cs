using System.ComponentModel.DataAnnotations.Schema;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[GetFullReservationDetails]")]
    public class DbGetFullReservationDetails : Audit
    {
        public string PropertyCode { get; set; } 
        public string PMSReservationNumber { get; set; }
        public string PMSSegmentNumber { get; set; }
        public string ExternalReservationNumber { get; set; }
        public string Language { get; set; }
        public string UniqueID { get; set; }  
    }
}
