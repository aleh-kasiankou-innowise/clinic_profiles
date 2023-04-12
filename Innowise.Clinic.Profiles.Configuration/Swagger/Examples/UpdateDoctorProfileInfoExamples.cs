using Innowise.Clinic.Profiles.Dto.Profile.Doctor;
using Swashbuckle.AspNetCore.Filters;

namespace Innowise.Clinic.Profiles.Configuration.Swagger.Examples;

public class UpdateDoctorProfileInfoExamples : IMultipleExamplesProvider<DoctorProfileStatusDto>
{
    public IEnumerable<SwaggerExample<DoctorProfileStatusDto>> GetExamples()
    {
        DoctorProfileStatusDto patientProfileWithNumberAndPhotoDto = new DoctorProfileUpdateDto(
            null,
            "James", "Sullivan", "Jose",
            new DateTime(1990, 2, 15),
            Guid.Empty, Guid.Empty,
            new DateTime(2015, 2, 15),
            Guid.Parse("e8dfe97a-1c89-45ff-b08c-a97f6edc5e41"));
        yield return SwaggerExample.Create("Update Doctor Profile",
            patientProfileWithNumberAndPhotoDto);

        var patientProfileRecordExample =
            new DoctorProfileStatusDto(Guid.Parse("e8dfe97a-1c89-45ff-b08c-a97f6edc5e41"));
        yield return SwaggerExample.Create("Update Doctor Status", patientProfileRecordExample
        );
    }
}