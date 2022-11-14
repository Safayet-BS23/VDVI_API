using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsListMealPlansRepository
    {
        Task<MealPlansDto> InsertAsync(MealPlansDto dto);
        Task<IEnumerable<MealPlansDto>> BulkInsertAsync(IEnumerable<MealPlansDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<MealPlansDto> dto);
        Task<MealPlansDto> UpdateAsync(MealPlansDto dto);
        Task<IEnumerable<MealPlansDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<MealPlansDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode); 
    }
}
