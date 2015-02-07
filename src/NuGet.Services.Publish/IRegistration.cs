﻿
using System.Threading.Tasks;

namespace NuGet.Services.Publish
{
    public interface IRegistration
    {
        Task AddOwner(RegistrationId registrationId, string owner);
        Task RemoveOwner(RegistrationId registrationId, string owner);

        Task Add(PackageId packageId);
        Task Remove(PackageId packageId);
        Task Remove(RegistrationId registrationId);

        Task<bool> Exists(RegistrationId registrationId);
        Task<bool> Exists(PackageId packageId);
        Task<bool> HasOwner(RegistrationId registrationId, string owner);
    }
}
