using System.Linq.Expressions;
using Innowise.Clinic.Profiles.Persistence.Models;

namespace Innowise.Clinic.Profiles.Persistence.Repositories.Interfaces;

public interface IDoctorRepository
{
    Task<Doctor> CreateProfileAsync(Doctor doctor);
    Task<Doctor> GetProfileAsync(Guid doctorId);
    Task<IEnumerable<Doctor>> GetDoctorListingAsync(int page, int quantity, Expression<Func<Doctor, bool>>? filter = null);
    Task UpdateProfileAsync(Doctor updatedProfile);
    Task<DoctorStatus> GetDoctorStatus(Guid doctorId);
}