using VDVI.Repository.Models;

namespace VDVI.DB.Dtos
{
    public class PackagesDto : Audit
    {
        public string PropertyCode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }  
    }
}
