using Dapper;
using MicroOrm.Dapper.Repositories; 
using Nelibur.ObjectMapper;  
using System.Collections.Generic;
using System.Data; 
using System.Threading.Tasks;
using VDVI.ApmaRepository.Interfaces;
using VDVI.Repository.DbContext.ApmaDbContext;  
using VDVI.Repository.DB; 
using VDVI.DB.Dtos;
using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using VDVI.Repository.DbContext.AfasDbContext;
using VDVI.Repository.Models.AfasModel;
using VDVI.Repository.Models.AfasModels.Dto;
using VDVI.AfasRepository.Interfaces;
using Framework.Core.Enums;

namespace VDVI.Repository.Implementation
{
    public class AfasSchedulerSetupRepository : DapperRepository<DbAfasSchedulerSetup>, IAfasSchedulerSetupRepository
    {
        private readonly AfasDbContext _dbContext;
        private readonly IDapperRepository<DbAfasSchedulerSetup> _tblRepository;
        public AfasSchedulerSetupRepository(AfasDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.AfasSchedulerSetup;
        }
        public async Task<AfasSchedulerSetupDto> InsertAsync(AfasSchedulerSetupDto dto)
        {
            var dbEntity = TinyMapper.Map<DbAfasSchedulerSetup>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<AfasSchedulerSetupDto>(dbEntity);
        }
         public async Task<AfasSchedulerSetupDto> UpdateAsync(AfasSchedulerSetupDto dto,string schedulerName)
        {
            var entities = TinyMapper.Map<DbAfasSchedulerSetup>(dto);

            var res=await _tblRepository.UpdateAsync(p => p.SchedulerName == schedulerName,  entities);

            return dto;
        }
        public async Task<Result<PrometheusResponse>> SaveWithProcAsync(AfasSchedulerSetupDto dto)
        {
            var queryResult = await _dbContext.Connection.QueryAsync<string>("sp_dmf_UpdateTaskScheduleDatetime",
                new
                {
                    SchedulerName = dto.SchedulerName,
                    NextExecutionDateTime = dto.NextExecutionDateTime,
                    LastExecutionDateTime = dto.LastExecutionDateTime,
                    SchedulerStatus=dto.SchedulerStatus
                },
                commandType: CommandType.StoredProcedure);

            return new PrometheusResponse { Data = queryResult };
        }
        public async Task<IEnumerable<AfasSchedulerSetupDto>> FindByAllScheduleAsync()
        {
            var result = await _dbContext.Connection.QueryAsync<AfasSchedulerSetupDto>("sp_dmf_GetSchedulers",
                 commandType: CommandType.StoredProcedure);

            return result;


        }

        public async Task ResetScheduleStatusAsync()
        {
            var result = await _dbContext.Connection.QueryAsync<SchedulerSetupDto>("[dbo].[sp_dmf_ResetSchedulerStatus]",
                 commandType: CommandType.StoredProcedure);
        }
        public async Task<AfasSchedulerSetupDto> FindByIdAsync(string schedulerName)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.SchedulerName == schedulerName);

            var dto = TinyMapper.Map<AfasSchedulerSetupDto>(dbEntity);

            return dto;
        }
        public async Task<bool> DeleteByPropertyCodeAsync(string schedulerName) => await _tblRepository.DeleteAsync(x => x.SchedulerName == schedulerName);
    }
}
