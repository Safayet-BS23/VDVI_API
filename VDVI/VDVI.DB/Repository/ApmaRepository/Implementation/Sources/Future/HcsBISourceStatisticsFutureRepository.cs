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
    public class HcsBISourceStatisticsFutureRepository : DapperRepository<SourceStatisticFutureDto>, IHcsBISourceStatisticsFutureRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbSourceStatisticFuture> _tblRepository;

        public HcsBISourceStatisticsFutureRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.SourceStatisticFuture;
        }
        public async Task<string> BulkInsertWithProcAsync(IEnumerable<SourceStatisticFutureDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_SourceStatistics_Future", new { SourceStatistics_Future_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<SourceStatisticFutureDto>> BulkInsertAsync(IEnumerable<SourceStatisticFutureDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbSourceStatisticFuture>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

        public async Task<bool> DeleteByBusinessDateAsync(DateTime businessDate) => await _tblRepository.DeleteAsync(x => x.BusinessDate == businessDate);

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<SourceStatisticFutureDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<SourceStatisticFutureDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<SourceStatisticFutureDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbSourceStatisticFuture> dbEntities = await _dbContext
                .SourceStatisticFuture
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<SourceStatisticFutureDto>>(dbEntities);

            return entities;
        }

        public async Task<SourceStatisticFutureDto> InsertAsync(SourceStatisticFutureDto dto)
        {
            var dbEntity = TinyMapper.Map<DbSourceStatisticFuture>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<SourceStatisticFutureDto>(dbEntity);
        }

        public async Task<SourceStatisticFutureDto> UpdateAsync(SourceStatisticFutureDto dto)
        {
            var dbEntity = TinyMapper.Map<DbSourceStatisticFuture>(dto);

            await _tblRepository.UpdateAsync(dbEntity);

            return dto;
        }
    }
}
