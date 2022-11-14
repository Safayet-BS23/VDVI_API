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
    public class HcsDailyHistoryFutureAuditRepository : DapperRepository<DbDailyHistory>, IHcsDailyFutureAuditRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbDailyHistory> _tblRepository;

        public HcsDailyHistoryFutureAuditRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.DailyHistory;
        }

        public async Task<IEnumerable<DailyHistoryFutureAuditDto>> BulkInsertAsync(IEnumerable<DailyHistoryFutureAuditDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbDailyHistory>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

        public async Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate) => await _tblRepository.DeleteAsync(x => x.Date == DashboardDate);

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<DailyHistoryFutureAuditDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<DailyHistoryFutureAuditDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<DailyHistoryFutureAuditDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbDailyHistory> dbEntities = await _dbContext
                .DailyHistory
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<DailyHistoryFutureAuditDto>>(dbEntities);

            return entities;
        }

        public async Task<DailyHistoryFutureAuditDto> InsertAsync(DailyHistoryFutureAuditDto dto)
        {
            var dbEntity = TinyMapper.Map<DbDailyHistory>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<DailyHistoryFutureAuditDto>(dbEntity);
        }

        public async Task<DailyHistoryFutureAuditDto> UpdateAsync(DailyHistoryFutureAuditDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbDailyHistory>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
