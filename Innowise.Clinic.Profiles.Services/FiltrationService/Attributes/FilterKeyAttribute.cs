using Microsoft.IdentityModel.Tokens;

namespace Innowise.Clinic.Profiles.Services.FiltrationService.Attributes;

public class FilterKeyAttribute : Attribute
{
    public string FilterKey { get; }

    public FilterKeyAttribute(string filterKey)
    {
        if (filterKey.IsNullOrEmpty())
        {
            throw new ApplicationException("The filter key cannot be null or empty");
        }
        
        else if (filterKey == "base")
        {
            throw new ApplicationException($"The filters cannot use keys of a base class : {filterKey}");
        }
        FilterKey = filterKey.ToLower();
    }
}