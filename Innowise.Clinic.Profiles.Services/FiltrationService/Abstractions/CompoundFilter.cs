using Innowise.Clinic.Profiles.Persistence.Models.Interfaces;
using Innowise.Clinic.Profiles.Services.FiltrationService.Filters;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Abstractions;

public class CompoundFilter<T> : ICompoundFilter<T> where T : IPersonRelatable
{
    public IEnumerable<KeyValuePair<string, string>> Filters { get; set; }
}