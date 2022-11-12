using Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator.Filters;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.ApmaRepository.Interfaces;
using VDVI.Repository.DbContext.ApmaDbContext;
using VDVI.Repository.DB;

namespace VDVI.Repository.ApmaRepository.Implementation
{
    public class HcsListRateTypeRepository : IHcsListRateTypeRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbLedgerBalanceHistory> _ledgerBalance;

        public HcsListRateTypeRepository(VDVISchedulerDbContext dbContext)
        {
            _dbContext = dbContext;
            _ledgerBalance = _dbContext.LedgerBalance;
        }

        public async Task<IEnumerable<RateTypeDto>> BulkInsertAsync(IEnumerable<RateTypeDto> dto)
        {
            await _ledgerBalance.BulkInsertAsync(TinyMapper.Map<List<DbLedgerBalanceHistory>>(dto));
            return dto;
        }

        public async Task<string> BulkInsertWithProcAsync(IEnumerable<RateTypeDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));
            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_RateType", new { RateType_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<RateTypeDto> FindByIdAsync(int id)
        {
            var dbEntity = await _ledgerBalance.FindAsync(x => x.PropertyCode == "");
            return TinyMapper.Map<RateTypeDto>(dbEntity);
        }

        public async Task<IEnumerable<RateTypeDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbLedgerBalanceHistory> dbEntities = await _ledgerBalance.SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode).FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<RateTypeDto>>(dbEntities);

            return entities;
        }

        public async Task<RateTypeDto> InsertAsync(RateTypeDto dto)
        {
            await _ledgerBalance.InsertAsync(TinyMapper.Map<DbLedgerBalanceHistory>(dto));
            return dto;
        }

        public async Task<RateTypeDto> UpdateAsync(RateTypeDto dto)
        {
            await _ledgerBalance.UpdateAsync(TinyMapper.Map<DbLedgerBalanceHistory>(dto));
            return dto;
        }

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _ledgerBalance.DeleteAsync(x => x.PropertyCode == propertyCode);

    }
}
