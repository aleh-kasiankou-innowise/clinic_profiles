using Innowise.Clinic.Profiles.Dto.Profile.Doctor;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CSharp.RuntimeBinder;

namespace Innowise.Clinic.Profiles.API.ModelBinders;

// TODO REMOVE CODE DUPLICATION

public class DoctorUpdateFormModelBinder : IModelBinder
{
    private const string ModelBindingExceptionMessage =
        "The form has been received but couldn't be processed. " +
        "The format of data doesn't have corresponding model. " +
        "Please recheck the fields of your form and try again.";

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        try
        {
            switch (bindingContext.ValueProvider.GetValue("ModelType").FirstValue)
            {
                case "status_update":
                    await BindToStatusUpdateDto(bindingContext);
                    break;
                case "profile_update":
                    await BindToProfileUpdateDto(bindingContext);
                    break;
                case "profile_create":
                    await BindToNewProfileDto(bindingContext);
                    break;
                default:
                    throw new RuntimeBinderException(ModelBindingExceptionMessage);
            }
        }
        catch (Exception e)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            Console.WriteLine(e);
        }
    }

    private Task BindToStatusUpdateDto(ModelBindingContext bindingContext)
    {
        var statusId = bindingContext.ValueProvider
            .GetValue(nameof(DoctorProfileStatusDto.StatusId))
            .FirstValue;
        var statusIdIsValidGuid = System.Guid.TryParse(statusId, out var statusGuid);
        if (!statusIdIsValidGuid)
        {
            throw new RuntimeBinderException();
        }

        bindingContext.Result = ModelBindingResult.Success(new DoctorProfileStatusDto(statusGuid));
        return Task.CompletedTask;
    }

    private async Task BindToProfileUpdateDto(ModelBindingContext bindingContext)
    {
        var photo =
            (await bindingContext.HttpContext.Request.ReadFormAsync()).Files.GetFile(
                nameof(DoctorProfileUpdateDto.Photo));
        var firstName = GetMandatoryFormValue(bindingContext, nameof(DoctorProfileUpdateDto.FirstName));
        var lastName = GetMandatoryFormValue(bindingContext, nameof(DoctorProfileUpdateDto.LastName));
        var middleName = bindingContext
            .ValueProvider
            .GetValue(nameof(DoctorProfileUpdateDto.MiddleName)
            )
            .FirstValue;
        var dateOfBirth = DateTime
            .Parse(GetMandatoryFormValue(bindingContext, nameof(DoctorProfileUpdateDto.DateOfBirth)));
        var specializationId = Guid
            .Parse(GetMandatoryFormValue(bindingContext, nameof(DoctorProfileUpdateDto.SpecializationId)));
        var officeId = Guid
            .Parse(GetMandatoryFormValue(bindingContext, nameof(DoctorProfileUpdateDto.OfficeId)));
        var careerStartYear = DateTime
            .Parse(GetMandatoryFormValue(bindingContext, nameof(DoctorProfileUpdateDto.CareerStartYear)));
        var statusId = Guid
            .Parse(GetMandatoryFormValue(bindingContext, nameof(DoctorProfileUpdateDto.StatusId)));
        var isToDeletePhotoPresent = Boolean.TryParse(bindingContext
            .ValueProvider
            .GetValue(nameof(DoctorProfileUpdateDto.IsToDeletePhoto)
            ).FirstValue, out var isToDeletePhoto);
        bindingContext.Result = ModelBindingResult.Success(new DoctorProfileUpdateDto(photo, firstName, lastName,
            middleName, dateOfBirth, specializationId, officeId, careerStartYear, statusId,
            isToDeletePhotoPresent && isToDeletePhoto));
    }

    private async Task BindToNewProfileDto(ModelBindingContext bindingContext)
    {
        var photo =
            (await bindingContext.HttpContext.Request.ReadFormAsync()).Files.GetFile(
                nameof(DoctorProfileDto.Photo));
        var firstName = GetMandatoryFormValue(bindingContext, nameof(DoctorProfileDto.FirstName));
        var lastName = GetMandatoryFormValue(bindingContext, nameof(DoctorProfileDto.LastName));
        var middleName = bindingContext
            .ValueProvider
            .GetValue(nameof(DoctorProfileDto.MiddleName)
            )
            .FirstValue;
        var dateOfBirth = DateTime
            .Parse(GetMandatoryFormValue(bindingContext, nameof(DoctorProfileDto.DateOfBirth)));
        var email = GetMandatoryFormValue(bindingContext, nameof(DoctorProfileDto.Email));
        var specializationId = Guid
            .Parse(GetMandatoryFormValue(bindingContext, nameof(DoctorProfileDto.SpecializationId)));
        var officeId = Guid
            .Parse(GetMandatoryFormValue(bindingContext, nameof(DoctorProfileDto.OfficeId)));
        var careerStartYear = DateTime
            .Parse(GetMandatoryFormValue(bindingContext, nameof(DoctorProfileDto.CareerStartYear)));
        var statusId = Guid
            .Parse(GetMandatoryFormValue(bindingContext, nameof(DoctorProfileDto.StatusId)));

        bindingContext.Result = ModelBindingResult.Success(new DoctorProfileDto(photo, firstName, lastName, middleName,
            dateOfBirth, email, specializationId, officeId, careerStartYear, statusId));
    }

    private string GetMandatoryFormValue(ModelBindingContext bindingContext, string key)
    {
        return bindingContext.ValueProvider
            .GetValue(key).FirstValue ?? throw new RuntimeBinderException();
    }
}