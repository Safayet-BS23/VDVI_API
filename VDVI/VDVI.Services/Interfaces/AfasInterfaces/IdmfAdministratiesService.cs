using CSharpFunctionalExtensions;
using Framework.Core.Base.ModelEntity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VDVI.DB.Dtos;
using VDVI.Repository.AfasDtos;

namespace VDVI.Services.AfasInterfaces
{
    public interface IdmfAdministratiesService
    {
        Task<Result<PrometheusResponse>> DmfAdministratiesAsync();
        Task<Result<PrometheusResponse>> GetDmfAdministratiesAsync();
    }
}
