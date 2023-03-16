using System.Collections.Concurrent;
using Innowise.Clinic.Profiles.Persistence.Models;
using Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;

namespace Innowise.Clinic.Profiles.Services;

public static class DoctorStatusCacheService
{
    public static async Task<DoctorStatus> GetDoctorStatus(Guid statusId, IDoctorRepository repository)
    {
        var statusIsCached = StatusCache.TryGetValue(statusId, out var doctorStatus);
        return statusIsCached && doctorStatus is not null 
            ? doctorStatus 
            : await GetDoctorStatusFromDb(statusId, repository);
    }

    private static ConcurrentDictionary<Guid, DoctorStatus> StatusCache { get; } = new();

    private static async Task<DoctorStatus> GetDoctorStatusFromDb(Guid statusId, IDoctorRepository doctorRepository)
    {
        var doctorStatus = await doctorRepository.GetDoctorStatus(statusId);
        StatusCache[statusId] = doctorStatus;
        return doctorStatus;
    }
}