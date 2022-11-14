using Dapper;
using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.SqlGenerator.Filters;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VDVI.Repository.DbContext.ApmaDbContext;
using VDVI.ApmaRepository.Interfaces;
using VDVI.Repository.DB;
using VDVI.DB.Dtos;

namespace VDVI.Repository.ApmaRepository.Implementation
{
    public class HcsListPackagesRepository : DapperRepository<PackagesDto>, IHcsListPackagesRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbPackages> _tblRepository;

        public HcsListPackagesRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.Packages;
        }
        public async Task<string> BulkInsertWithProcAsync(IEnumerable<PackagesDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_Packages", new { Packages_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<PackagesDto>> BulkInsertAsync(IEnumerable<PackagesDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbPackages>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }
         

        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<PackagesDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<PackagesDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<PackagesDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbPackages> dbEntities = await _dbContext
                .Packages
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<PackagesDto>>(dbEntities);

            return entities;
        }

        public async Task<PackagesDto> InsertAsync(PackagesDto dto)
        {
            var dbEntity = TinyMapper.Map<DbPackages>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<PackagesDto>(dbEntity);
        }

        public async Task<PackagesDto> UpdateAsync(PackagesDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbPackages>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
