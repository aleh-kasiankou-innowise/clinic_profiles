using Innowise.Clinic.Profiles.Dto.Profile.Patient;
using Swashbuckle.AspNetCore.Filters;

namespace Innowise.Clinic.Profiles.Configuration.Swagger.Examples;

public class CreatePatientProfileExamples : IMultipleExamplesProvider<PatientProfileDto>
{
    public IEnumerable<SwaggerExample<PatientProfileDto>> GetExamples()
    {
        var patientProfileRecordExample = new PatientProfileDto("John", "Doe", "Mariano", new DateTime(1990, 2, 15));
        yield return SwaggerExample.Create("Create Profile by Receptionist", patientProfileRecordExample
        );

        PatientProfileDto patientProfileWithNumberAndPhotoDto = new PatientProfileWithNumberAndPhotoDto("John", "Doe",
            "Mariano",
            new DateTime(1990, 2, 15),
            "8-800-555-35-35", "photo"u8.ToArray());
        yield return SwaggerExample.Create("Create Profile by Patient",
            patientProfileWithNumberAndPhotoDto);
    }
}