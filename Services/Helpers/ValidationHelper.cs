using System.ComponentModel.DataAnnotations;

namespace Services.Helpers;

public static class ValidationHelper
{
    public static void ModelValidation<T>(T model)
    {
        if (model == null)
        {
            throw new NullReferenceException(nameof(model));
        }

        ValidationContext validationContext = new ValidationContext(model);
        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

        if (!isValid)
        {
            throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage, nameof(model));
        }
    }
}
