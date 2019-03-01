using System;
using System.Threading.Tasks;
using Axity.Security.Core.Dto;

namespace Axity.Security.Core.Interfaces.Services
{
    public interface IJwtFactory
    {
        Task<AccessToken> GenerateEncodedToken(string id, string userName);
        double LifeTimeToken();
    }
}
