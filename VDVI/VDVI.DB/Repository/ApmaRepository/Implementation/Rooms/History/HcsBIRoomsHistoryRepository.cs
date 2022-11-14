using Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator.Filters;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VDVI.Repository.DbContext.ApmaDbContext; 
using VDVI.ApmaRepository.Interfaces;
using VDVI.Repository.DB;
using VDVI.DB.Dtos;

namespace VDVI.Repository.ApmaRepository.Implementation
{
    public class HcsBIRoomsHistoryRepository : DapperRepository<DbReservationDashboardRoomsHistory>, IHcsBIRoomsHistoryRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbReservationDashboardRoomsHistory> _tblRepository;

        public HcsBIRoomsHistoryRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.ReservationDashboardRooms;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<ReservationDashboardRoomsHistoryDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_ReservationDashboard_Rooms_History", new { ReservationDashboard_Rooms_History_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<ReservationDashboardRoomsHistoryDto>> BulkInsertAsync(IEnumerable<ReservationDashboardRoomsHistoryDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbReservationDashboardRoomsHistory>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

        public async Task<bool> DeleteByDashboardDateAsync(DateTime DashboardDate) => await _tblRepository.DeleteAsync(x => x.DashboardDate == DashboardDate);

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<ReservationDashboardRoomsHistoryDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<ReservationDashboardRoomsHistoryDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<ReservationDashboardRoomsHistoryDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbReservationDashboardRoomsHistory> dbEntities = await _dbContext
                .ReservationDashboardRooms
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<ReservationDashboardRoomsHistoryDto>>(dbEntities);

            return entities;
        }

        public async Task<ReservationDashboardRoomsHistoryDto> InsertAsync(ReservationDashboardRoomsHistoryDto dto)
        {
            var dbEntity = TinyMapper.Map<DbReservationDashboardRoomsHistory>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<ReservationDashboardRoomsHistoryDto>(dbEntity);
        }

        public async Task<ReservationDashboardRoomsHistoryDto> UpdateAsync(ReservationDashboardRoomsHistoryDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbReservationDashboardRoomsHistory>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
