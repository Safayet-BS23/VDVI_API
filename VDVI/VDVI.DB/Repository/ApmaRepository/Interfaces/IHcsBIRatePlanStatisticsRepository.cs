﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Repository.Dtos.Accounts;

namespace VDVI.Repository.Interfaces
{
    public interface IHcsBIRatePlanStatisticsRepository
    {
        Task<RatePlanStatisticHistoryDto> InsertAsync(RatePlanStatisticHistoryDto dto);
        Task<IEnumerable<RatePlanStatisticHistoryDto>> BulkInsertAsync(IEnumerable<RatePlanStatisticHistoryDto> dto);
        Task<string> BulkInsertWithProcAsync(IEnumerable<RatePlanStatisticHistoryDto> dto);
        Task<RatePlanStatisticHistoryDto> UpdateAsync(RatePlanStatisticHistoryDto dto);
        Task<IEnumerable<RatePlanStatisticHistoryDto>> GetAllByPropertyCodeAsync(string propertyCode);
        Task<RatePlanStatisticHistoryDto> FindByIdAsync(int id);
        Task<bool> DeleteByPropertyCodeAsync(string propertyCode);
        Task<bool> DeleteByBusinessDateAsync(DateTime businessDate);
    }
}
