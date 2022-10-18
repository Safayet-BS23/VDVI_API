﻿using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.Repository.AfasDtos;

namespace VDVI.Services.AfasInterfaces
{
    public interface IdmFFinancieleMutationService
    {
        Task<Result<PrometheusResponse>> InsertAsync(DMFFinancieleMutatiesDto dto);
        Task<Result<PrometheusResponse>> BulkInsertAsync(List<DMFFinancieleMutatiesDto> dtos);
        Task<Result<PrometheusResponse>> BulkInsertWithProcAsync(List<DMFFinancieleMutatiesDto> dto);
    }
}
