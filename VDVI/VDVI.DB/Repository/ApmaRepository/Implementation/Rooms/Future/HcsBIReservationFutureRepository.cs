using Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator.Filters;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using VDVI.ApmaRepository.Interfaces;
using VDVI.DB.Dtos;
using VDVI.Repository.DB;
using VDVI.Repository.DbContext.ApmaDbContext; 

namespace VDVI.Repository.ApmaRepository.Implementation
{
    public class HcsBIReservationFutureRepository : DapperRepository<DbReservationDashboardReservationFuture>, IHcsBIReservationFutureRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbReservationDashboardReservationFuture> _tblRepository;

        public HcsBIReservationFutureRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.ReservationFuture;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardReservationFutureDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_ReservationDashboard_Reservation_Future",
                            new { ReservationDashboard_Reservation_Future_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<ReservationDashboardReservationFutureDto>> BulkInsertAsync(IEnumerable<ReservationDashboardReservationFutureDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbReservationDashboardReservationFuture>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

        public async Task<bool> DeleteByDashboardDateAsync(DateTime businessDate) => await _tblRepository.DeleteAsync(x => x.DashboardDate == businessDate);

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<ReservationDashboardReservationFutureDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<ReservationDashboardReservationFutureDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<ReservationDashboardReservationFutureDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbReservationDashboardReservationFuture> dbEntities = await _dbContext
                .ReservationFuture
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<ReservationDashboardReservationFutureDto>>(dbEntities);

            return entities;
        }

        public async Task<ReservationDashboardReservationFutureDto> InsertAsync(ReservationDashboardReservationFutureDto dto)
        {
            var dbEntity = TinyMapper.Map<DbReservationDashboardReservationFuture>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<ReservationDashboardReservationFutureDto>(dbEntity);
        }

        public async Task<ReservationDashboardReservationFutureDto> UpdateAsync(ReservationDashboardReservationFutureDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbReservationDashboardReservationFuture>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
