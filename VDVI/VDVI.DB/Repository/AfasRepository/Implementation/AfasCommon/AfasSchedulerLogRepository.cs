using Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator.Filters;
using Nelibur.ObjectMapper;
using System;
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

namespace VDVI.Repository.ApmaRepository.Implementation
{
    public class AfasSchedulerLogRepository : DapperRepository<DbAfasSchedulerLog>, IAfasSchedulerLogRepository
    {
        private readonly AfasDbContext _dbContext;
        private readonly IDapperRepository<DbAfasSchedulerLog> _tblRepository;

        public AfasSchedulerLogRepository(AfasDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.AfasSchedulerLog;
        }
        public async Task<AfasSchedulerLogDto> InsertAsync(AfasSchedulerLogDto dto)
        {
            var dbEntity = TinyMapper.Map<DbAfasSchedulerLog>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<AfasSchedulerLogDto>(dbEntity);
        }

        public async Task<Result<PrometheusResponse>> SaveWithProcAsync(string methodName,int logDayLimits,DateTime currentDateTime)
        {

            var queryResult = await _dbContext.Connection.QueryAsync<string>("sp_dmf_InsertScheduleLog",
                new
                {
                    MethodName = methodName,
                    LogDayLimits = logDayLimits,
                    CurrentDateTime = currentDateTime
                },
                commandType: CommandType.StoredProcedure);

            return new PrometheusResponse { Data = queryResult };
        }
        public Task<IEnumerable<AfasSchedulerLogDto>> BulkInsertAsync(IEnumerable<AfasSchedulerLogDto> dto)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteByMethodNameAsync(string methodName) => await _tblRepository.DeleteAsync(x => x.MethodName == methodName);

        public async Task<bool> DeleteByDateAsync(DateTime executionDate) => await _tblRepository.DeleteAsync(x => x.ExecutionDateTime == executionDate);
        public async Task<AfasSchedulerLogDto> FindByMethodNameAsync(string methodName)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.MethodName == methodName);

            var dto = TinyMapper.Map<AfasSchedulerLogDto>(dbEntity);

            return dto;
        }
        public async Task<IEnumerable<AfasSchedulerLogDto>> GetAllByMethodNameAsync(string methodName)
        {
            IEnumerable<DbAfasSchedulerLog> dbEntities = await _dbContext
                .AfasSchedulerLog
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.MethodName)
                .FindAllAsync(x => x.MethodName==methodName);

            var entities = TinyMapper.Map<List<AfasSchedulerLogDto>>(dbEntities);

            return entities;
        }
        public async Task<AfasSchedulerLogDto> UpdateAsync(AfasSchedulerLogDto dto)
        {
            var entity = TinyMapper.Map<DbAfasSchedulerLog>(dto);

            await _tblRepository.UpdateAsync(entity);

            return dto;
        }

        
    }
}
