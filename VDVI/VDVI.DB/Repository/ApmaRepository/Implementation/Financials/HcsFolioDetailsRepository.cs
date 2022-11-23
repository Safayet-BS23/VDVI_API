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
    public class HcsFolioDetailsRepository : DapperRepository<DbFolioDetail>, IHcsFolioDetailsRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbFolioDetail> _tblRepository;

        public HcsFolioDetailsRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.FolioDetails;
        }

        public async Task<string> BulkInsertWithProcAsync(IEnumerable<FolioDetailDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_FolioDetails",
                            new { RoomTypes_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<FolioDetailDto>> BulkInsertAsync(IEnumerable<FolioDetailDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbFolioDetail>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }


        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<FolioDetailDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<FolioDetailDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<FolioDetailDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbFolioDetail> dbEntities = await _tblRepository
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<FolioDetailDto>>(dbEntities);

            return entities;
        }

        public async Task<FolioDetailDto> InsertAsync(FolioDetailDto dto)
        {
            var dbEntity = TinyMapper.Map<DbFolioDetail>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<FolioDetailDto>(dbEntity);
        }

        public async Task<FolioDetailDto> UpdateAsync(FolioDetailDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbFolioDetail>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
