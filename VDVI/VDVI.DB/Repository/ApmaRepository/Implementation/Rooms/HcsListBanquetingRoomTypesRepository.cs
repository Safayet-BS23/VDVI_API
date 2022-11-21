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
    public class HcsListBanquetingRoomTypesRepository : DapperRepository<DbBanquetingRoomTypes>, IHcsListBanquetingRoomTypesRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbBanquetingRoomTypes> _tblRepository;

        public HcsListBanquetingRoomTypesRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.BanquetingRoomTypes;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<BanquetingRoomTypesDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_BanquetingRoomTypes",
                            new { BanquetingRoomTypes_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<BanquetingRoomTypesDto>> BulkInsertAsync(IEnumerable<BanquetingRoomTypesDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbBanquetingRoomTypes>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

     
        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<BanquetingRoomTypesDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<BanquetingRoomTypesDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<BanquetingRoomTypesDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbBanquetingRoomTypes> dbEntities = await _tblRepository
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<BanquetingRoomTypesDto>>(dbEntities);

            return entities;
        }

        public async Task<BanquetingRoomTypesDto> InsertAsync(BanquetingRoomTypesDto dto)
        {
            var dbEntity = TinyMapper.Map<DbBanquetingRoomTypes>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<BanquetingRoomTypesDto>(dbEntity);
        }

        public async Task<BanquetingRoomTypesDto> UpdateAsync(BanquetingRoomTypesDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbBanquetingRoomTypes>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
