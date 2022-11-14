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
    public class HcsGetFullReservationDetailsRepository : DapperRepository<DbGetFullReservationDetails>, IHcsGetFullReservationDetailsRepository
    {
        private readonly VDVISchedulerDbContext _dbContext;
        private readonly IDapperRepository<DbGetFullReservationDetails> _tblRepository;

        public HcsGetFullReservationDetailsRepository(VDVISchedulerDbContext dbContext) : base(dbContext.Connection)
        {
            _dbContext = dbContext;
            _tblRepository = _dbContext.GetFullReservationDetails;
        }


        public async Task<string> BulkInsertWithProcAsync(IEnumerable<GetFullReservationDetailsDto> dto)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(JsonConvert.SerializeObject(dto));

            var queryResult = await _dbContext.Connection.QueryAsync<string>("spINSERT_hce_GetFullReservationDetails",
                            new { ReservationDashboard_Reservation_History_UDT = dt }, commandType: CommandType.StoredProcedure);

            return queryResult.ToString();
        }

        public async Task<IEnumerable<GetFullReservationDetailsDto>> BulkInsertAsync(IEnumerable<GetFullReservationDetailsDto> dto)
        {
            var dbEntity = TinyMapper.Map<List<DbGetFullReservationDetails>>(dto);

            await _tblRepository.BulkInsertAsync(dbEntity);

            return dto;
        }

     
        public async Task<bool> DeleteByPropertyCodeAsync(string propertyCode) => await _tblRepository.DeleteAsync(x => x.PropertyCode == propertyCode);

        public async Task<GetFullReservationDetailsDto> FindByIdAsync(int id)
        {
            var dbEntity = await _tblRepository.FindAsync(x => x.PropertyCode == "");

            var dto = TinyMapper.Map<GetFullReservationDetailsDto>(dbEntity);

            return dto;
        }

        public async Task<IEnumerable<GetFullReservationDetailsDto>> GetAllByPropertyCodeAsync(string propertyCode)
        {
            IEnumerable<DbGetFullReservationDetails> dbEntities = await _tblRepository
                .SetOrderBy(OrderInfo.SortDirection.DESC, x => x.PropertyCode)
                .FindAllAsync(x => x.PropertyCode == propertyCode);

            var entities = TinyMapper.Map<List<GetFullReservationDetailsDto>>(dbEntities);

            return entities;
        }

        public async Task<GetFullReservationDetailsDto> InsertAsync(GetFullReservationDetailsDto dto)
        {
            var dbEntity = TinyMapper.Map<DbGetFullReservationDetails>(dto);

            await _tblRepository.InsertAsync(dbEntity);

            return TinyMapper.Map<GetFullReservationDetailsDto>(dbEntity);
        }

        public async Task<GetFullReservationDetailsDto> UpdateAsync(GetFullReservationDetailsDto dto)
        {
            var dbCustomerEntity = TinyMapper.Map<DbGetFullReservationDetails>(dto);

            await _tblRepository.UpdateAsync(dbCustomerEntity);

            return dto;
        }
    }
}
