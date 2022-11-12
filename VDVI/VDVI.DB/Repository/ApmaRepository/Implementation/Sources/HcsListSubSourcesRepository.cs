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
    public class HcsListSubSourcesRepository : DapperRepository<SubSourcesDto>, IHcsListSubSourcesRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbSubSources> _tblRepository;

        public HcsListSubSourcesRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.SubSources;
        }
        public async Task<string> BulkInsertWithProcAsync(IEnumerable<SubSourcesDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_SubSources", new { SubSources_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<SubSourcesDto>> BulkInsertAsync(IEnumerable<SubSourcesDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbSubSources>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }
         

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<SubSourcesDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<SubSourcesDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<SubSourcesDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbSubSources> dbEntities = await _dbContext
                .SubSources
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<SubSourcesDto>>(dbEntities);

            return entities;
        }

        public async Task<SubSourcesDto> InsertAsync(SubSourcesDto dto)
        {
            var dbEntity = TinyMapper.Map<DbSubSources>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<SubSourcesDto>(dbEntity);
        }

        public async Task<SubSourcesDto> UpdateAsync(SubSourcesDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbSubSources>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
