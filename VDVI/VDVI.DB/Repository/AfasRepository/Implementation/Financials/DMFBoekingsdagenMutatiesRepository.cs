using Dapper;
using MicroOrm.Dapper.Repositories;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VDVI.AfasRepository.Interfaces;
using VDVI.Repository.AfasDtos;
using VDVI.Repository.AfasModels;
using VDVI.Repository.DbContext.AfasDbContext;
using VDVI.Repository.Dtos.AfasDtos;
using VDVI.Repository.Dtos.AfasDtos.AfasCommonDtos;

namespace VDVI.Repository.AfasRepository.Implementation
{

    public class DMFBoekingsdagenMutatiesRepository : DapperRepository<DbDMFBoekingsdagenMutaties>, IDMFBoekingsdagenMutatiesRepository
    {
        private readonly AfasDbContext _dbContext;
        private readonly IDapperRepository<DbDMFBoekingsdagenMutaties> _tblRepository;
        public DMFBoekingsdagenMutatiesRepository(AfasDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.BoekingsdagenMutaties;
        }
        public async Task<DMFBoekingsdagenMutatiesDto> InsertAsync(DMFBoekingsdagenMutatiesDto dto)
        {
            var dbEntity = TinyMapper.Map<DbDMFBoekingsdagenMutaties>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<DMFBoekingsdagenMutatiesDto>(dbEntity);
        }

        public async Task<DMFBoekingsdagenMutatiesDto> UpdateAsync(DMFBoekingsdagenMutatiesDto dto)
        {
            var entities = TinyMapper.Map<DbDMFBoekingsdagenMutaties>(dto);

            var res = await _tblRepository.UpdateAsync(entities);

            return dto;
        }
        public async Task<IEnumerable<DMFBoekingsdagenMutatiesDto>> BulkInsertAsync(IEnumerable<DMFBoekingsdagenMutatiesDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbDMFBoekingsdagenMutaties>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }
        public async Task<string> BulkInsertWithProcAsync(IEnumerable<DMFBoekingsdagenMutatiesDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto)); 
            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_dmf_Boekingsdagen_Mutaties",
                new
                {
                    BoekingsdagenMutaties_UDT = dt
                }, commandType: CommandType.StoredProcedure);
            return queryResult.ToString();
        } 

    }
}
