using System.ComponentModel.DataAnnotations.Schema;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[BanquetingRooms]")]
    public class DbBanquetingRooms :Audit
    {      
        public string PropertyCode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string BanquetingRoomType { get; set; }
        public bool IsCombination { get; set; }
        public string Combination { get; set; }
        public int ListOrder { get; set; }
    }
}
