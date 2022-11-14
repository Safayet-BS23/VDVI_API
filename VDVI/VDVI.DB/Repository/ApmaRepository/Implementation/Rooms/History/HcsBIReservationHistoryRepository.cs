using CSharpFunctionalExtensions;
using Dapper;
using Framework.Core.Base.ModelEntity;
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
    public class HcsBIReservationHistoryRepository : DapperRepository<DbReservationDashboardReservationHistory>, IHcsBIReservationHistoryRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbReservationDashboardReservationHistory> _tblRepository;

        public HcsBIReservationHistoryRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.Reservation;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardReservationHistoryDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_ReservationDashboard_Reservation_History",
                            new { ReservationDashboard_Reservation_History_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<ReservationDashboardReservationHistoryDto>> BulkInsertAsync(IEnumerable<ReservationDashboardReservationHistoryDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbReservationDashboardReservationHistory>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

        public async Task<bool> DeleteByDashboardDateAsync(DateTime businessDate) => await _tblRepository.DeleteAsync(x => x.DashboardDate == businessDate);

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<ReservationDashboardReservationHistoryDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<ReservationDashboardReservationHistoryDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<ReservationDashboardReservationHistoryDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbReservationDashboardReservationHistory> dbEntities = await _dbContext
                .Reservation
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<ReservationDashboardReservationHistoryDto>>(dbEntities);

            return entities;
        }

        public async Task<ReservationDashboardReservationHistoryDto> InsertAsync(ReservationDashboardReservationHistoryDto dto)
        {
            var dbEntity = TinyMapper.Map<DbReservationDashboardReservationHistory>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<ReservationDashboardReservationHistoryDto>(dbEntity);
        }

        public async Task<ReservationDashboardReservationHistoryDto> UpdateAsync(ReservationDashboardReservationHistoryDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbReservationDashboardReservationHistory>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
