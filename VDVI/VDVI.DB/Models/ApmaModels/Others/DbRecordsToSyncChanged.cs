using System.ComponentModel.DataAnnotations.Schema;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[RecordsToSyncChanged]")]
    public class DbRecordsToSyncChanged : Audit
    {
        public string PropertyCode { get; set; }

        public string Type { get; set; }

        public string Reference { get; set; }

        public string SubReference { get; set; }

        public string NewReference { get; set; }

        public string Status { get; set; }
}
}
