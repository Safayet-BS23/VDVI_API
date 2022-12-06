using Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator.Filters;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VDVI.ApmaRepository.Interfaces;
using VDVI.DB.Dtos;
using VDVI.Repository.DB;
using VDVI.Repository.DbContext.ApmaDbContext;

namespace VDVI.Repository.ApmaRepository.Implementation
{
    public class RecordsToSyncChangedRepository : DapperRepository<DbRecordsToSyncChanged>, IRecordsToSyncChangedRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbRecordsToSyncChanged> _tblRepository;

        public RecordsToSyncChangedRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.RecordsToSyncChanged;
        }

        public async Task<string> BulkInsertWithProcAsync(IEnumerable<RecordsToSyncChangedDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_RecordsToSyncChanged",
                            new { ReservationDashboard_Reservation_History_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<RecordsToSyncChangedDto>> BulkInsertAsync(IEnumerable<RecordsToSyncChangedDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbRecordsToSyncChanged>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }


        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<RecordsToSyncChangedDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<RecordsToSyncChangedDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<RecordsToSyncChangedDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbRecordsToSyncChanged> dbEntities = await _tblRepository
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<RecordsToSyncChangedDto>>(dbEntities);

            return entities;
        }

        public async Task<RecordsToSyncChangedDto> InsertAsync(RecordsToSyncChangedDto dto)
        {
            var dbEntity = TinyMapper.Map<DbRecordsToSyncChanged>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<RecordsToSyncChangedDto>(dbEntity);
        }

        public async Task<RecordsToSyncChangedDto> UpdateAsync(RecordsToSyncChangedDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbRecordsToSyncChanged>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
