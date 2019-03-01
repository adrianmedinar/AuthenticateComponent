﻿using Axity.Security.Core.Dto.UseCaseRequests;
using Axity.Security.Core.Dto.UseCaseResponses;

namespace Axity.Security.Core.Interfaces.UseCases
{
    public interface ILoginUseCase : IUseCaseRequestHandler<LoginRequest, LoginResponse>
    {
    }
}
