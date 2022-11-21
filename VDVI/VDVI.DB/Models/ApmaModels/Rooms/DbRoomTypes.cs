using System.ComponentModel.DataAnnotations.Schema;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[RoomTypes]")]
    public class DbRoomTypes : Audit
    { 
        public string PropertyCode { get; set; }
        public string RoomType { get; set; }
        public string Description { get; set; }
        public string InfoText { get; set; }
        public int ListOrder { get; set; }
        public int Inventory { get; set; }
        public int BedsInRoom { get; set; }
        public int MaxOccupancy { get; set; }
        public int Adults { get; set; }
        public int ChildOld { get; set; }
        public int ChildYoung { get; set; }
        public int Infants { get; set; }
        public int Baby { get; set; }
        public string RoomTypeGroup { get; set; }
        public string UpsellRoomGroup { get; set; }
        public bool Publish { get; set; }
        public string Url { get; set; }
    }
}


