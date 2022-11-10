using DutchGrit.Afas;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VDVI.Repository.Dtos.AfasDtos.AfasCommonDtos;

namespace VDVI.Services.Configurations
{
    public static class AfasConfiguration
    {
        public static IServiceCollection AddAfasConfig(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient<AfasCrenditalsDto>(provider =>
            {
                return new AfasCrenditalsDto
                {
                    clientAA = new AfasClient(85007, Configuration.GetSection("AfasAuthenticationCredential").GetSection("AA").Value),
                    clientAC = new AfasClient(85007, Configuration.GetSection("AfasAuthenticationCredential").GetSection("AC").Value),
                    clientAD = new AfasClient(85007, Configuration.GetSection("AfasAuthenticationCredential").GetSection("AD").Value),
                    clientAE = new AfasClient(85007, Configuration.GetSection("AfasAuthenticationCredential").GetSection("AE").Value)
                };
            });

            return services;
        }

    }
}
