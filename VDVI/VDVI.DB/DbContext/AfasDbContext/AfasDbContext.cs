using Framework.Core.Repository;
using MicroOrm.Dapper.Repositories;
using Microsoft.Extensions.Configuration;
using Nelibur.ObjectMapper;
using System.Collections.Generic;
using System.Data.SqlClient;
using VDVI.Repository.AfasDtos;
using VDVI.Repository.AfasModels;
using VDVI.Repository.Dtos.AfasDtos;
using VDVI.Repository.Models.AfasModel;
using VDVI.Repository.Models.AfasModels;
using VDVI.Repository.Models.AfasModels.Dto;

namespace VDVI.Repository.DbContext.AfasDbContext
{
    public class AfasDbContext : ProDbContext, IAfasDbContext
    {
        private IDapperRepository<DbDMFAdministraties> _administraties;
        private IDapperRepository<DbAfasSchedulerSetup> _afasSchedulerSetup;
        private IDapperRepository<DbAfasSchedulerLog> _afasSchedulerLog;
        private IDapperRepository<DbDMFBeginbalans> _beginbalans;
        private IDapperRepository<DbDMFGrootboekrekeningen> _grootboekrekeningen;
        private IDapperRepository<DbDMFFinancieleMutaties> _financieleMutaties;
        private IDapperRepository<DbDMFBoekingsdagenMutaties> _boekingsdagenMutaties;

        public AfasDbContext(IConfiguration configuration) : base(new SqlConnection(configuration["ConnectionStrings:AfasDb"]))
        {
            // Dto to Db - Single
            TinyMapper.Bind<DMFAdministratiesDto, DbDMFAdministraties>();           
            TinyMapper.Bind<AfasSchedulerSetupDto, DbAfasSchedulerSetup>(); 
            TinyMapper.Bind<AfasSchedulerLogDto, DbAfasSchedulerLog>();
            TinyMapper.Bind<DMFBeginbalansDto, DbDMFBeginbalans>();
            TinyMapper.Bind<DMFGrootboekrekeningenDto, DbDMFGrootboekrekeningen>();
            TinyMapper.Bind<DMFFinancieleMutatiesDto, DbDMFFinancieleMutaties>();
            TinyMapper.Bind<DMFBoekingsdagenMutatiesDto, DbDMFBoekingsdagenMutaties>();

            // Dto to Db List
            TinyMapper.Bind<List<DMFAdministratiesDto>, List<DbDMFAdministraties>>();
            TinyMapper.Bind<List<DMFBeginbalansDto>, List<DbDMFBeginbalans>>();  
            TinyMapper.Bind<List<DMFGrootboekrekeningenDto>, List<DbDMFGrootboekrekeningen>>();
            TinyMapper.Bind<List<DMFFinancieleMutatiesDto>, List<DbDMFFinancieleMutaties>>();
            TinyMapper.Bind<List<DMFBoekingsdagenMutatiesDto>,List<DbDMFBoekingsdagenMutaties>>();

            //Db to Dto -Single 
            TinyMapper.Bind<DbAfasSchedulerSetup, AfasSchedulerSetupDto>();

            //Db to Dto List

            TinyMapper.Bind<List<DbAfasSchedulerSetup>, List<AfasSchedulerSetupDto>>();


        }
        public IDapperRepository<DbDMFBoekingsdagenMutaties> BoekingsdagenMutaties => _boekingsdagenMutaties ??= new DapperRepository<DbDMFBoekingsdagenMutaties>(Connection);
        public IDapperRepository<DbDMFFinancieleMutaties> FinancieleMutaties => _financieleMutaties ??= new DapperRepository<DbDMFFinancieleMutaties>(Connection);
        public IDapperRepository<DbDMFGrootboekrekeningen> Grootboekrekeningen => _grootboekrekeningen ??= new DapperRepository<DbDMFGrootboekrekeningen>(Connection);
        public IDapperRepository<DbDMFBeginbalans> Beginbalans => _beginbalans ??= new DapperRepository<DbDMFBeginbalans>(Connection);
        public IDapperRepository<DbDMFAdministraties> Administraties => _administraties ??= new DapperRepository<DbDMFAdministraties>(Connection);
        public IDapperRepository<DbAfasSchedulerSetup> AfasSchedulerSetup => _afasSchedulerSetup ??= new DapperRepository<DbAfasSchedulerSetup>(Connection);
        public IDapperRepository<DbAfasSchedulerLog> AfasSchedulerLog => _afasSchedulerLog ??= new DapperRepository<DbAfasSchedulerLog>(Connection);
    }
}
