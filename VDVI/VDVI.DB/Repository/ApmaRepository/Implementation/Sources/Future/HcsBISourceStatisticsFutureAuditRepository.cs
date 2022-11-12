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
    public class HcsBISourceStatisticsFutureAuditRepository : DapperRepository<SourceStatisticsFutureAuditDto>, IHcsBISourceStatisticsFutureAuditRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbSourceStatisticFuture> _tblRepository;

        public HcsBISourceStatisticsFutureAuditRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.SourceStatisticFuture;
        }  
        public async Task<IEnumerable<SourceStatisticsFutureAuditDto>> BulkInsertAsync(IEnumerable<SourceStatisticsFutureAuditDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbSourceStatisticFuture>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

        public async Task<bool> DeleteByBusinessDateAsync(DateTime businessDate) => await _tblRepository.DeleteAsync(x => x.BusinessDate == businessDate);

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<SourceStatisticsFutureAuditDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<SourceStatisticsFutureAuditDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<SourceStatisticsFutureAuditDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbSourceStatisticFuture> dbEntities = await _dbContext
                .SourceStatisticFuture
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<SourceStatisticsFutureAuditDto>>(dbEntities);

            return entities;
        }

        public async Task<SourceStatisticsFutureAuditDto> InsertAsync(SourceStatisticsFutureAuditDto dto)
        {
            var dbEntity = TinyMapper.Map<DbSourceStatisticFuture>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<SourceStatisticsFutureAuditDto>(dbEntity);
        }

        public async Task<SourceStatisticsFutureAuditDto> UpdateAsync(SourceStatisticsFutureAuditDto dto)
        {
            var dbEntity = TinyMapper.Map<DbSourceStatisticFuture>(dto);

            await _tblRepository.UpdateAsync(dbEntity);

            return dto;
        }
    }
}
