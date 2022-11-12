using Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator.Filters;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VDVI.Repository.DbContext.ApmaDbContext;
using VDVI.ApmaRepository.Interfaces;
using VDVI.Repository.DB;
using VDVI.DB.Dtos;

namespace VDVI.Repository.ApmaRepository.Implementation
{
    public class HcsListSourcesRepository : DapperRepository<SourcesDto>, IHcsListSourcesRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbSources> _tblRepository;

        public HcsListSourcesRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.Sources;
        }
        public async Task<string> BulkInsertWithProcAsync(IEnumerable<SourcesDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_Sources", new { Sources_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<SourcesDto>> BulkInsertAsync(IEnumerable<SourcesDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbSources>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }
         

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<SourcesDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<SourcesDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<SourcesDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbSources> dbEntities = await _dbContext
                .Sources
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<SourcesDto>>(dbEntities);

            return entities;
        }

        public async Task<SourcesDto> InsertAsync(SourcesDto dto)
        {
            var dbEntity = TinyMapper.Map<DbSources>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<SourcesDto>(dbEntity);
        }

        public async Task<SourcesDto> UpdateAsync(SourcesDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbSources>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
