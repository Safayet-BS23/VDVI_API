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
    public class HcsDailyHistoryFutureRepository : DapperRepository<DbDailyHistory>, IHcsDailyHistoryFutureRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbDailyHistory> _tblRepository;

        public HcsDailyHistoryFutureRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.DailyHistory;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<DailyHistoryFutureDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_DailyHistory_Future", new { DailyHistory_Future_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<DailyHistoryFutureDto>> BulkInsertAsync(IEnumerable<DailyHistoryFutureDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbDailyHistory>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

        public async Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate) => await _tblRepository.DeleteAsync(x => x.Date == DashboardDate);

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<DailyHistoryFutureDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<DailyHistoryFutureDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<DailyHistoryFutureDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbDailyHistory> dbEntities = await _dbContext
                .DailyHistory
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<DailyHistoryFutureDto>>(dbEntities);

            return entities;
        }

        public async Task<DailyHistoryFutureDto> InsertAsync(DailyHistoryFutureDto dto)
        {
            var dbEntity = TinyMapper.Map<DbDailyHistory>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<DailyHistoryFutureDto>(dbEntity);
        }

        public async Task<DailyHistoryFutureDto> UpdateAsync(DailyHistoryFutureDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbDailyHistory>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
