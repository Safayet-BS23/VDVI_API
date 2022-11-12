using Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator.Filters;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VDVI.Repository.DbContext.ApmaDbContext; 
using VDVI.ApmaRepository.Interfaces;
using VDVI.Repository.DB;
using VDVI.DB.Dtos;

namespace VDVI.Repository.ApmaRepository.Implementation
{
    public class HcsBISourceStatisticsHistoryRepository : DapperRepository<SourceStatisticHistoryDto>, IHcsBISourceStatisticsHistoryRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbSourceStatisticHistory> _tblRepository;

        public HcsBISourceStatisticsHistoryRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.SourceStatistic;
        }
        public async Task<string> BulkInsertWithProcAsync(IEnumerable<SourceStatisticHistoryDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_SourceStatistics_History", new { SourceStatistics_History_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<SourceStatisticHistoryDto>> BulkInsertAsync(IEnumerable<SourceStatisticHistoryDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbSourceStatisticHistory>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

        public async Task<bool> DeleteByBusinessDateAsync(DateTime businessDate) => await _tblRepository.DeleteAsync(x => x.BusinessDate == businessDate);

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<SourceStatisticHistoryDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<SourceStatisticHistoryDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<SourceStatisticHistoryDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbSourceStatisticHistory> dbEntities = await _dbContext
                .SourceStatistic
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<SourceStatisticHistoryDto>>(dbEntities);

            return entities;
        }

        public async Task<SourceStatisticHistoryDto> InsertAsync(SourceStatisticHistoryDto dto)
        {
            var dbEntity = TinyMapper.Map<DbSourceStatisticHistory>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<SourceStatisticHistoryDto>(dbEntity);
        }

        public async Task<SourceStatisticHistoryDto> UpdateAsync(SourceStatisticHistoryDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbSourceStatisticHistory>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
