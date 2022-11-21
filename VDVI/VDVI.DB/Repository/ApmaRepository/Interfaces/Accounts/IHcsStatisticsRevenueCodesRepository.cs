using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;

namespace VDVI.ApmaRepository.Interfaces
{
    public interface IHcsStatisticsRevenueCodesRepository
    {
        Task<StatisticsRevenueCodesDto> InsertAsync(StatisticsRevenueCodesDto dto);
        Task<IEnumerable<StatisticsRevenueCodesDto>> BulkInsertAsync(IEnumerable<StatisticsRevenueCodesDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<StatisticsRevenueCodesDto> dto);
        Task<StatisticsRevenueCodesDto> UpdateAsync(StatisticsRevenueCodesDto dto);
        Task<IEnumerable<StatisticsRevenueCodesDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<StatisticsRevenueCodesDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
    }
}
