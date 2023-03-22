using Innowise.Clinic.Profiles.Persistence.Models.Interfaces;
using Innowise.Clinic.Profiles.Services.FiltrationService.Filters;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Abstractions;

public interface ICompoundFilter<T> where T : IPersonRelatable
{
    IEnumerable<KeyValuePair<string, string>> Filters { get; set; }
}