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
    public class HcsBanquetingRoomsRepository : DapperRepository<DbBanquetingRooms>, IHcsBanquetingRoomsRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbBanquetingRooms> _tblRepository;

        public HcsBanquetingRoomsRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.BanquetingRooms;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<BanquetingRoomsDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_BanquetingRooms",
                            new { BanquetingRooms_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<BanquetingRoomsDto>> BulkInsertAsync(IEnumerable<BanquetingRoomsDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbBanquetingRooms>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

     
        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<BanquetingRoomsDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<BanquetingRoomsDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<BanquetingRoomsDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbBanquetingRooms> dbEntities = await _tblRepository
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<BanquetingRoomsDto>>(dbEntities);

            return entities;
        }

        public async Task<BanquetingRoomsDto> InsertAsync(BanquetingRoomsDto dto)
        {
            var dbEntity = TinyMapper.Map<DbBanquetingRooms>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<BanquetingRoomsDto>(dbEntity);
        }

        public async Task<BanquetingRoomsDto> UpdateAsync(BanquetingRoomsDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbBanquetingRooms>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
