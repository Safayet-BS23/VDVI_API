using Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator.Filters;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VDVI.ApmaRepository.Interfaces;
using VDVI.DB.Dtos;
using VDVI.Repository.DB;
using VDVI.Repository.DbContext.ApmaDbContext;

namespace VDVI.Repository.ApmaRepository.Implementation
{
    public class HcsStatisticsRevenueCodesRepository : DapperRepository<DbStatisticsRevenueCodes>, IHcsStatisticsRevenueCodesRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbStatisticsRevenueCodes> _tblRepository;

        public HcsStatisticsRevenueCodesRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.StatisticsRevenueCode;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<StatisticsRevenueCodesDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_StatisticsRevenueCodes",
                            new { BanquetingRoomTypes_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<StatisticsRevenueCodesDto>> BulkInsertAsync(IEnumerable<StatisticsRevenueCodesDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbStatisticsRevenueCodes>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }


        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<StatisticsRevenueCodesDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<StatisticsRevenueCodesDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<StatisticsRevenueCodesDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbStatisticsRevenueCodes> dbEntities = await _tblRepository
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<StatisticsRevenueCodesDto>>(dbEntities);

            return entities;
        }

        public async Task<StatisticsRevenueCodesDto> InsertAsync(StatisticsRevenueCodesDto dto)
        {
            var dbEntity = TinyMapper.Map<DbStatisticsRevenueCodes>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<StatisticsRevenueCodesDto>(dbEntity);
        }

        public async Task<StatisticsRevenueCodesDto> UpdateAsync(StatisticsRevenueCodesDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbStatisticsRevenueCodes>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
