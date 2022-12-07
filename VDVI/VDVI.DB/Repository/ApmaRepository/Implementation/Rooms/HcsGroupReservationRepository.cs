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
    public class HcsGroupReservationRepository : DapperRepository<DbGroupReservation>, IHcsGroupReservationRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbGroupReservation> _tblRepository;

        public HcsGroupReservationRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.GroupReservations;
        }

        public async Task<string> BulkInsertWithProcAsync(IEnumerable<GroupReservationDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_GroupReservation",
                            new { GroupReservation_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<GroupReservationDto>> BulkInsertAsync(IEnumerable<GroupReservationDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbGroupReservation>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }


        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<GroupReservationDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<GroupReservationDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<GroupReservationDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbGroupReservation> dbEntities = await _tblRepository
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<GroupReservationDto>>(dbEntities);

            return entities;
        }

        public async Task<GroupReservationDto> InsertAsync(GroupReservationDto dto)
        {
            var dbEntity = TinyMapper.Map<DbGroupReservation>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<GroupReservationDto>(dbEntity);
        }

        public async Task<GroupReservationDto> UpsertAsync(GroupReservationDto dto)
        {
            var dbEntity = TinyMapper.Map<DbGroupReservation>(dto);

            var existing = await _tblRepository.FindAsync(x => x.GroupReservationNumber == dto.GroupReservationNumber && x.PropertyCode == dto.PropertyCode);

            if (existing != null)
            {
                await _tblRepository.DeleteAsync(x => x.GroupReservationNumber == dbEntity.GroupReservationNumber && x.PropertyCode == dto.PropertyCode);
            }

            

            return TinyMapper.Map<GroupReservationDto>(dbEntity);
        }

        public async Task<GroupReservationDto> UpdateAsync(GroupReservationDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbGroupReservation>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
