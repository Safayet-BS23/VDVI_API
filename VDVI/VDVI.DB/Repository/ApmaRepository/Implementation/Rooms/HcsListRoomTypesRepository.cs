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
    public class HcsListRoomTypesRepository : DapperRepository<DbRoomTypes>, IHcsListRoomTypesRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbRoomTypes> _tblRepository;

        public HcsListRoomTypesRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.RoomTypes;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<RoomTypesDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_RoomTypes",
                            new { RoomTypes_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<RoomTypesDto>> BulkInsertAsync(IEnumerable<RoomTypesDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbRoomTypes>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

     
        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<RoomTypesDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<RoomTypesDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<RoomTypesDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbRoomTypes> dbEntities = await _tblRepository
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<RoomTypesDto>>(dbEntities);

            return entities;
        }

        public async Task<RoomTypesDto> InsertAsync(RoomTypesDto dto)
        {
            var dbEntity = TinyMapper.Map<DbRoomTypes>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<RoomTypesDto>(dbEntity);
        }

        public async Task<RoomTypesDto> UpdateAsync(RoomTypesDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbRoomTypes>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
