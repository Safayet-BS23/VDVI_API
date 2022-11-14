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
    public class HcsListMealPlansRepository : DapperRepository<DbMealPlans>, IHcsListMealPlansRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbMealPlans> _tblRepository;

        public HcsListMealPlansRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.MealPlans;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<MealPlansDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_MealPlans",
                            new { BanquetingRooms_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<MealPlansDto>> BulkInsertAsync(IEnumerable<MealPlansDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbMealPlans>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

     
        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<MealPlansDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<MealPlansDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<MealPlansDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbMealPlans> dbEntities = await _tblRepository
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<MealPlansDto>>(dbEntities);

            return entities;
        }

        public async Task<MealPlansDto> InsertAsync(MealPlansDto dto)
        {
            var dbEntity = TinyMapper.Map<DbMealPlans>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<MealPlansDto>(dbEntity);
        }

        public async Task<MealPlansDto> UpdateAsync(MealPlansDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbMealPlans>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
