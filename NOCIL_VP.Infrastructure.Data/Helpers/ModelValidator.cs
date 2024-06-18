using System.ComponentModel.DataAnnotations;


namespace NOCIL_VP.Infrastructure.Data.Helpers
{
    public class ModelValidator
    {
        public ModelValidator()
        {
        }


        public List<string> ValidateModel(object formData)
        {
            List<string> validationMessages = new List<string>();
            ValidateObject(formData, validationMessages);
            return validationMessages;
        }

        private void ValidateObject(object obj, List<string> validationMessages)
        {
            var context = new ValidationContext(obj, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, context, results, true);
            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    validationMessages.Add(validationResult.ErrorMessage);
                }
            }

            // For properties that are complex types, recursively validate
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    var value = property.GetValue(obj);
                    if (value != null)
                    {
                        if (value is IEnumerable<object> enumerableValues)
                        {
                            foreach(var prop in enumerableValues)
                            {
                                ValidateObject(prop, validationMessages);
                            }
                        }
                        else
                        {
                            ValidateObject(value, validationMessages);
                        }
   
                    }
                }
            }
        }
    }
}
