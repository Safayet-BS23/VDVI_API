using Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator.Filters;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VDVI.ApmaRepository.Interfaces;
using VDVI.DB.Dtos;
using VDVI.Repository.DB;
using VDVI.Repository.DbContext.ApmaDbContext; 

namespace VDVI.Repository.ApmaRepository.Implementation
{
    public class HcsBIReservationFutureAuditRepository : DapperRepository<DbReservationDashboardReservationFutureAudit>, IHcsBIReservationFutureAuditRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbReservationDashboardReservationFutureAudit> _tblRepository;

        public HcsBIReservationFutureAuditRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.ReservationFutureAudit;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardReservationFutureAuditDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_ReservationDashboard_Reservation_Future_Audit",
                            new { ReservationDashboard_Reservation_History_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<ReservationDashboardReservationFutureAuditDto>> BulkInsertAsync(IEnumerable<ReservationDashboardReservationFutureAuditDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbReservationDashboardReservationFutureAudit>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

        public async Task<bool> DeleteByDashboardDateAsync(DateTime businessDate) => await _tblRepository.DeleteAsync(x => x.DashboardDate == businessDate);

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<ReservationDashboardReservationFutureAuditDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<ReservationDashboardReservationFutureAuditDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<ReservationDashboardReservationFutureAuditDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbReservationDashboardReservationFuture> dbEntities = await _dbContext
                .ReservationFuture
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<ReservationDashboardReservationFutureAuditDto>>(dbEntities);

            return entities;
        }

        public async Task<ReservationDashboardReservationFutureAuditDto> InsertAsync(ReservationDashboardReservationFutureAuditDto dto)
        {
            var dbEntity = TinyMapper.Map<DbReservationDashboardReservationFutureAudit>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<ReservationDashboardReservationFutureAuditDto>(dbEntity);
        }

        public async Task<ReservationDashboardReservationFutureAuditDto> UpdateAsync(ReservationDashboardReservationFutureAuditDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbReservationDashboardReservationFutureAudit>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
