using System.ComponentModel.DataAnnotations.Schema;
using VDVI.Repository.Models;

namespace VDVI.Repository.DB
{
    [Table("[hce].[MealPlans]")]
    public class DbMealPlans : Audit
    {
        public string PropertyCode { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
