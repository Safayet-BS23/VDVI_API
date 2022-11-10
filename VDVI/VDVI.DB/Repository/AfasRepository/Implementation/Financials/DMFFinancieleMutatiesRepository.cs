using CSharpFunctionalExtensions;
using Dapper;
using Framework.Core.Base.ModelEntity;
using Framework.Core.Extensions;
using MicroOrm.Dapper.Repositories;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using VDVI.AfasRepository.Interfaces;
using VDVI.Repository.AfasDtos;
using VDVI.Repository.AfasModels;
using VDVI.Repository.DB;
using VDVI.Repository.DbContext.AfasDbContext;
using VDVI.Repository.Dtos.AfasDtos.AfasCommonDtos;
using VDVI.Repository.Models.AfasModels;
using VDVI.Repository.Models.AfasModels.Dto;

namespace VDVI.Repository.AfasRepository.Implementation
{

    public class DMFFinancieleMutatiesRepository : DapperRepository<DbDMFFinancieleMutaties>, IDMFFinancieleMutatiesRepository
    {
        private readonly AfasDbContext _dbContext;
        private readonly IDapperRepository<DbDMFFinancieleMutaties> _tblRepository;
        public DMFFinancieleMutatiesRepository(AfasDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.FinancieleMutaties;
        }
        public async Task<DMFFinancieleMutatiesDto> InsertAsync(DMFFinancieleMutatiesDto dto)
        {
            var dbEntity = TinyMapper.Map<DbDMFFinancieleMutaties>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<DMFFinancieleMutatiesDto>(dbEntity);
        }

        public async Task<DMFFinancieleMutatiesDto> UpdateAsync(DMFFinancieleMutatiesDto dto)
        {
            var entities = TinyMapper.Map<DbDMFFinancieleMutaties>(dto);

            var res = await _tblRepository.UpdateAsync(entities);

            return dto;
        }
        public async Task<IEnumerable<DMFFinancieleMutatiesDto>> BulkInsertAsync(IEnumerable<DMFFinancieleMutatiesDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbDMFFinancieleMutaties>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }
        public async Task<string> BulkInsertWithProcAsync(IEnumerable<DMFFinancieleMutatiesDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));
            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_dmf_Financiele_Mutaties",
                new
                {

                    Financiele_Mutaties_UDT = dt
                }, commandType: CommandType.StoredProcedure);
            return queryResult.ToString();
        }

        public async Task<string> BulkInsertWithBoekingsdagenMutatiesAsync(IEnumerable<DMFFinancieleMutatiesDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));
            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_dmf_Financiele_Boekingsdagen_Mutaties",
                new
                { 
                    Financiele_Mutaties_UDT = dt
                }, commandType: CommandType.StoredProcedure);
            return queryResult.ToString();
        }

        

    }
}
