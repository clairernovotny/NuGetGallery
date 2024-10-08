// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Validators;
using NuGet.Services.Entities;

#nullable enable

namespace NuGetGallery.Services.Authentication
{
    public class FederatedCredentialEvalutator
    {
        public async Task<FederatedCredentialResult> GetMatchingPolicyAsync(IReadOnlyList<FederatedCredentialPolicy> policies, string bearerToken)
        {
            JsonWebToken jwt;
            try
            {
                jwt = new JsonWebToken(bearerToken);
            }
            catch (ArgumentException ex)
            {
                return FederatedCredentialResult.InvalidJsonWebToken(ex.Message);
            }

            FederatedCredentialResult? result = null;
            foreach (var policy in policies)
            {
                switch (policy.Type)
                {
                    case FederatedCredentialType.EntraIdServicePrincipal:
                        result = await EvaluateEntraIdServicePrincipalToken(policy, jwt);
                        break;
                    default:
                        throw new NotImplementedException("Unsupported federated credential type: {policy.Type}");
                }
            }

            if (result is null)
            {
                return FederatedCredentialResult.NoMatchingPolicy();
            }

            return result;
        }

        private const string EntraIdAuthority = "https://login.microsoftonline.com/";

        private Task<FederatedCredentialResult> EvaluateEntraIdServicePrincipalToken(FederatedCredentialPolicy policy, JsonWebToken token)
        {
            var handler = new JsonWebTokenHandler();
            handler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                IssuerValidator = AadIssuerValidator.GetAadIssuerValidator(EntraIdAuthority).Validate,
            });
            throw new NotImplementedException();
        }
    }

    public enum FederatedCredentialResultType
    {
        InvalidJsonWebToken,
        NoMatchingPolicy,
    }

    public class FederatedCredentialResult
    {
        private FederatedCredentialResult(FederatedCredentialResultType type, string? errorMessage)
        {
            Type = type;
            ErrorMessage = errorMessage;
        }

        public FederatedCredentialResultType Type { get; }
        public string? ErrorMessage { get; }

        public static FederatedCredentialResult InvalidJsonWebToken(string errorMessage)
        {
            return new FederatedCredentialResult(FederatedCredentialResultType.InvalidJsonWebToken, errorMessage);
        }

        public static FederatedCredentialResult NoMatchingPolicy()
        {
            return new FederatedCredentialResult(FederatedCredentialResultType.NoMatchingPolicy, errorMessage: null);
        }
    }

    public class FederatedCredentialService
    {
        private readonly IEntityRepository<FederatedCredentialPolicy> _policyRepository;

        public FederatedCredentialService(IEntityRepository<FederatedCredentialPolicy> policyRepository)
        {
            _policyRepository = policyRepository ?? throw new ArgumentNullException(nameof(policyRepository));
        }


    }
}
