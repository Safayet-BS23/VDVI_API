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
    public class HcsDailyHistoryRepository : DapperRepository<DbDailyHistory>, IHcsDailyHistoryRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbDailyHistory> _tblRepository;

        public HcsDailyHistoryRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.DailyHistory;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<DailyHistoryDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_DailyHistory_History", new { DailyHistory_History_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<DailyHistoryDto>> BulkInsertAsync(IEnumerable<DailyHistoryDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbDailyHistory>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

        public async Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate) => await _tblRepository.DeleteAsync(x => x.Date == DashboardDate);

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<DailyHistoryDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<DailyHistoryDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<DailyHistoryDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbDailyHistory> dbEntities = await _dbContext
                .DailyHistory
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<DailyHistoryDto>>(dbEntities);

            return entities;
        }

        public async Task<DailyHistoryDto> InsertAsync(DailyHistoryDto dto)
        {
            var dbEntity = TinyMapper.Map<DbDailyHistory>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<DailyHistoryDto>(dbEntity);
        }

        public async Task<DailyHistoryDto> UpdateAsync(DailyHistoryDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbDailyHistory>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
